using UnityEngine;

namespace Mans
{
    internal sealed class ThrowBackController : ControllerBasic
    {
        ControlLeak _controlLeak = new ControlLeak(nameof(ThrowBackController));
        IUnitView _unitView;
        IInteractive _interactive;
        IReadOnlySubscriptionField<bool> _isShielded;
        DataUnitCfg _unitData;

        internal ThrowBackController(IUnitView unitView, IInteractive interactive, IReadOnlySubscriptionField<bool> isShielded, DataUnitCfg unitData)
        {
            _unitView = unitView;
            _interactive = interactive;
            _isShielded = isShielded;
            _unitData = unitData;
            _interactive.evtCollided += ProcessCollision;
        }

        private void ProcessCollision(Vector3 direction)
        {
            if(_isShielded.Value)
                _unitView.ObjectRigidbody.AddForce(direction.normalized * (_unitData.ThrowBackForce * _unitData.ThrowBackForceShieldMultiplier), ForceMode.Impulse);
            else
                _unitView.ObjectRigidbody.AddForce(direction.normalized * _unitData.ThrowBackForce, ForceMode.Impulse);

        }

        protected override void OnDispose()
        {
            _interactive.evtCollided -= ProcessCollision;
        }
    }
}
