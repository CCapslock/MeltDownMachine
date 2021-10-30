using UnityEngine;

namespace Mans
{
    internal sealed class ForsageController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("ForsageController");
        private EffectsModel _effectsModel;
        private IReadOnlySubscriptionField<Vector2> _control;

        internal ForsageController(IReadOnlySubscriptionField<Vector2> control, EffectsModel effectsModel)
        {
            _effectsModel = effectsModel;
            _control = control;
            _control.Subscribe(AddForsage);
        }

        protected override void OnDispose() =>
            _control.UnSubscribe(AddForsage);

        private void AddForsage(Vector2 controlValue)
        {
            if (controlValue.y > 0 && !_effectsModel.HaveEffect(0)) _effectsModel.AddItem(0);
        }
    }
}
