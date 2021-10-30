using UnityEngine;

namespace Mans
{

    [CreateAssetMenu(menuName = "My/AnimationData",fileName ="UnitAnimation")]
    public sealed class AnimationCfg : ScriptableObject
    {
        public float Speed => _speed;
        [SerializeField] private float _speed=10;
        public bool IsMirror => _isMirror;
        [SerializeField] private bool _isMirror;
        public AnimationData[] animationData => _animationData;
        [SerializeField] private AnimationData[] _animationData;
    }
}
