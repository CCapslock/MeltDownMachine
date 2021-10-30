using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Mans
{
    [CreateAssetMenu(menuName = "My/DataUnit",fileName ="Unit_Data 0")]
    public sealed class DataUnitCfg : ScriptableObject
    {
        [Header("Move")]
        [SerializeField] private bool _isAllwaysMove = false;
        public bool IsAllwaysMove => _isAllwaysMove;

        [SerializeField] private float _powerMove = 1500f;
        public float PowerMove => _powerMove;

        [SerializeField] private float _powerRotate = 500f;
        public float PowerRotate => _powerRotate;


        [SerializeField] private float _powerJump = 300f;
        public float PowerJump => _powerJump;

        [SerializeField] private float _distGroundForJump = 1f;
        public float DistGroundForJump => _distGroundForJump;

        [Header("Extend Move")]
        [Range(0, 100f)]
        [SerializeField] private float _randomMove=0;
        public float RandomMove => _randomMove;
        public float PowerRandom => _powerRandom;
        [SerializeField] private float _powerRandom=50;

        public float PowerConstRotatation => _powerConstRotatation;
        [SerializeField] private float _powerConstRotatation;

        public float TimeConstRotation => _timeConstRotation;
        [SerializeField] private float _timeConstRotation=0f;


        [Header("Target Move")]
        [SerializeField] private float _minSqrDistanceToTarget = 1f;
        public float MinSqrDistanceToTarget => _minSqrDistanceToTarget;

        [Header("Bypass obstacles")]

        [SerializeField] private float _maxForwardObstaclesDistance = 1f;
        public float MaxObstaclesDistance => _maxForwardObstaclesDistance;

        [SerializeField] private float _obstaclesDetectionHalfWidth = 0.25f;
        public float ObstaclesDetectionHalfWidth => _obstaclesDetectionHalfWidth;

        [SerializeField] private float _obstaclesAngleStep = 0.2f;
        public float ObstaclesAngleStep => _obstaclesAngleStep;

        [SerializeField] private int _obstaclesCheckPeriod = 1;
        public int ObstaclesCheckPeriod => _obstaclesCheckPeriod;

        [Header("Limits")]
        [SerializeField] private float _maxSpeed = 2;
        public float MaxSpeed => _maxSpeed;

        [Header("Live")]
        [SerializeField] private int _maxLive=1;
        public int MaxLive => _maxLive;

        [SerializeField] private AssetReference _destroyEffects;
        public AssetReference DestroyEffects => _destroyEffects;

        [SerializeField] private float _timeViewDestroyEffects=10;
        public float TimeViewDestroyEffects => _timeViewDestroyEffects;

        [Header("Attack")]
        [SerializeField] private int _attackPower=1;
        public int AttackPower => _attackPower;

        [SerializeField] private int _addScores=0;
        public int AddScores => _addScores;

        
    }
}
