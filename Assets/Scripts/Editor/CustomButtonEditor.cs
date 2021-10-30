using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mans
{
    [CustomEditor(typeof(CustomButton))]
    public class CustomButtonEditor : ButtonEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var visualElement = new VisualElement();
            var fieldTypeAnimationData = new PropertyField(serializedObject.FindProperty(CustomButton.NameTypeAnimationData));
            visualElement.Add(fieldTypeAnimationData);
            visualElement.Add(new Label(" "));
            visualElement.Add(new IMGUIContainer(OnInspectorGUI));
            return visualElement;
        }
    }
}