using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/StateMachine", fileName = "StateMachineUnit")]
    public sealed class StateMachineCfg : ScriptableObject
    {
        public StateData[] StateData => _stateData;
        [SerializeField] private StateData[] _stateData;
    }
}
