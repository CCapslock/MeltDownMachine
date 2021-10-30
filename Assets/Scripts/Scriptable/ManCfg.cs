using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "My/ManCfg", fileName = "ManCfg")]
public class ManCfg : ScriptableObject
{
    public float MaxLenghthFindMan => _maxLenghthFindMan;
    [SerializeField] private float _maxLenghthFindMan=30f;

    public float MinLenghthAttack => _minLenghthAttack;
    [SerializeField] private float _minLenghthAttack=10f;

    public float ChanceFindMan => _chanceFindMan;
    [SerializeField] private float _chanceFindMan=0.1f;
}
