using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Builder/BuilderFoodCfg", fileName = "BuilderFoodCfg")]
    public sealed class BuilderFoodCfg : BuilderCfgBasic
    {
        public DataUnitCfg DataUnitCfg => _dataUnitCfg;
        [SerializeField] private DataUnitCfg _dataUnitCfg;
    }
}
