using UnityEditor;
using UnityEngine;

namespace SpritePlatformer
{
    [CustomPropertyDrawer(typeof(Traectory),true)]
    public class EditorTraectoryDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float width;
            Rect posCurrent;
            
            posCurrent = position;

            width = position.width *0.6f;
            posCurrent.width = width;
            EditorGUI.PropertyField(posCurrent, property.FindPropertyRelative("transform"), GUIContent.none);
            posCurrent.x += width;

            width = position.width *0.2f;
            posCurrent.width = width;            
            EditorGUI.PropertyField(posCurrent, property.FindPropertyRelative("powerMove"), GUIContent.none);
            posCurrent.x += width;

            width = position.width * 0.2f;
            posCurrent.width = width;
            EditorGUI.PropertyField(posCurrent, property.FindPropertyRelative("stopTime"), GUIContent.none);
            posCurrent.x += width;
        }
    }
}