using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mans
{
    public interface IInteractive
    {
        (int scores, bool isDead) Attack(PackInteractiveData data);
        event Action<bool> evtAnyCollision;
        event Action<IInteractive, bool> evtInteractive;
        event Func<PackInteractiveData, (int, bool)> evtAttack;
        event Action<Collider, bool> evtTrigger;
        void Kill();
    }
}