using UnityEngine;

[CreateAssetMenu(menuName = "My/Paralax",fileName = "Paralax")]
public class ParalaxCfg : ScriptableObject
{
    public float AddPositionForward => _addPositionForward;
    [SerializeField] private float _addPositionForward=2;
    public float ElasticPower => _elasticPower;
    [SerializeField] private float _elasticPower=1;
    public float AddPositionRight => _addPositionRight;
    [SerializeField] private float _addPositionRight=5;

    public Vector3 SeeAngle => _seeAngle;
    [SerializeField] private Vector3 _seeAngle;
}
