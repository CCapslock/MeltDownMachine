using System;
using UnityEngine.UI;

namespace Mans
{
    internal sealed class UINextSceneController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("UINextController");
        private const string _keyAddressable = "NextLevel.prefab";
        private GameModel _gameModel;
        private Button _button;

        internal UINextSceneController(GameModel gameModel)
        {
            _gameModel = gameModel;
            CreateGameObjectAddressable(_keyAddressable, Reference.Canvas, (obj) =>
            {
                _button = obj.gameObject.GetComponent<Button>();
                _button.onClick.AddListener(ClickButton);
            });
        }

        private void ClickButton()
        {
            _gameModel.GameState.Value = GameState.StartLevel;
        }
    }
}
