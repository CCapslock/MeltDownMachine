using System;
using UnityEngine;

public sealed class SubscriptionStringPrefs : IReadOnlySubscriptionField<string>
{
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            PlayerPrefs.SetString(_name, _value);
            evtChange.Invoke(value);
        }
    }
    private string _value;
    private string _name;
    private event Action<string> evtChange = delegate { };

    public SubscriptionStringPrefs(string name)
    {
        _name = name;
        Value = PlayerPrefs.GetString(_name,"");
    }

    public void Subscribe(Action<string> func)
    {
        evtChange += func;
    }

    public void UnSubscribe(Action<string> func)
    {
        evtChange -= func;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
