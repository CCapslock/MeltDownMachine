using System;
using UnityEngine;

public class OnTriggerView : MonoBehaviour
{
    private int _countTrigger = 0;
    public event Action<Collider, bool> evtUpdate = delegate { };
    
    private void OnTriggerEnter(Collider other)
    {
        _countTrigger++;
        if (_countTrigger == 1)
        {
            evtUpdate.Invoke(other, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _countTrigger--;
        if (_countTrigger == 0)
        {
            evtUpdate.Invoke(other, false);
        }
    }
}
