namespace Mans
{
    internal sealed class EmptyUnitController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("");
        private UnitModel _unit;
        private IUnitView _iUnitView;

        internal EmptyUnitController(UnitModel unit, IUnitView iUnitView)
        {
            _unit = unit;
            _iUnitView = iUnitView;
        }
    }
}
