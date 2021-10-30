using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.EditorTools;

[EditorTool("Perpendicular Move Tool")]
public class PerpendicularTool : EditorTool
{
    private GUIContent _toolBarIcon;

    public override GUIContent toolbarIcon => _toolBarIcon;

    public override void OnToolGUI(EditorWindow window)
    {
        GameObject targetGameObject = target as GameObject;

        if(targetGameObject == null)
            return;

        Quaternion handleRotation;
        if(Tools.pivotRotation == PivotRotation.Local)
            handleRotation = targetGameObject.transform.rotation;
        else
            handleRotation = Quaternion.identity;

        EditorGUI.BeginChangeCheck();
        Vector3 newPosition = Handles.PositionHandle(targetGameObject.transform.position, handleRotation);
        
        if(EditorGUI.EndChangeCheck())
        {
            if (PhysicsSceneExtensions.GetPhysicsScene(targetGameObject.scene).Raycast(newPosition + targetGameObject.transform.up*0.01f, -newPosition, out var hit))
            {
                Undo.RecordObject(targetGameObject.transform, "Move object");
                if(PrefabUtility.IsPartOfPrefabInstance(targetGameObject))
                    PrefabUtility.RecordPrefabInstancePropertyModifications(targetGameObject.transform);
                targetGameObject.transform.position = hit.point;
                var newForward = Quaternion.FromToRotation(targetGameObject.transform.up, hit.normal)*targetGameObject.transform.forward;
                targetGameObject.transform.rotation = Quaternion.LookRotation(newForward, hit.normal);
            }
        }
    }

    public void OnEnable()
    {
        _toolBarIcon = new GUIContent("PerpendicularMoveTool", "Perpendicular Move Tool");
    }
}
