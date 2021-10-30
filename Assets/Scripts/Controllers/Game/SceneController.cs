using System;
using UnityEngine;

namespace Mans
{
    internal sealed class SceneController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("SceneController");
        private string _keyAddressable;
        private GameModel _gameModel;
        private UnitModel _playerModel;

        internal SceneController(GameModel gameModel, UnitModel playerModel, string keyAddressable)
        {
            _numCfg = gameModel.CurrentLevel.Value;
            _gameModel = gameModel;
            _keyAddressable = keyAddressable;
            _playerModel = playerModel;
            _ = Reference.AudioListener;

            CreateGameObjectAddressable(_keyAddressable, Reference.ActiveElements, EndCreateLevel);
            AddController(new UINextSceneController(_gameModel));
        }

        private void EndCreateLevel(GameObjectData obj)
        {
            //ControllerBasic playerBuild;
            //AddController(playerBuild=new PlayerBuild(_gameModel, _playerModel).Create());
            //playerBuild.EvtAddressableCompleted += () =>
            //  {
                  AddController(new ActivateMazeElementsController(obj.gameObject.transform, _gameModel, _playerModel));
              //};
        }
    }
}
