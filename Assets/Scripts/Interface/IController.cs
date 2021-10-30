using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Mans
{
    public interface IController
    {
        event Action<IController> EvtNeedDestroy;
        event Action<AssetReference, Transform, Vector3, Quaternion, Action<GameObjectData>, TypeCreateForObject, float> EvtCreateGameObjectAddressable;
        event Action<AssetReference, Transform, Vector3, Quaternion, Action<GameObjectData>, TypeCreateForObject> EvtCreateGameObjectAndControllersAddressable;
        public ControllerBasic SetNumCfg(int numCfg);
    }

    public interface IControllerAddressable
    {
        public event Action EvtAddressableCompleted;
    }
}