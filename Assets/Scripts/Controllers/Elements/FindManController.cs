using System.Collections;
using System.Linq;
using UnityEngine;

namespace Mans
{
    internal sealed class FindManController : ControllerBasic, IExecute
    {
        private const float _constTiemUpdateFindMan = 1f;
        private ControlLeak _controlLeak = new ControlLeak("FindManController");
        private ControlModel _controlModel;
        private IUnitView _unitView;
        private IEnumerator _currentCoroutine;
        private ManCfg _manCfg;

        internal FindManController(ControlModel controlModel, IUnitView unitView,ManCfg manCfg)
        {
            _controlModel = controlModel;
            _unitView = unitView;
            _manCfg = manCfg;
            _controlModel.ReachedTarget.Subscribe(ReachedTarget);
            _currentCoroutine = CoroutinesView.inst.AddCoroutine(Wait());
        }

        private void ReachedTarget(bool isReachedTarget)
        {
            if (isReachedTarget)
            {                
                SetRandomTarget();
            }
        }

        protected override void OnDispose() =>
            CoroutinesView.inst.RemoveCoroutine(_currentCoroutine);


        private void SetRandomTarget()
        {
            var randomForFindMan = Random.Range(0, 1f);
            if (randomForFindMan>_manCfg.ChanceFindMan)
            {
                _currentCoroutine = CoroutinesView.inst.AddCoroutine(Wait());
                return;
            }

            var findUnits = Reference.GameModel.GetUnitsView((TypeUnit.Man, 0)).Where(x => x != _unitView).ToList();
            var findPlayers = Reference.GameModel.GetUnitsView((TypeUnit.Player, 0)).ToList();
            findUnits.AddRange(findPlayers);

            if (findUnits.Count > 0)             
            {
                var rnd = Random.Range(0, findUnits.Count);
                _controlModel.TargetPosition.Value = findUnits[rnd].ObjectTransform;
                Debug.Log($"Set random target for:{_unitView.ObjectTransform.parent.name}");
            }
            else Debug.LogWarning($"findUnits.Length=0 {_unitView.ObjectTransform.parent.name}");
        }


        private void SetNearestTarget()
        {
            float minDistance = float.MaxValue;
            IUnitView manUnitView = null;

            var findUnits = Reference.GameModel.GetUnitsView((TypeUnit.Man, 0)).Where(x=>x!=_unitView);

            foreach (var item in findUnits)
            {
                if (item.ObjectTransform == null) Debug.LogWarning($"item.ObjectTransform==null");
                if (_unitView.ObjectTransform == null) Debug.LogWarning($"_unitView.ObjectTransform==null");
                var distance = Utils.SqrDistance(item.ObjectTransform.position, _unitView.ObjectTransform.position);

                if (distance < _manCfg.MaxLenghthFindMan * _manCfg.MaxLenghthFindMan && minDistance > distance)
                {
                    manUnitView = item;
                    minDistance = distance;
                }
            }
            if (minDistance != float.MaxValue)
                _controlModel.TargetPosition.Value = manUnitView.ObjectTransform;
            else
                _currentCoroutine = CoroutinesView.inst.AddCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(_constTiemUpdateFindMan);
            SetRandomTarget();
        }

        public void Execute(float deltaTime)
        {
            if (!_controlModel.ReachedTarget.Value)
            {
                var distance = Utils.SqrDistance(_controlModel.TargetPosition.Value.position, _unitView.ObjectTransform.position);
                if (distance > _manCfg.MaxLenghthFindMan * _manCfg.MaxLenghthFindMan) _controlModel.ReachedTarget.Value = true;
                else if (distance < _manCfg.MinLenghthAttack * _manCfg.MinLenghthAttack)
                    _controlModel.Control.Value = _controlModel.Control.Value.Change(y: 1f);
            }
        }
    }
}
