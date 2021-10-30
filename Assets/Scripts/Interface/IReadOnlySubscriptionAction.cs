using System;

public interface IReadOnlySubscriptionAction
{
    void Subscribe(Action func);
    void UnSubscribe(Action func);
}
