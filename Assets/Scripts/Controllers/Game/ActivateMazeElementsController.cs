using System.Linq;
using UnityEngine;

namespace Mans
{
    internal sealed class ActivateMazeElementsController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("ActivateMazeElementsController");

        internal ActivateMazeElementsController(Transform findFolder,GameModel gameModel ,UnitModel unitMPlayer) 
        {
            
            var objects=findFolder.GetComponentsInChildren<MonoBehaviour>().OfType<IUnitView>();

            foreach (var item in objects)
            {
                if (Utils.TrySetUnitBuild(item.ObjectTransform.gameObject,out ControllerBasic controller, gameModel, unitMPlayer))
                    AddController(controller);   
            }
        }
    }
}
