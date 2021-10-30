using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Builder/BuilderPortalCfg", fileName = "BuilderPortalCfg")]
    public sealed class BuilderPortalCfg : BuilderCfgBasic
    {
        public DataUnitCfg DataUnitCfg => _dataUnitCfg;
        [SerializeField] private DataUnitCfg _dataUnitCfg;
    }
}
