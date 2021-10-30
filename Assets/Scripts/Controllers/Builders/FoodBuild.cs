using UnityEngine;

namespace Mans
{
    internal sealed class FoodBuild : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("FoodBuild");
        private const string _nameUnit = "Food";
        private const TypeUnit _typeItemStart = TypeUnit.Food;
        private UnitModel _unitModel;

        internal FoodBuild()
        {
            _isParent = true;
        }

        internal override ControllerBasic CreateControllers()
        {
            var cfg = (BuilderFoodCfg)_gameObjects[0].iUnitView.BuilderConfig;
            //Reference.GameModel.AddUnitView(_gameObjects[0].iUnitView);

            _unitModel = new UnitModel();
            var controlModel = new ControlModel();
            AddController(new UnitController(_unitModel, new SubscriptionField<int>(), _gameObjects[0].iInteractive, _typeItem, cfg.DataUnitCfg));
            AddController(new MoveController(controlModel.Control, controlModel.IsJump, _unitModel, _gameObjects[0].iUnitView, cfg.DataUnitCfg));
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
            ClearUnit();
        }

        private void Kill()
        {
            ClearUnit(); 
            Clear();            
        }

        private void ClearUnit()
        {
            //if (_gameObjects.Count>0) Reference.GameModel.RemoveUnitView(_gameObjects[0].iUnitView);
            _unitModel.evtKill -= Kill;
        }

    }
}
