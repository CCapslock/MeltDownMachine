using System;
using UnityEngine;

namespace Mans
{
    internal sealed class PickUpController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("");
        private EffectsModel _effectsModel;
        private IUnitView _iUnitView;
        private IInteractive _interactive;

        internal PickUpController(EffectsModel effectsModel, IInteractive interactive)
        {
            _effectsModel = effectsModel;
            _interactive = interactive;
            _interactive.evtAttack+=Attack;
        }

        protected override void OnDispose()
        {
            _interactive.evtAttack -= Attack;
        }

        private (int, bool) Attack(PackInteractiveData arg)
        {
            Debug.Log($"PuckUp");
            switch (arg.typeUnit)
            {
                case TypeUnit.EffectsItem:
                    _effectsModel.AddItem(arg.NumCfg);
                    break;
            }


            return (0, false);
        }
    }
}
