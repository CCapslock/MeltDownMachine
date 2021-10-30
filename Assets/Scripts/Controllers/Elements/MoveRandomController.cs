using System.Collections;
using UnityEngine;

namespace Mans
{
    internal sealed class MoveRandomController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak("MoveExtendController");
        ControlModel _controlModel;
        private DataUnitCfg _dataUnitCfg;
        private IEnumerator _coroutineRotate;
        private UnitModel _unitModel;

        internal MoveRandomController(ControlModel controlModel,UnitModel unitModel , DataUnitCfg dataUnitCfg)
        {
            _controlModel = controlModel;
            _unitModel = unitModel;
            _dataUnitCfg = dataUnitCfg;
        }

        public void Execute(float deltaTime)
        {
            if (!_dataUnitCfg.PowerRandom.Equals(0))
            {
                float x = 0;
                if (Random.Range(0f, 1f) < _dataUnitCfg.RandomMove * deltaTime)
                    x = Mathf.Sign(Random.Range(-1f, 1f)) * _dataUnitCfg.PowerRandom;
                if (!_controlModel.Control.Value.x.Equals(x))
                    _controlModel.Control.Value = new Vector2(x, _controlModel.Control.Value.y);
            }
            if (_unitModel.StateUnit.Value == StateUnit.Fail) _controlModel.IsJump.Value = true;
            if (_unitModel.StateUnit.Value == StateUnit.StandUp) _controlModel.IsJump.Value = false;
        }
    }
}
