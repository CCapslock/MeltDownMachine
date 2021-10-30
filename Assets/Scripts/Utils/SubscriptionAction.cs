using System;

public sealed class SubscriptionAction : IReadOnlySubscriptionAction
{
    private event Action evtChange = delegate { };

    public void Subscribe(Action func)
    {
        evtChange += func;
    }

    public void UnSubscribe(Action func)
    {
        evtChange -= func;
    }

    public void Invoke()
    {
        evtChange.Invoke();
    }
}
