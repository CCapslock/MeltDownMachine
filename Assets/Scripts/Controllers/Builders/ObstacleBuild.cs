﻿using UnityEngine;

namespace Mans
{
    internal sealed class ObstacleBuild : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("ObstacleBuild");
        private const TypeUnit _typeItemStart = TypeUnit.Obstacles;
        private UnitModel _unitModel;

        internal ObstacleBuild()
        {
            _isParent = true;
        }

        internal override ControllerBasic CreateControllers()
        {
            var cfg = (BuilderObstacleCfg)_gameObjects[0].iUnitView.BuilderConfig;

            _unitModel = new UnitModel();
            var controlModel = new ControlModel();
            AddController(new UnitController(_unitModel, new SubscriptionField<int>(), _gameObjects[0].iInteractive, _typeItemStart, cfg.DataUnitCfg));
            AddController(new MoveController(controlModel.Control, controlModel.IsJump, _unitModel, _gameObjects[0].iUnitView, cfg.DataUnitCfg));

            AddController(new RotateExtendController(controlModel.Control, _gameObjects[0].iUnitView, cfg.DataUnitCfg));
            AddController(new MoveToPointController(controlModel.Control, controlModel.TargetPosition, controlModel.ReachedTarget, _gameObjects[0].iUnitView, cfg.DataUnitCfg));
            if (_gameObjects[0].gameObject.TryGetComponent(out UnitViewTraectory unitViewTraectory))
                AddController(new MoveTrackController(controlModel.TargetPosition, controlModel.ReachedTarget, unitViewTraectory));

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
