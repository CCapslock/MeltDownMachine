
namespace Mans
{
    internal sealed class PortalBuild : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("PortalBuild");
        private const string _nameUnit = "Portal";
        private const TypeUnit _typeItemStart = TypeUnit.Portal;
        private UnitModel _unitModel;

        internal PortalBuild()
        {
            _isParent = true;
        }

        internal override ControllerBasic CreateControllers()
        {
            var cfg = (BuilderPortalCfg)_gameObjects[0].iUnitView.BuilderConfig;

            _unitModel = new UnitModel();
            var controlModel = new ControlModel();
            AddController(new UnitController(_unitModel, new SubscriptionField<int>(), _gameObjects[0].iInteractive, _typeItemStart, cfg.DataUnitCfg));
            AddController(new MoveController(controlModel.Control, controlModel.IsJump, _unitModel, _gameObjects[0].iUnitView, cfg.DataUnitCfg));

            if (_gameObjects[0].gameObject.TryGetComponent(out ViewListGameObject viewListGameObject))
                AddController(new PortalController(_gameObjects[0].iInteractive, _gameObjects[0].iUnitView, viewListGameObject));
            AddController(new MoveRandomController(controlModel, _unitModel, cfg.DataUnitCfg));
            AddController(new MoveToPointController(controlModel.Control, controlModel.TargetPosition, controlModel.ReachedTarget, _gameObjects[0].iUnitView, cfg.DataUnitCfg));
            if (_gameObjects[0].gameObject.TryGetComponent(out UnitViewTraectory unitViewTraectory))
                AddController(new MoveTrackController(controlModel.TargetPosition, controlModel.ReachedTarget, unitViewTraectory));
            AddController(new BypassingObstaclesController(controlModel.Control, Reference.GameModel.ObstacleLayersBits.Value, _gameObjects[0].iUnitView, cfg.DataUnitCfg));

            _unitModel.evtKill += Kill;

            return this;
        }

        protected override void OnDispose()
        {
            _unitModel.evtKill -= Kill;
        }


        private void Kill()
        {
            _unitModel.evtKill -= Kill;
            Clear();
        }
    }
}
