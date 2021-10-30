using System.Collections;
using UnityEngine;

namespace Mans
{
    internal sealed class RotateExtendController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak("RotateExtendController");
        private SubscriptionField<Vector2> _control;
        private DataUnitCfg _dataUnitCfg;
        private IUnitView _unitView;
        private IEnumerator _coroutineRotate;
        private Vector3 _angles;
        private Vector3 _position;

        internal RotateExtendController(SubscriptionField<Vector2> control, IUnitView unitView, DataUnitCfg dataUnitCfg)
        {
            _control = control;
            _unitView = unitView;
            _dataUnitCfg = dataUnitCfg;
            _angles = _unitView.ObjectTransform.rotation.eulerAngles;
            _position = _unitView.ObjectTransform.localPosition;
            if (!_dataUnitCfg.PowerConstRotatation.Equals(0)) _coroutineRotate = CoroutinesView.inst.AddCoroutine(Rotate());
        }

        protected override void OnDispose()
        {
            if (_coroutineRotate != null) CoroutinesView.inst.RemoveCoroutine(_coroutineRotate);
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                _unitView.ObjectRigidbody.AddTorque(_dataUnitCfg.PowerConstRotatation * _unitView.ObjectRigidbody.mass * Vector3.up);
                yield return new WaitForSeconds(_dataUnitCfg.TimeConstRotation);
            }
        }

        public void Execute(float deltaTime)
        {
            _unitView.ObjectRigidbody.rotation = Quaternion.Euler(_angles.Change(y: _unitView.ObjectRigidbody.rotation.eulerAngles.y));
            _unitView.ObjectTransform.localPosition = _position;
        }
    }
}
