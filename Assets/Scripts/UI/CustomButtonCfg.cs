using UnityEngine;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/ButtonUICfg", fileName = "ButtonUICfg")]
    public sealed class CustomButtonCfg : ScriptableObject
    {
        public ButtonAnimationData[] typeAnimationData;
    }
}