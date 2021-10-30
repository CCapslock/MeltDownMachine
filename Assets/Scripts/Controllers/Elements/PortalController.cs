using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mans
{
    internal sealed class PortalController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("PortalController");
        private IListGameObject _viewListGameObject;
        private IInteractive _interactive;
        private Collider _portalCollider;
        private PackInteractiveData packInteractiveData;
        private float _portalTime;
        private const float _timeRetryPortal = 0.1f;
        private AudioSource audioSource;

        internal PortalController(IInteractive interactive, IUnitView iUnitView, IListGameObject viewListGameObject)
        {
            _portalCollider = iUnitView.ObjectTransform.GetComponent<Collider>();
            if (_portalCollider == null) Debug.LogWarning($"Dont Set portal collider:{iUnitView.ObjectTransform.name}");
            _interactive = interactive;
            _viewListGameObject = viewListGameObject;
            _interactive.evtTrigger += Enter;
            _interactive.evtAttack += Attack;
            packInteractiveData = new PackInteractiveData(0, TypeUnit.Portal,0);
            audioSource = iUnitView.ObjectTransform.GetComponent<AudioSource>();
        }

        private (int, bool) Attack(PackInteractiveData arg)
        {
            if (arg.typeUnit == TypeUnit.Portal)
                _portalTime = Time.time;
            return (0, false);
        }

        protected override void OnDispose()
        {
            _interactive.evtTrigger -= Enter;
        }

        private void Enter(Collider collider, bool isEnter)
        {
            if (!isEnter || _portalTime + _timeRetryPortal > Time.time) return;

            if (_viewListGameObject.ListGameObjects.Length > 0 && collider.attachedRigidbody.TryGetComponent<IUnitView>(out IUnitView unitView))
            {
                var setListGameObject = _viewListGameObject.ListGameObjects[Random.Range(0, _viewListGameObject.ListGameObjects.Length)];

                if (setListGameObject.TryGetComponent<IInteractive>(out IInteractive interactivePortal))
                {
                    interactivePortal.Attack(packInteractiveData);
                }
                unitView.ObjectTransform.position = setListGameObject.transform.position;
                unitView.ObjectTransform.rotation = setListGameObject.transform.rotation;
                audioSource.Play();
            }
        }
    }
}
