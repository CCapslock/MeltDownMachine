using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mans
{
    public class UnitView : MonoBehaviour, IInteractive, IUnitView
    {

        #region Variables

        [SerializeField] private TypeUnit _typeItem;
        [SerializeField] private int _numCfg = 0;
        public BuilderCfgBasic BuilderConfig => _builderConfig;
        [SerializeField] private BuilderCfgBasic _builderConfig;

        public event Action<Collider, bool> evtTrigger = delegate { };
        public event Action<IInteractive, bool> evtInteractive = delegate { };
        public event Action<bool> evtAnyCollision = delegate { };
        public event Action<Vector3> evtCollided = delegate { };

        public Transform ObjectTransform => _objectTransform;
        private Transform _objectTransform;
        public Rigidbody ObjectRigidbody => _objectRigidbody;
        private Rigidbody _objectRigidbody;

        private Dictionary<int, int> _listCollisionEnter = new Dictionary<int, int>();

        private List<Func<PackInteractiveData, (int, bool)>> _evtAttack = new List<Func<PackInteractiveData, (int, bool)>>();

        event Func<PackInteractiveData, (int, bool)> IInteractive.evtAttack
        {
            add=> _evtAttack.Add(value);
            remove=> _evtAttack.Remove(value);
        }

        #endregion


        #region Init

        private void Awake()
        {
            if (_objectTransform == null) _objectTransform = transform;
            if (_objectRigidbody==null) _objectRigidbody = GetComponent<Rigidbody>();            
        }

        #endregion


        #region Utils

        public (TypeUnit type, int cfg) GetTypeItem()
        {
            return (_typeItem, _numCfg);
        }

        public void SetTypeItem(TypeUnit type = TypeUnit.None, int cfg = -1)
        {
            if (cfg == -1) cfg = _numCfg;
            if (type == TypeUnit.None) type = _typeItem;
            _typeItem = type; _numCfg = cfg;
        }


        void IInteractive.Kill()
        {
            Destroy(gameObject);
        }

        #endregion


        #region Interaction

        private void OnCollisionEnter(Collision collision)
        {
            evtAnyCollision.Invoke(true);
            Interactive(collision.gameObject);

            if(collision.transform.TryGetComponent<TagPlayerOrEnemy>(out TagPlayerOrEnemy _))
                evtCollided(collision.relativeVelocity);
        }

        private void Interactive(GameObject gameObjectIn)
        {
            var ID = gameObjectIn.GetInstanceID();
            _listCollisionEnter[ID] = _listCollisionEnter.ContainsKey(ID) ? _listCollisionEnter[ID] + 1 : 1;

            if (/*_listCollisionEnter[ID] == 1 &&*/ gameObjectIn.TryGetComponent<IInteractive>(out IInteractive unitInteractive))
            {
                evtInteractive.Invoke(unitInteractive, true);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            evtAnyCollision.Invoke(false);
            var ID = collision.gameObject.GetInstanceID();
            _listCollisionEnter[ID] = _listCollisionEnter.ContainsKey(ID) ? _listCollisionEnter[ID] - 1 : 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            evtTrigger.Invoke(other,true);
            Interactive(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            evtTrigger.Invoke(other,false);
        }

        public (int,bool) Attack(PackInteractiveData data)
        {
            int addScores = 0;
            bool isDead=false;
            foreach (var item in _evtAttack)
            {
                (var addScoresTmp, var isDeadTmp) = item(data);
                addScores += addScoresTmp;
                if (isDeadTmp) isDead = true;
            }
            return (addScores,isDead);
        }

        #endregion
    }
}