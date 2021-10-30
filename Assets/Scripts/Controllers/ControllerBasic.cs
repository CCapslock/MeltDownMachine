using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Mans
{

    public abstract class ControllerBasic : IDisposable, IController, IControllerAddressable
    {
        #region Fields

        public event Action EvtAddressableCompleted;
        public event Action<IController> EvtNeedDestroy;
        public event Action<AssetReference, Transform, Vector3, Quaternion, Action<GameObjectData>, TypeCreateForObject, float> EvtCreateGameObjectAddressable;
        public event Action<AssetReference, Transform, Vector3, Quaternion, Action<GameObjectData>, TypeCreateForObject> EvtCreateGameObjectAndControllersAddressable;

        public bool IsEndCreateAddressable => _isCreateAddressableComplete;

        protected List<GameObjectData> _gameObjects = new List<GameObjectData>();
        protected int _numCfg = 0;
        protected string _subTypeName;
        protected List<IController> _iControllers = new List<IController>();
        //protected List<string> _dataNames = new List<string>();
        protected Dictionary<string, (AsyncOperationHandle handle, ScriptableObject cfg)> _dataCfg = new Dictionary<string, (AsyncOperationHandle, ScriptableObject)>();
        protected bool _isCreateAddressableComplete;
        protected bool _isParent;

        private int _countCompleteAddressableObject;
        private List<AsyncOperationHandle<GameObject>> _addressableAll = new List<AsyncOperationHandle<GameObject>>();
        protected TypeUnit _typeItem;

        #endregion


        #region Util

        public ControllerBasic SetNumCfg(int numCfg)
        {
            foreach (var item in _iControllers)
            {
                item.SetNumCfg(numCfg);
            }
            _numCfg = numCfg;
            OnSetNumCfg(numCfg);
            return this;
        }

        protected virtual void OnSetNumCfg(int numCfg)
        {
        }

        protected void NeedDestroy()
        {
            EvtNeedDestroy.Invoke(this);
        }

        protected IController AddController(IController controller)
        {
            _iControllers.Add(controller);
            ListControllers.Add(controller);
            if (controller is IControllerAddressable controllerAddressable)
            {
                _countCompleteAddressableObject++;
                controllerAddressable.EvtAddressableCompleted += ControllerAddressableCompleted;
            }
            controller.EvtNeedDestroy += DestroyController;
            controller.EvtCreateGameObjectAddressable += CreateGameObjectAddressable;
            controller.EvtCreateGameObjectAndControllersAddressable += CreateGameObjectAndControllersAddressable;
            return controller;
        }

        protected void DestroyController(IController controller)
        {
            ListControllers.Delete(controller);
            _iControllers.Remove(controller);
            controller.EvtNeedDestroy -= DestroyController;
            controller.EvtCreateGameObjectAddressable -= CreateGameObjectAddressable;
            controller.EvtCreateGameObjectAndControllersAddressable -= CreateGameObjectAndControllersAddressable;

            if (controller is IDisposable disposeController)
                disposeController.Dispose();
        }

        private void ControllerAddressableCompleted()
        {
            if (--_countCompleteAddressableObject == 0)
            {
                EvtAddressableCompleted?.Invoke();
                EvtAddressableCompleted = null;
            }
        }

        private void GetInfoGameObject(GameObjectData data)
        {
            if (data.gameObject != null)
            {
                //if (data.gameObject.TryGetComponent(out IInteractive iInteractive))
                //{
                //    data.iInteractive = iInteractive;
                //}
                //if (data.gameObject.TryGetComponent(out IUnitView iUnitView))
                //{
                //    data.iUnitView = iUnitView;
                //    Reference.GameModel.AddUnitView(iUnitView);
                //    if (iUnitView.BuilderConfig == null) Debug.LogWarning($"Dont Set config:{iUnitView.ObjectTransform.name}");
                //}
                var component = data.gameObject.GetComponentsInChildren<MonoBehaviour>().OfType<IUnitView>().
                    Select(x => (x as MonoBehaviour).gameObject).FirstOrDefault();

                if (component != null)
                {
                    if (component.TryGetComponent(out IInteractive iInteractive))
                    {
                        data.iInteractive = iInteractive;
                    }
                    if (component.TryGetComponent(out IUnitView iUnitView))
                    {
                        data.iUnitView = iUnitView;
                        Reference.GameModel.AddUnitView(iUnitView);
                        if (iUnitView.BuilderConfig == null) Debug.LogWarning($"Dont Set config:{iUnitView.ObjectTransform.name}");
                    }
                }
            }
        }

        public GameObjectData this[int index]
        {
            get => _gameObjects[index];
        }

        public void Dispose()
        {
            OnDispose();
            Clear();
            EvtAddressableCompleted = null;
            //isDispose = true;
        }

        protected void Clear()
        {
            foreach (var item in _addressableAll)
            {
                if (!item.IsDone) Addressables.Release(item);
            }
            _addressableAll.Clear();

            for (int i = 0; i < _iControllers.Count; i++)
            {
                ListControllers.Delete(_iControllers[i]);
                if (_iControllers[i] is IDisposable disposeController)
                    disposeController.Dispose();
            }
            _iControllers.Clear();

            for (int i = 0; i < _gameObjects.Count; i++)
            {
                if (_gameObjects[i].gameObject != null)
                {
                    if (_gameObjects[i].iUnitView!=null) Reference.GameModel.RemoveUnitView(_gameObjects[i].iUnitView);
                    if (!_gameObjects[i].isAddressable) Object.Destroy(_gameObjects[i].gameObject);
                    else Addressables.ReleaseInstance(_gameObjects[i].gameObject);
                }
            }

            _gameObjects.Clear();

            foreach (var item in _dataCfg)
            {
                Addressables.Release(item.Value.handle);
            }
            _dataCfg.Clear();


        }

        protected void DestroyGameObject(GameObjectData gameObjectData)
        {
            if (gameObjectData.gameObject != null)
            {
                if (!gameObjectData.isAddressable) Object.Destroy(gameObjectData.gameObject);
                else Addressables.ReleaseInstance(gameObjectData.gameObject);
            }
            _gameObjects.Remove(gameObjectData);
        }

        protected virtual void OnDispose()
        {
        }

        #endregion


        #region Builds

        private GameObjectData CreateUnitBasic(string nameRes)
        {
            var data = new GameObjectData();
            nameRes = nameRes.Replace("##", $"{_numCfg}");
            data.prefabGameObject = LoadResources.GetValue<GameObject>(nameRes);
            return data;
        }

        protected GameObjectData CreateGameObject(Transform folder, string nameRes)
        {
            var data = CreateUnitBasic(nameRes);
            data.gameObject = GameObject.Instantiate(data.prefabGameObject, folder);
            GetInfoGameObject(data);
            _gameObjects.Add(data);
            return data;
        }

        protected void CreateGameObjectAddressable(string key, Transform folder, Action<GameObjectData> evt = null, float timeDestroy = 0)
        {
            key = key.Replace("##", $"{_numCfg}");
            var addressable = Addressables.InstantiateAsync(key, folder);
            SetCompleteAddressable(evt, addressable, timeDestroy);
        }

        protected void CreateGameObjectAddressable(string key, Transform folder, Vector3 position, Quaternion rotation, Action<GameObjectData> evt = null, float timeDestroy = 0)
        {
            key = key.Replace("##", $"{_numCfg}");
            var addressable = Addressables.InstantiateAsync(key, position, rotation, folder);
            SetCompleteAddressable(evt, addressable, timeDestroy);
        }

        protected void CreateGameObjectAddressable(AssetReference assetReference, Transform folder, Action<GameObjectData> evt = null, float timeDestroy = 0)
        {
            var addressable = Addressables.InstantiateAsync(assetReference, folder);
            SetCompleteAddressable(evt, addressable, timeDestroy);
        }

        protected void CreateGameObjectAndControllersAddressable(AssetReference assetReference, Transform folder, Vector3 position, Quaternion rotation, Action<GameObjectData> evt = null, TypeCreateForObject typeCreateForObject = TypeCreateForObject.None)
        {
            if (typeCreateForObject == TypeCreateForObject.UpPassParent ||
                (typeCreateForObject == TypeCreateForObject.notForParent && _isParent))
            {
                EvtCreateGameObjectAndControllersAddressable?.Invoke(assetReference, folder, position, rotation, evt, TypeCreateForObject.notForParent);
                return;
            }

            Addressables.InstantiateAsync(assetReference, position, rotation, folder).Completed +=
                (obj) =>
                {
                    if (Utils.TrySetUnitBuild(obj.Result, out ControllerBasic controllerBasic))
                        AddController(controllerBasic);
                };
        }

        protected void CreateGameObjectAddressable(AssetReference assetReference, Transform folder, Vector3 position, Quaternion rotation, Action<GameObjectData> evt = null, TypeCreateForObject typeCreateForObject = TypeCreateForObject.None, float timeDestroy = 0)
        {
            if (typeCreateForObject == TypeCreateForObject.UpPassParent ||
                (typeCreateForObject == TypeCreateForObject.notForParent && _isParent))
            {
                EvtCreateGameObjectAddressable?.Invoke(assetReference, folder, position, rotation, evt, TypeCreateForObject.notForParent,timeDestroy);
                return;
            }

            var addressable = Addressables.InstantiateAsync(assetReference, position, rotation, folder);
            SetCompleteAddressable(evt, addressable, timeDestroy);
        }

        private void SetCompleteAddressable(Action<GameObjectData> evt, AsyncOperationHandle<GameObject> addressable, float timeDestroy)
        {
            _addressableAll.Add(addressable);
            _countCompleteAddressableObject++;
            var gameObjectData = new GameObjectData();
            _gameObjects.Add(gameObjectData);

            addressable.Completed += (obj) =>
            {
                gameObjectData.isAddressable = true;
                gameObjectData.gameObject = obj.Result;
                GetInfoGameObject(gameObjectData);
                DecCountForCompeteAddressable();
                evt?.Invoke(gameObjectData);
                if (timeDestroy > 0) CoroutinesView.inst.AddCoroutine(DestoyingDelay(gameObjectData, timeDestroy));
            };
        }

        private IEnumerator DestoyingDelay(GameObjectData gameObjectData, float timeDestroy)
        {
            yield return new WaitForSeconds(timeDestroy);
            DestroyGameObject(gameObjectData);
        }

        protected void LoadCfg(string key, Action<ScriptableObject> evt = null)
        {
            var addreessable = Addressables.LoadAssetAsync<ScriptableObject>(key);
            _countCompleteAddressableObject++;

            addreessable.Completed += (obj) =>
            {
                if (!_dataCfg.ContainsKey(key))
                    _dataCfg.Add(key, (addreessable, obj.Result));

                DecCountForCompeteAddressable();
                evt?.Invoke(obj.Result);
            };
        }

        protected string GetSubTypeName(TypeUnit typeUnit, int subType)
        {
            return subType.ToString();
        }

        private void DecCountForCompeteAddressable()
        {
            _countCompleteAddressableObject--;
            if (_countCompleteAddressableObject == 0)
            {
                EvtAddressableCompleted?.Invoke();
                EvtAddressableCompleted = null;
                _isCreateAddressableComplete = true;
            }
        }

        public ControllerBasic SetGameObject(GameObject gameObject)
        {
            var data = new GameObjectData();
            data.gameObject = gameObject;
            GetInfoGameObject(data);
            _gameObjects.Add(data);
            return this;
        }

        internal virtual ControllerBasic CreateControllers()
        {
            return this;
        }

        #endregion

    }
}
