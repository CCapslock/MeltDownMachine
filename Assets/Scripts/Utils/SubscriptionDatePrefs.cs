using System;
using UnityEngine;

public sealed class SubscriptionDatePrefs : IReadOnlySubscriptionField<DateTime>
{
    public DateTime Value
    {
        get => _value;
        set
        {
            _value = value;
            PlayerPrefs.SetString(_namePrefs, _value.ToString());
            evtChange.Invoke(value);
        }
    }
    private DateTime _value;
    private string _namePrefs;
    private event Action<DateTime> evtChange = delegate { };

    public SubscriptionDatePrefs(string name)
    {
        _namePrefs = name;
        var result = PlayerPrefs.GetString(_namePrefs, "");
        if (!result.Equals("") && DateTime.TryParse(result, out DateTime dateTime))
        {
            Value = dateTime;
        }
        else Value = new DateTime(2000, 1, 1);
    }

    public void Subscribe(Action<DateTime> func)
    {
        evtChange += func;
    }

    public void UnSubscribe(Action<DateTime> func)
    {
        evtChange -= func;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
