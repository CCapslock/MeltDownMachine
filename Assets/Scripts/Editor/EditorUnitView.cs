using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Mans
{
    [CustomEditor(typeof(UnitView))]
    public class EditorUnitView : Editor
    {
        private IUnitView _unitView;
        private ControllerBasic controllerBasic;

        public static void Test()
        {
            //Debug.Log($"Count:{Addressables.ResourceLocators.Count()}");
        }


        private void OnEnable()
        {
            Test();
        }

        public override void OnInspectorGUI()
        {
            _unitView = target as IUnitView;
            var typeItem = _unitView.GetTypeItem();
            controllerBasic = Utils.ParseType(typeItem.type).SetNumCfg(typeItem.cfg);

            DrawDefaultInspector();
            if (_unitView != null)
            {
                float win = Screen.width;

                //var typeItem = _unitView.GetTypeItem();
                //var build = Utils.ParseType(typeItem.type).SetNumCfg(typeItem.cfg);
            }
        }
    }
}