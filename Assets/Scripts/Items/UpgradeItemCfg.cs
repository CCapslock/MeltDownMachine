using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Items/UpgradeItem", fileName = "UpgradeItem")]
    public class UpgradeItemCfg : ItemCfg
    {
        [SerializeField] private ItemCfg _placeofUpgrade;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private TypeModification _typeModification;
        [SerializeField] private float _value;

        public ItemCfg PlaceOfUpgrade => _placeofUpgrade;
        public Sprite Sprite => _sprite;
        public TypeModification TypeUpgrade => _typeModification;
        public float Value => _value;
    }
}