using UnityEngine;

[CreateAssetMenu(menuName = "My/Homing",fileName = "Unit_Homing 0")]
public class HomingCfg : ScriptableObject
{
    public bool IsHomingCondition => _isHomingCondition;
    [SerializeField] private bool _isHomingCondition;

    public float MinDistanceHoming => _minDistanceHoming;
    [SerializeField] private float _minDistanceHoming;

    public float PowerMove => _powerMove;
    [SerializeField] private float _powerMove;
}
