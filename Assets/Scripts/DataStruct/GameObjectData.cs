using UnityEngine;

namespace Mans
{
    public sealed class GameObjectData
    {
        public GameObject gameObject;
        public GameObject prefabGameObject;
        public IInteractive iInteractive;
        public IUnitView iUnitView;
        public bool isAddressable;
    }
}
