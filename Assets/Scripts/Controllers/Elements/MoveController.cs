using System;
using UnityEngine;

namespace Mans
{
    internal sealed class MoveController : ControllerBasic, IExecute
    {
        private const float _constScaleGravity = 100f;
        private const float _constUnitStair = 1f;

        private ControlLeak _controlLeak = new ControlLeak("MoveController");
        private IUnitView _unitView;
        private DataUnitCfg _unitData;
        private IReadOnlySubscriptionField<Vector2> _control;

        private Vector3 _normalGround;
        private IReadOnlySubscriptionField<bool> _isJump;
        private UnitModel _unitModel;
        private Vector3 _direct;

        internal MoveController(IReadOnlySubscriptionField<Vector2> control, IReadOnlySubscriptionField<bool> isJump,
            UnitModel unitModel, IUnitView unitView, DataUnitCfg unitData)
        {
            _control = control;
            _isJump = isJump;
            _unitView = unitView;
            _unitData = unitData;
            _unitModel = unitModel;

            _direct = _unitView.ObjectTransform.forward;
        }

        public void Execute(float deltaTime)
        {
            if (_unitModel.StateUnit.Value != StateUnit.Normal) return;
            Move(deltaTime);
            SetLimits(deltaTime);
        }

        private void SetLimits(float deltaTime)
        {
            var velocity = _unitView.ObjectRigidbody.velocity.Change(y:0);            
            velocity= Vector3.ClampMagnitude(velocity, _unitModel.MaxSpeed.Value * _unitModel.CoefficientDrift.Value);
            velocity.y = _unitView.ObjectRigidbody.velocity.y;
            _unitView.ObjectRigidbody.velocity = velocity;
        }

        private void Move(float deltaTime)
        {
            var force = deltaTime * _unitData.PowerMove * _unitView.ObjectTransform.forward * _unitView.ObjectRigidbody.mass *
                    (_unitData.IsAllwaysMove ? 1f : _control.Value.y) * _unitModel.CoefficientDrift.Value;
            _unitView.ObjectRigidbody.AddForce(force);

            if (!_control.Value.x.Equals(0))
            {
                
                //var torgue = new Vector3(0, _control.Value.x * _unitData.PowerRotate * deltaTime,0);
                _unitView.ObjectTransform.Rotate(Vector3.up, _control.Value.x * _unitData.PowerRotate * deltaTime);
                //_unitView.ObjectRigidbody.AddRelativeTorque(torgue);
            }

            if (_isJump.Value)
            {
                _unitView.ObjectRigidbody.AddForce(_unitView.ObjectTransform.up *
                                _unitData.PowerJump * _unitView.ObjectRigidbody.mass);
            }
        }
       
    }
}