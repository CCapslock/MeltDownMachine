using UnityEngine;

namespace Mans
{
    internal sealed class PlayerBuild : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("PlayerBuild");
        private const string _targetRes = "Man 1.prefab";
        private const TypeUnit _typeItemStart = TypeUnit.Player;
        private SubscriptionField<GameState> _gameState;
        private GameModel _gameModel;
        private UnitModel _unitMPlayer;

        internal PlayerBuild()
        {
            _isParent = true;
        }

        internal ControllerBasic AddExtParams(GameModel gameModel, UnitModel unitMPlayer)
        {
            _unitMPlayer = unitMPlayer;
            _gameModel = gameModel;
            _gameState = gameModel.GameState;
            return this;
        }

        internal ControllerBasic Create()
        {
            var startPos = GameObject.FindObjectOfType<TagStartPosition>().transform;
            CreateGameObjectAddressable(_targetRes, Reference.ActiveElements, startPos.position, startPos.rotation);
            EvtAddressableCompleted += ()=> CreateControllers(); 
            return this;
        }

        internal override ControllerBasic CreateControllers()
        {
            var cfg = (BuilderPlayerCfg)_gameObjects[0].iUnitView.BuilderConfig;
            var controlModel = new ControlModel();
            var upLevelTransform = _gameObjects[0].gameObject.transform.parent;

            AddController(new UnitController(_unitMPlayer, _gameModel.Scores, _gameObjects[0].iInteractive, _typeItem, cfg.BuilderManCfg.DataUnitCfg));
            AddController(new AllInputsController(controlModel));

            var targetForCamera = upLevelTransform.GetComponentInChildren<TagHead>().transform;
            AddController(new ParalaxController(Reference.MainCamera.transform, targetForCamera, cfg.ParalaxCfgCamera));

            Reference.AudioListener.transform.position = _gameObjects[0].iUnitView.ObjectTransform.position;
            Reference.AudioListener.transform.rotation = _gameObjects[0].iUnitView.ObjectTransform.transform.rotation;
            AddController(new ParalaxController(Reference.AudioListener.transform, _gameObjects[0].iUnitView.ObjectTransform.transform, cfg.ParalaxCfgAudioListener));

            AddController(new MoveController(controlModel.Control, controlModel.IsJump, _unitMPlayer, _gameObjects[0].iUnitView, cfg.BuilderManCfg.DataUnitCfg));
            AddController(new PuppetController(_unitMPlayer, controlModel.IsJump, _gameObjects[0].iUnitView, _gameObjects[0].iInteractive, upLevelTransform, cfg.BuilderManCfg.DataUnitCfg, cfg.BuilderManCfg.PuppetCfg));
            var effectsItem = new EffectsModel(cfg.BuilderManCfg.ItemsArray);
            AddController(new AddModificationController<EffectsItemCfg>(effectsItem, _unitMPlayer, _gameObjects[0].iUnitView));
            AddController(new ModificationTimeController(effectsItem));
            AddController(new ForsageController(controlModel.Control, effectsItem));

            _unitMPlayer.evtKill += Kill;

            return this;
        }

        protected override void OnDispose()
        {
            _unitMPlayer.evtKill -= Kill;
        }

        private void Kill()
        {
            Clear();
            _unitMPlayer.evtKill -= Kill;
            _gameState.Value = GameState.GameOver;
        }
    }
}
