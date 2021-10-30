
namespace Mans
{
    internal sealed class GlobalGameController : ControllerBasic
    {
        private GameModel _gameModel;
        private UnitModel _playerModel;
        private string _nameGameCfg = "GameCfg";
        private GameCfg _gameCfg;

        public GlobalGameController()
        {
            _playerModel = new UnitModel();
            _gameModel = new GameModel();
            Reference.AddGameModel(_gameModel);
            LoadCfg(_nameGameCfg, (obj) =>
            {
                _gameCfg = (GameCfg)obj;
                _gameModel.GroundLayersBits.Value= Utils.ParseLayers(_gameCfg.GroundLayers);
                _gameModel.ObstacleLayersBits.Value= Utils.ParseLayers(_gameCfg.ObstaclesLayers);
                AddController(new GameController(_gameModel, _playerModel));
            }
            );
        }
    }
}
