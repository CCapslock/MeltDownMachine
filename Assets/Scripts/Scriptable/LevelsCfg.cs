using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "My/Levels", fileName = "Levels")]
public class LevelsCfg : ScriptableObject
{
    public AssetReference[] LoadPrefab => _loadPrefab;
    [SerializeField] private AssetReference[] _loadPrefab;
}
