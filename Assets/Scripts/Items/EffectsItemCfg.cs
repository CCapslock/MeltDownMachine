using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Items/EffectsItem", fileName = "EffectsItem")]
    public class EffectsItemCfg : ItemCfg
    {
        public Sprite Sprite => _sprite;
        [SerializeField] private Sprite _sprite;
        public TypeModification TypeUpgrade => _typeModification;
        [SerializeField] private TypeModification _typeModification;
        public float Value => _value;
        [SerializeField] private float _value;
        public float Time => _time;
        [SerializeField] private float _time;
    }

}