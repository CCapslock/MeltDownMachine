using System.Collections.Generic;
using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Items/ItemsArray", fileName = "Items")]
    public class ItemsArray : ScriptableObject
    {
        [SerializeField] private List<ItemCfg> _itemCfg;
        public List<ItemCfg> ItemCfg => _itemCfg;

        public void AddItems()
        {
            if (_itemCfg == null) _itemCfg = new List<ItemCfg>();
            _itemCfg.Add(null);
        }
        public void DeleteItems(int i)
        {
            _itemCfg.RemoveAt(i);
        }
    }
}