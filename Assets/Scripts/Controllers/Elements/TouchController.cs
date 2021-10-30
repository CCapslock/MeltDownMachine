using UnityEngine;

namespace Mans
{
    internal sealed class TouchController : ControllerBasic, IExecute
    {
        private float _constMaxPowerControl = 2;
        private const float _constPowerScaleControl = 30;
        private ControlLeak _controlLeak = new ControlLeak("TouchController");
        private ControlModel _controlM;
        private Camera _camera;
        private Vector3 _startPosition;
        private Vector2 _vector2Zero = Vector2.zero;
        private float _dpi;
        private const float _constMinLenghthForward=10;

        internal TouchController(ControlModel controlM)
        {
            Debug.Log($"Init Toch controller");
            _controlM = controlM;
            _camera = Reference.MainCamera;
            _dpi = Screen.dpi;
        }

        public void Execute(float deltaTime)
        {
            _controlM.IsNewTouch.Value = false;
            if (Input.touches.Length > 0)
            {
                var item = Input.touches[0];
                if (item.phase == TouchPhase.Moved || item.phase == TouchPhase.Began)
                {
                    //var currentPosition = _camera.ScreenToWorldPoint(item.position) - _camera.transform.position;                    
                    var currentPosition = item.position / _dpi;
                    _controlM.PositionCursor.Value = _camera.ScreenToWorldPoint(item.position);

                    if (item.phase == TouchPhase.Began)
                    {
                        _controlM.IsNewTouch.Value = true;
                        _startPosition = currentPosition;
                        UpdateControlPosition(currentPosition);
                    }
                    if (item.phase == TouchPhase.Moved)
                    {
                        UpdateControlPosition(currentPosition);
                        _startPosition = currentPosition;
                    }

                }
            }
            else _controlM.Control.Value = _vector2Zero;
            if (Input.touches.Length == 2 && Input.touches[1].phase == TouchPhase.Began) _controlM.IsJump.Value = true;
            else _controlM.IsJump.Value = false;            
        }

        private void UpdateControlPosition(Vector3 currentPosition)
        {
            var control = (currentPosition - _startPosition) * _constPowerScaleControl;
            if (control.y < _constMinLenghthForward) control.y = 0;

            if (control.sqrMagnitude > _constMaxPowerControl * _constMaxPowerControl)
                control = control.normalized * _constMaxPowerControl;
            _controlM.Control.Value = control;
        }
    }

}