using UnityEngine;

namespace Mans
{

    [CreateAssetMenu(menuName = "My/PuppetCfg", fileName = "PuppetCfg")]
    public sealed class PuppetCfg : ScriptableObject
    {
        public float WeightFall => _weightFall;
        [SerializeField] private float _weightFall=0.2f;

        public float WeightNormal => _weightNormal;
        [SerializeField] private float _weightNormal=1;


        public float MappingWeghtFall => _mappingWeghtFall;
        [SerializeField] private float _mappingWeghtFall=1;

        public float MappingWeghtNormal => _mappingWeghtNormal;
        [SerializeField] private float _mappingWeghtNormal=0;

        public float PowerMoveToNormal => _powerMoveToNormal;
        [SerializeField] private float _powerMoveToNormal=0.1f;


    }
}
