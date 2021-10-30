using System;
using UnityEngine;

namespace Mans
{
    internal sealed class GameController : ControllerBasic
    {
        private GameModel _gameModel;
        private UnitModel _playerModel;
        private Vector3 _startCameraPosition;
        private Quaternion _startCameraRotation;
        private ControlLeak _controlLeak = new ControlLeak("GameController");
        private const string _keyLevels = "Level ##.prefab";

        internal GameController(GameModel gameModel, UnitModel playerModel)
        {
            _gameModel = gameModel;
            _playerModel = playerModel;
            _gameModel.GameState.Subscribe(ChangeStateGame);
            _startCameraPosition = Reference.MainCamera.transform.position; 
            _startCameraRotation = Reference.MainCamera.transform.rotation; 

            _gameModel.GameState.Value = GameState.StartLevel;                      
        }

        protected override void OnDispose()
        {
            _gameModel.GameState.UnSubscribe(ChangeStateGame);
        }

        private void ChangeStateGame(GameState gameState)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            switch (gameState)
            {
                case GameState.Menu:
                    Menu();
                    break;
                case GameState.StartLevel:
                    StartGame();
                    break;
                case GameState.GameOver:
                    break;
                case GameState.WinGame:
                    break;
                default:
                    break;
            }
        }


        private void Menu()
        {
            Clear();
            AddController(new MenuController(_gameModel.GameState));
        }

        private void StartGame()
        {            
            _gameModel.CurrentLevel.Value++;
            _playerModel = new UnitModel();

            var currentKeyLevel = _keyLevels.Replace("##", $"{_gameModel.CurrentLevel.Value}");
            if (!Utils.HasAddressableKey(currentKeyLevel)) _gameModel.CurrentLevel.Value = 1;

            Clear();
            Reference.MainCamera.transform.position = _startCameraPosition;
            Reference.MainCamera.transform.rotation = _startCameraRotation;

            AddController(new SceneController(_gameModel,_playerModel,_keyLevels));
        }

        
    }
}
