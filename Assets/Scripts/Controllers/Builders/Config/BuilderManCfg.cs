using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Builder/BuilderManCfg", fileName = "BuilderManCfg")]
    public sealed class BuilderManCfg : BuilderCfgBasic
    {
        public DataUnitCfg DataUnitCfg => _dataUnitCfg;
        [SerializeField] private DataUnitCfg _dataUnitCfg;
        public ManCfg ManCfg => _manCfg;
        [SerializeField] private ManCfg _manCfg;
        public PuppetCfg PuppetCfg => _puppetCfg;
        [SerializeField] private PuppetCfg _puppetCfg;
        public ItemsArray ItemsArray => _itemsArray;
        [SerializeField] private ItemsArray _itemsArray;

    }
}
