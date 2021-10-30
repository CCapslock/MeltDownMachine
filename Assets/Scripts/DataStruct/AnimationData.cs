using System;
using UnityEngine;

namespace Mans
{
    [Serializable]
    public sealed class AnimationData
    {
        public bool isLoop => _isLoop;
        [SerializeField] private bool _isLoop;

        public TypeAnimation typeAnimation => _typeAnimation;
        [SerializeField] private TypeAnimation _typeAnimation;

        public Sprite[] sprites => _sprites;
        [SerializeField] private Sprite[] _sprites;
    }
}
