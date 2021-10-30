using UnityEngine;

namespace Mans
{
    public interface IUnitView
    {
        (TypeUnit type, int cfg) GetTypeItem();
        BuilderCfgBasic BuilderConfig { get; }
        void SetTypeItem(TypeUnit type = TypeUnit.None, int cfg = -1);

        Transform ObjectTransform { get; }
        Rigidbody ObjectRigidbody { get; }
    }
}