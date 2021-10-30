using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Builder/BuilderObstacleCfg", fileName = "BuilderObstacleCfg")]
    public sealed class BuilderObstacleCfg : BuilderCfgBasic
    {
        public DataUnitCfg DataUnitCfg => _dataUnitCfg;
        [SerializeField] private DataUnitCfg _dataUnitCfg;
    }
}
