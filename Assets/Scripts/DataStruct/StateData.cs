using System;
using UnityEngine;

namespace Mans
{
    [Serializable]
    public sealed class StateData
    {
        public TypeAnimation CurrentAnimation => _currentAnimation;
        [SerializeField] private TypeAnimation _currentAnimation;
        public Commands command => _command;
        [SerializeField] private Commands _command;
        public TypeAnimation TargetAnimation => _targetAnimation;
        [SerializeField] private TypeAnimation _targetAnimation;
    }
}
