using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Builder/BuilderEffectItemCfg", fileName = "BuilderEffectItemCfg")]
    public sealed class BuilderEffectItemCfg : BuilderCfgBasic
    {
        public DataUnitCfg DataUnitCfg => _dataUnitCfg;
        [SerializeField] private DataUnitCfg _dataUnitCfg;
    }
}
