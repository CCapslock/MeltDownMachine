
namespace Mans
{
    internal sealed class ManBuild : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("ManBuild");
        private const TypeUnit _typeItemStart = TypeUnit.Man;
        private UnitModel _unitModel;

        internal ManBuild()
        {
            _isParent = true;
        }


        internal override ControllerBasic CreateControllers()
        {
            _unitModel = new UnitModel();
            var cfg = (BuilderManCfg)_gameObjects[0].iUnitView.BuilderConfig;
            var controlModel = new ControlModel();
            var upLevelTransform = _gameObjects[0].gameObject.transform.parent;
            AddController(new UnitController(_unitModel, new SubscriptionField<int>(), _gameObjects[0].iInteractive, _typeItemStart, cfg.DataUnitCfg));
            AddController(new MoveController(controlModel.Control, controlModel.IsJump, _unitModel, _gameObjects[0].iUnitView, cfg.DataUnitCfg));
            AddController(new PuppetController(_unitModel, controlModel.IsJump, _gameObjects[0].iUnitView, _gameObjects[0].iInteractive, upLevelTransform, cfg.DataUnitCfg, cfg.PuppetCfg));
            var effectsItem = new EffectsModel(cfg.ItemsArray);
            AddController(new AddModificationController<EffectsItemCfg>(effectsItem, _unitModel, _gameObjects[0].iUnitView));
            AddController(new ModificationTimeController(effectsItem));
            //AddController(new ForsageController(controlModel.Control, effectsItem));

            AddController(new MoveRandomController(controlModel, _unitModel, cfg.DataUnitCfg));
            _unitModel.evtKill += Kill;
            AddController(new MoveToPointController(controlModel.Control, controlModel.TargetPosition, controlModel.ReachedTarget, _gameObjects[0].iUnitView, cfg.DataUnitCfg));
            if (_gameObjects[0].gameObject.TryGetComponent(out UnitViewTraectory unitViewTraectory))
                AddController(new MoveTrackController(controlModel.TargetPosition, controlModel.ReachedTarget, unitViewTraectory));
            else AddController(new FindManController(controlModel, _gameObjects[0].iUnitView, cfg.ManCfg));
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
            Clear();
            _unitModel.evtKill -= Kill;
        }
    }
}
