using System;
using UnityEngine;

public sealed class SubscriptionIntPrefs : IReadOnlySubscriptionField<int>
{
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            PlayerPrefs.SetInt(_name, _value);
            evtChange.Invoke(value);
        }
    }
    private int _value;
    private string _name;
    private event Action<int> evtChange = delegate { };

    public SubscriptionIntPrefs(string name)
    {
        _name = name;
        Value = PlayerPrefs.GetInt(_name, 0);
    }

    public void Subscribe(Action<int> func)
    {
        evtChange += func;
    }

    public void UnSubscribe(Action<int> func)
    {
        evtChange -= func;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
