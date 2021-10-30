namespace Mans
{
    internal sealed class MenuController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("MenuBuild");

        internal MenuController(SubscriptionField<GameState> gameState) : base()
        {
            AddController(new MenuStartController(gameState));
        }
    }
}
