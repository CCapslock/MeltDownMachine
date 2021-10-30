using UnityEngine;

namespace Mans
{
    public sealed class ControlModel
    {
        private ControlLeak _controlLeak = new ControlLeak("ControlModel");

        public ControlModel()
        {
            PositionCursor = new SubscriptionField<Vector2>();
            IsNewTouch = new SubscriptionField<bool>();
            Control = new SubscriptionField<Vector2>();
            IsJump = new SubscriptionField<bool>();
            TargetPosition = new SubscriptionField<Transform>();
            ReachedTarget = new SubscriptionField<bool> { Value = true };
        }

        public SubscriptionField<Vector2> PositionCursor { get; }
        public SubscriptionField<bool> IsNewTouch { get; }
        public SubscriptionField<Vector2> Control { get; }
        public SubscriptionField<bool> IsJump { get; }
        public SubscriptionField<Transform> TargetPosition { get; }
        public SubscriptionField<bool> ReachedTarget { get; }
    }
}