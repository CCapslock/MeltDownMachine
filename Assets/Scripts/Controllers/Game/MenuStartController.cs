using UnityEngine;
using UnityEngine.UI;

namespace Mans
{
    internal sealed class MenuStartController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("MenuController");
        private SubscriptionField<GameState> _gameState;
        private GameObject _menu;
        private Button _button;
        private const string _nameRes = "Menu.prefab";

        internal MenuStartController(SubscriptionField<GameState> gameState)
        {
            CreateGameObjectAddressable (_nameRes, Reference.Canvas,EndCreate);
            _gameState = gameState;

        }

        private void EndCreate(GameObjectData obj)
        {
            _menu = obj.gameObject;
            var goStartGame = _menu.transform.GetComponentInChildren<TagButtonStartGame>();
            if (goStartGame.TryGetComponent(out Button button))
            {
                _button = button;
                _button.onClick.AddListener(StartGame);
            }
        }

        private void StartGame()
        {
            _gameState.Value = GameState.StartLevel;
            Object.Destroy(_menu);
        }

        protected override void OnDispose()
        {
            _button?.onClick.RemoveAllListeners();
        }
    }
}