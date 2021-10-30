using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Mans
{
    public static partial class Utils
    {
        public static ControllerBasic ParseType(TypeUnit typeItem)
        {
            return typeItem switch
            {
                TypeUnit.Player => new PlayerBuild(),
                TypeUnit.Man => new ManBuild(),
                TypeUnit.Food => new FoodBuild(),
                TypeUnit.Portal => new PortalBuild(),
                TypeUnit.Obstacles => new ObstacleBuild(),
                TypeUnit.None => new EmptyBuild(),
                _ => new EmptyBuild()
            };
        }

        public static bool TrySetUnitBuild(GameObject gameObject, out ControllerBasic controller, GameModel gameModel=default, UnitModel unitMPlayer=default)
        {
            controller = null;

            var unitView = gameObject.GetComponentsInChildren<MonoBehaviour>().OfType<IUnitView>().First();
            if (unitView == null) return false;

            var type = unitView.GetTypeItem();
            if (!ActualType(type.type)) return false;

            controller = ParseType(type.type);
            controller.SetNumCfg(type.cfg).SetGameObject(unitView.ObjectTransform.gameObject);
            if (controller is PlayerBuild playerBuild) playerBuild.AddExtParams(gameModel, unitMPlayer);
            controller.CreateControllers();
            return true;
        }

        private static bool ActualType(TypeUnit type)
        {
            //if (type == TypeUnit.Player) return false;
            return true;
        }

        public static bool HasAddressableKey(string keyIn)
        {
            foreach (var item in Addressables.ResourceLocators)
            {
                foreach (var key in item.Keys)
                {
                    if (key.ToString() == keyIn) return true;
                }
            }
            return false;
        }

        public static Dictionary<int, ItemCfg> DecompositeItems(ItemsArray itemsArray)
        {
            var itemsArrayOut = new Dictionary<int, ItemCfg>();
            for (int i = 0; i < itemsArray.ItemCfg.Count; i++)
            {
                ItemCfg item = itemsArray.ItemCfg[i];
                if (!itemsArrayOut.ContainsKey(item.Id))
                {
                    itemsArrayOut.Add(item.Id, item);
                }
                else Debug.LogWarning($"Double Id of elements {item}:{itemsArrayOut[item.Id]}");
            }
            return itemsArrayOut;
        }
    }
}