using System;
using UnityEngine;

public sealed class SubscriptionFloatPrefs : IReadOnlySubscriptionField<float>
{
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            PlayerPrefs.SetFloat(_name, _value);
            evtChange.Invoke(value);
        }
    }
    private float _value;
    private string _name;
    private event Action<float> evtChange = delegate { };

    public SubscriptionFloatPrefs(string name)
    {
        _name = name;
        Value = PlayerPrefs.GetFloat(_name, 0);
    }

    public void Subscribe(Action<float> func)
    {
        evtChange += func;
    }

    public void UnSubscribe(Action<float> func)
    {
        evtChange -= func;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
