using System;
using UnityEngine;

public class AnimatorManView : MonoBehaviour
{
    public event Action evtStandUpComplete;
    public void StandUpComplete() =>
        evtStandUpComplete?.Invoke();

    public event Action evtFailComplete;
    public void FailComplete()
    {
        evtFailComplete?.Invoke();
    }

////    public Animator animator;

//    private void Start()
//    {
//        //animator = GetComponent<Animator>();
//        //if (animator==null) Debug.LogWarning($"Dont attach animator:{gameObject.name}");
//        //var clips=animator.runtimeAnimatorController.animationClips;
        
//        //foreach (var clip in clips)
//        //{
//        //    foreach (var evt in clip.events)
//        //    {
//        //    }
//        //}
//    }

}
