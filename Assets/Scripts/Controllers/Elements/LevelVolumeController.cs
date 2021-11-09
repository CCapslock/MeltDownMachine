using UnityEngine;

namespace Mans
{
    internal sealed class LevelVolumeController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak(nameof(LevelVolumeController));
        private Collider _levelVolume;
        private GameObject _levelVolumeObject;
        private float _lastTimeVolumeChecked;
        
        private const float _minTimeBetweenVolumeChecks = 1.0f;
        private const float _maxDistance = 100.0f;

        private IUnitView _unitView;
        private UnitModel _unitModel;

        internal LevelVolumeController(IUnitView unitView, UnitModel unitModel)
        {
            _levelVolume = UnityEngine.Object.FindObjectOfType<TagLevelVolume>().GetComponent<Collider>();
            _levelVolumeObject = _levelVolume?.gameObject;
            _unitView = unitView;
            _unitModel = unitModel;
        }

        public void Execute(float deltaTime)
        {
            if((Time.time - _lastTimeVolumeChecked) > _minTimeBetweenVolumeChecks)
            {
                var ray = new Ray(_unitView.ObjectTransform.position, -_unitView.ObjectTransform.position);
                if(_levelVolume.Raycast(ray, out RaycastHit _, _maxDistance))
                {
                }
                _lastTimeVolumeChecked = Time.time;
            }
        }
    }
}
