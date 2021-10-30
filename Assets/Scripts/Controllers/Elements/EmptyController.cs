namespace Mans
{
    internal sealed class EmptyController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("");

        internal EmptyController()
        {
        }
    }
}
