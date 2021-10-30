using UnityEngine;

namespace Mans
{
    public class MoveToPointController : ControllerBasic, IExecute
    {
        const float cosMaxAngleConsideredAsRotated = 0.1f;

        private ControlLeak _controlLeak = new ControlLeak("MoveToPointController");
        private SubscriptionField<Vector2> _control;
        private IReadOnlySubscriptionField<Transform> _targetPosition;
        private SubscriptionField<bool> _isReachedTarget;
        private IUnitView _unitView;
        private DataUnitCfg _dataUnitCfg;

        public MoveToPointController(SubscriptionField<Vector2> control,
            IReadOnlySubscriptionField<Transform> targetPosition, SubscriptionField<bool> isReachedTarget, IUnitView unitView, DataUnitCfg dataUnitCfg)
        {
            _control = control;
            _targetPosition = targetPosition;
            _isReachedTarget = isReachedTarget;
            _unitView = unitView;
            _dataUnitCfg = dataUnitCfg;
            _targetPosition.Subscribe(TargetChanged);
        }


        protected override void OnDispose() =>
            _targetPosition.UnSubscribe(TargetChanged);


        public void Execute(float deltaTime)
        {
            if (_isReachedTarget.Value) return;

            var objectTransform = _unitView.ObjectTransform;
            var directionToTarget = _targetPosition.Value.position - objectTransform.position;

            if (directionToTarget.sqrMagnitude > _dataUnitCfg.MinSqrDistanceToTarget)
            {
                var directionToMove = directionToTarget.normalized;

                Debug.DrawRay(objectTransform.position, directionToTarget, Color.white);

                var rotate = Vector3.Dot(directionToMove, objectTransform.right);
                var behind = Vector3.Dot(directionToMove, objectTransform.forward);
                if (behind < 0) rotate = Mathf.Sign(rotate == 0 ? 1 : rotate);
                if (Mathf.Abs(rotate) < cosMaxAngleConsideredAsRotated) rotate = 0;

                _control.Value = new Vector2(rotate, 0);
            }
            else
                _isReachedTarget.Value = true;
        }

        private void TargetChanged(Transform newTarget) =>
            _isReachedTarget.Value = false;
    }
}
