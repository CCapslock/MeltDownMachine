using UnityEngine;

[CreateAssetMenu(menuName = "My/GameCfg", fileName = "GameCfg")]
public class GameCfg : ScriptableObject
{
    public string[] GroundLayers => _groundLayers;
    [SerializeField] private string[] _groundLayers;
    public string[] ObstaclesLayers => _obstaclesLayers;
    [SerializeField] private string[] _obstaclesLayers;
}
