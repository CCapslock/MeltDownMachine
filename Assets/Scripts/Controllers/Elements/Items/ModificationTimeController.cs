using System.Collections.Generic;

namespace Mans
{
    internal class ModificationTime
    {        
        public float value;
    }

    internal sealed class ModificationTimeController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak("ModificationTimeController");
        private IItemsModel<EffectsItemCfg> _itemsM;
        private Dictionary<int, ModificationTime> _timeEffectsItemCfg=new Dictionary<int, ModificationTime>();

        internal ModificationTimeController(IItemsModel<EffectsItemCfg> itemsM)
        {
            _itemsM = itemsM;
            _itemsM.EvtAddItem += AddItem;
            _itemsM.EvtRemoveItem += RemoveItem;
        }

        protected override void OnDispose()
        {
            _itemsM.EvtAddItem -= AddItem;
            _itemsM.EvtRemoveItem -= RemoveItem;
        }

        public void Execute(float deltaTime)
        {            
            List<int> forRemove=null;

            foreach (var item in _timeEffectsItemCfg)
            {
                _timeEffectsItemCfg[item.Key].value-=deltaTime;

                if (_timeEffectsItemCfg[item.Key].value <= 0)
                {
                    if (forRemove==null) forRemove = new List<int>();
                    forRemove.Add(item.Key);                    
                }
            }
            if (forRemove != null)
            {
                foreach (var item in forRemove)
                {
                    _itemsM.RemoveItem(item);
                }
            }
        }

        private void AddItem(EffectsItemCfg effectsItemCfg,bool _)
        {
            if (!_timeEffectsItemCfg.ContainsKey(effectsItemCfg.Id))
            {
                _timeEffectsItemCfg.Add(effectsItemCfg.Id, new ModificationTime { value = effectsItemCfg.Time });
            }
            else _timeEffectsItemCfg[effectsItemCfg.Id].value+= effectsItemCfg.Time;
        }

        private void RemoveItem(EffectsItemCfg effectsItemCfg)
        {
            if (_timeEffectsItemCfg.ContainsKey(effectsItemCfg.Id))
            {
                _timeEffectsItemCfg.Remove(effectsItemCfg.Id);                
            }
        }
    }
}
