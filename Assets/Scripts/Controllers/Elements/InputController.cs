using UnityEngine;

namespace Mans
{
    internal sealed class InputController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak("InputController");
        private ControlModel _controlM;
        private Vector2 _vector2Zero= Vector2.zero;

        internal InputController(ControlModel controlM)
        {
            //Debug.Log($"Init Input controller");
            _controlM = controlM;
        }

        public void Execute(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0))
            {   
                _controlM.IsNewTouch.Value = true;
                
            }
            else _controlM.IsNewTouch.Value = false;

            if (Input.GetMouseButton(0))
            {
                _controlM.PositionCursor.Value = Reference.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            //var currentControlValue = new Vector2(0.1f, Input.GetAxisRaw("Vertical"));            
            var currentControlValue = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (currentControlValue != _controlM.Control.Value) _controlM.Control.Value = currentControlValue;
            _controlM.IsJump.Value = Input.GetButtonDown("Jump");
        }
    }

}