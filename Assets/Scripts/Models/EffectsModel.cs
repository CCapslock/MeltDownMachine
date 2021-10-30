using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mans
{
    public sealed class EffectsModel : IItemsModel<EffectsItemCfg>
    {
        private ControlLeak _controlLeak = new ControlLeak("EffectsModel");
        public event Action<EffectsItemCfg, bool> EvtAddItem = delegate { };
        public event Action<EffectsItemCfg> EvtRemoveItem = delegate { };
        private IReadOnlyDictionary<int, ItemCfg> AllItems;
        private List<ItemCfg> _effects = new List<ItemCfg>();

        public EffectsModel(ItemsArray itemsForEffects) =>
            AllItems = Utils.DecompositeItems(itemsForEffects);

        public bool HaveEffect(int ID)
        {
            if (AllItems.TryGetValue(ID, out ItemCfg itemCfg))
            {
                var effectsItemCfg = itemCfg as EffectsItemCfg;
                if (_effects.Contains(effectsItemCfg))
                    return true;
            }
            return false;
        }

        public void AddItem(int ID)
        {
            if (AllItems.TryGetValue(ID, out ItemCfg itemCfg))
            {
                var effectsItemCfg = itemCfg as EffectsItemCfg;
                if (!_effects.Contains(effectsItemCfg))
                {
                    _effects.Add(effectsItemCfg);
                    EvtAddItem.Invoke(effectsItemCfg, false);
                }
                else EvtAddItem.Invoke(effectsItemCfg, true);
                Debug.Log($"Added the EffectItem: {effectsItemCfg.TitleName}");
            }
            else Debug.LogWarning($"Attempt to add  an unknown UpgradeItemID:{ID}");
        }

        public void RemoveItem(int ID)
        {
            if (AllItems.TryGetValue(ID, out ItemCfg itemCfg))
            {
                var effectsItemCfg = itemCfg as EffectsItemCfg;
                if (_effects.Contains(effectsItemCfg))
                {
                    _effects.Remove(effectsItemCfg);
                    EvtRemoveItem.Invoke(effectsItemCfg);
                    //Debug.Log($"Remove the EffectItem: {effectsItemCfg}");
                }
                else Debug.LogWarning($"Attempt to delete an unknown EffectItem:{effectsItemCfg}");
            }
            else Debug.LogWarning($"Attempt to remove an unknown EffectItemID:{ID}");
        }
    }
}