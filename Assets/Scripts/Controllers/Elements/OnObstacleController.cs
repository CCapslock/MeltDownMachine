using UnityEngine;

namespace Mans
{
    
    internal sealed class OnObstacleController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("OnObstacleController");
        private UnitModel _unit;
        private Transform _transform;
        private int count;
        private SubscriptionField<bool> _isObstacle;
        private OnTriggerView[] _allGroundView;

        internal OnObstacleController(UnitModel unit, IUnitView iUnitView,SubscriptionField<bool> isObstacle)
        {
            _unit = unit;
            _transform = iUnitView.ObjectTransform;
            _isObstacle = isObstacle;

            _allGroundView = iUnitView.ObjectTransform.GetComponentsInChildren<OnTriggerView>();
            foreach (var item in _allGroundView)
                item.evtUpdate += DetectGround;
            count = 0;
        }

        protected override void OnDispose()
        {
            foreach (var item in _allGroundView)
                item.evtUpdate -= DetectGround;
        }

        private void DetectGround(Collider _, bool isEnter)
        {
            if (isEnter) count++;
            else count--;
            if (count>0 && !_unit.isOnGround.Value) _isObstacle.Value=true;
            if (count==0 && _unit.isOnGround.Value) _isObstacle.Value =false;
        }
    }
}