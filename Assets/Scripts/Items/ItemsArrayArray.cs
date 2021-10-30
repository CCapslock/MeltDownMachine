using System.Collections.Generic;
using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Items/ItemsArrayArray", fileName = "ItemsArrayArray")]
    public class ItemsArrayArray : ScriptableObject
    {
        [SerializeField] private List<ItemsArrayParams> _itemsArray;
        public List<ItemsArrayParams> ItemsArray => _itemsArray;

        public void AddItems()
        {
            if (_itemsArray == null) _itemsArray = new List<ItemsArrayParams>();
            _itemsArray.Add(new ItemsArrayParams());
        }
        public void DeleteItems(int i)
        {
            _itemsArray.RemoveAt(i);
        }
    }
}