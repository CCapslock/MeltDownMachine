using UnityEditor;
using UnityEngine;

namespace Mans
{
    [CustomEditor(typeof(ItemsArray))]
    public class EditorItemsArray : Editor
    {
        private ItemsArray _itemsArray;

        private void OnEnable()
        {
            _itemsArray = target as ItemsArray;
        }

        public override void OnInspectorGUI()
        {

            
            float win = Screen.width*0.9f;
            float win90 = win * 0.8f;
            float win10 = win * 0.1f;

            EditorGUILayout.BeginVertical("box");

            if (_itemsArray.ItemCfg != null)
            {
                for (int i = 0; i < _itemsArray.ItemCfg.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("X", GUILayout.Width(win), GUILayout.Width(win10)))
                    {
                        _itemsArray.DeleteItems(i);
                        EditorUtility.SetDirty(_itemsArray);
                        break;
                    }


                    _itemsArray.ItemCfg[i] = (ItemCfg)EditorGUILayout.ObjectField(_itemsArray.ItemCfg[i], typeof(ItemCfg), false, GUILayout.Width(win90));
                    if (_itemsArray.ItemCfg[i] != null)
                    {
                        EditorGUILayout.LabelField(_itemsArray.ItemCfg[i].Id.ToString(), GUILayout.Width(win10));
                    }
                    EditorGUILayout.EndHorizontal();

                }
            }
            if (GUILayout.Button("Add Item", GUILayout.Width(win)))
            {
                _itemsArray.AddItems();
                EditorUtility.SetDirty(_itemsArray);
            }
                EditorGUILayout.EndVertical();

            if (GUI.changed) EditorUtility.SetDirty(_itemsArray);
        }
    }
}