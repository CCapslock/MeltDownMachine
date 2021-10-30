using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/Builder/BuilderPlayerCfg", fileName = "BuilderPlayerCfg")]
    public sealed class BuilderPlayerCfg : BuilderCfgBasic
    {
        public BuilderManCfg BuilderManCfg => _builderManCfg;
        [SerializeField] private BuilderManCfg _builderManCfg;
        public ParalaxCfg ParalaxCfgCamera => _paralaxCfgCamera;
        [SerializeField] private ParalaxCfg _paralaxCfgCamera;
        public ParalaxCfg ParalaxCfgAudioListener => _paralaxCfgAudioListener;
        [SerializeField] private ParalaxCfg _paralaxCfgAudioListener;
    }
}
