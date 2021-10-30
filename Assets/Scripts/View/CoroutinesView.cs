using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutinesView : MonoBehaviour
{
    public static CoroutinesView inst;

    private void Awake()
    {
        if (inst == null) inst = this;
    }

    public IEnumerator AddCoroutine(IEnumerator metod)
    {
        StartCoroutine(metod);
        return metod;
    }
    public void RemoveCoroutine(IEnumerator metod)
    {
        if (metod == null) return;
        StopCoroutine(metod);
    }
}
