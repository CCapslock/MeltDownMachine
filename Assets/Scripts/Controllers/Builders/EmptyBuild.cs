namespace Mans
{
    internal sealed class EmptyBuild : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("");
        private const string _nameRes = "../Empty";
        private const string _nameData = "../Empty";
        private const TypeUnit _typeItemStart = TypeUnit.None;
        private UnitModel _unitModel;


        internal EmptyBuild()
        {
            _isParent = true;
        }

        internal override ControllerBasic CreateControllers()
        {
            var cfg = (BuilderManCfg)_gameObjects[0].iUnitView.BuilderConfig;
            _unitModel = new UnitModel();
            _unitModel.evtKill += Kill;
            return this;
        }

        protected override void OnDispose()
        {
            _unitModel.evtKill -= Kill;
        }

        private void Kill()
        {
            Clear();
            _unitModel.evtKill -= Kill;
        }
    }
}
