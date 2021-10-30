
using System.Collections.Generic;
using UnityEngine;

public class ControlLeak
{
    public static int Count { get; private set; }
    public static Dictionary<string, int> DataLeak { get; private set; } = new Dictionary<string, int>(); 

    private string _name;

    public ControlLeak(string name)
    {
        _name = name;
        Count++;
        DataLeak[name] = DataLeak.ContainsKey(name) ? DataLeak[name] + 1 : 1;
    }

    ~ControlLeak()
    {
        Count--;
        DataLeak[_name] -= 1;
    }
}
