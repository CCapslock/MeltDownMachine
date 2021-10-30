using System.Collections;
using UnityEngine;

namespace Mans
{
    internal sealed class MoveTrackController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("MoveTrackController");
        private ITraectory _iTraectory;
        private int _numTraectory;
        private SubscriptionField<Transform> _targetPosition;
        private IReadOnlySubscriptionField<bool> _reachedTarget;
        private IEnumerator _currentCoroutine;

        internal MoveTrackController(SubscriptionField<Transform> targetPosition, IReadOnlySubscriptionField<bool> reachedTarget,  ITraectory iTraectory)
        {
            _iTraectory = iTraectory;
            _targetPosition = targetPosition;
            _reachedTarget = reachedTarget;
            _numTraectory = -1;
            NextPosition();
            _reachedTarget.Subscribe(EndPosition);
        }

        protected override void OnDispose()
        {
            _reachedTarget.UnSubscribe(EndPosition);
            CoroutinesView.inst.RemoveCoroutine(_currentCoroutine);
        }

        private void EndPosition(bool reachedTarget)
        {
            if (reachedTarget)
                _currentCoroutine= CoroutinesView.inst.AddCoroutine(WaitInPoint());
        }

        private IEnumerator WaitInPoint()
        {
            yield return new WaitForSeconds(_iTraectory.Track[_numTraectory].stopTime);
            NextPosition();
        }

        public void NextPosition()
        {
            if (_iTraectory.Track.Length > 0)
            {
                if (++_numTraectory>= _iTraectory.Track.Length) _numTraectory=0;
                _targetPosition.Value = _iTraectory.Track[_numTraectory].transform;
            }
        }
    }
}