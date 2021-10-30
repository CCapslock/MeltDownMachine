namespace Mans
{
    using UnityEngine;

    public class BypassingObstaclesController : ControllerBasic, IExecute
    {
        private const float _verticalOffsetMultiplier = 0.1f;

        private ControlLeak _controlLeak = new ControlLeak("BypassingObstaclesController");
        private SubscriptionField<Vector2> _control;
        private IUnitView _unitView;
        private DataUnitCfg _dataUnitCfg;
        private int _obstaclesLayers;
        private ChosenDirection _chosenDirection;
        private int _frameOffset;

        public BypassingObstaclesController(SubscriptionField<Vector2> control, int obstaclesLayers, IUnitView unitView, DataUnitCfg dataUnitCfg)
        {
            _control = control;
            _obstaclesLayers = obstaclesLayers;
            _unitView = unitView;
            _dataUnitCfg = dataUnitCfg;
            _frameOffset = Random.Range(0, _dataUnitCfg.ObstaclesCheckPeriod);
        }

        public void Execute(float deltaTime)
        {

            if (_control.Value == Utils.Vectro2Zero && !_dataUnitCfg.IsAllwaysMove) return;

            bool checkForObstacles = (Time.frameCount + _frameOffset) % _dataUnitCfg.ObstaclesCheckPeriod == 0;
            if (!checkForObstacles) return;

            if (!CheckObstaclesCustom(false) && !CheckObstaclesCustom(true)) return;

            float rotate = _chosenDirection switch
            {
                ChosenDirection.Left => -2.0f,
                ChosenDirection.Right => 2.0f,
                _ => 0.0f
            };

            _control.Value = new Vector2(rotate, _control.Value.y);
        }

        //private bool CheckObstacles()
        //{

        //    var centerRayPoint = _unitView.ObjectTransform.position + _unitView.ObjectTransform.up * _verticalOffsetMultiplier;
        //    var rightRayPoint = centerRayPoint + _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth;
        //    var leftRayPoint = centerRayPoint - _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth;

        //    var rightHit = Physics.Raycast(rightRayPoint, _unitView.ObjectTransform.forward, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers);
        //    var leftHit = Physics.Raycast(leftRayPoint, _unitView.ObjectTransform.forward, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers);
        //    var rightHitSide = Physics.Raycast(centerRayPoint, _unitView.ObjectTransform.right, _dataUnitCfg.ObstaclesDetectionHalfWidth, _obstaclesLayers);
        //    var leftHitSide = Physics.Raycast(centerRayPoint, -_unitView.ObjectTransform.right, _dataUnitCfg.ObstaclesDetectionHalfWidth, _obstaclesLayers);

        //    //Debug.Log($"leftHit:{leftHit} rightHit:{rightHit} leftHitSide:{leftHitSide} rightHitSide:{rightHitSide}");


        //    if (!leftHit && !rightHit && !leftHitSide && !rightHitSide)
        //    {
        //        Debug.DrawLine(rightRayPoint, centerRayPoint + _unitView.ObjectTransform.right *
        //            _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.blue);
        //        Debug.DrawLine(leftRayPoint, centerRayPoint - _unitView.ObjectTransform.right *
        //            _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.blue);
        //        Debug.DrawLine(centerRayPoint, centerRayPoint + _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth, Color.blue);
        //        Debug.DrawLine(centerRayPoint, centerRayPoint - _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth, Color.blue);

        //        return false;
        //    }

        //    Debug.DrawLine(rightRayPoint, centerRayPoint + _unitView.ObjectTransform.right *
        //        _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.red);
        //    Debug.DrawLine(leftRayPoint, centerRayPoint - _unitView.ObjectTransform.right * 
        //        _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.red);
        //    Debug.DrawLine(centerRayPoint, centerRayPoint + _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth, Color.red);
        //    Debug.DrawLine(centerRayPoint, centerRayPoint - _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth, Color.red);


        //    float rightDistance;
        //    float leftDistance;
        //    float currentAngle = _dataUnitCfg.ObstaclesAngleStep;

        //    var objectTransform = _unitView.ObjectTransform;
        //    do
        //    {
        //        RaycastHit hit;

        //        var rightAngleDirection = Vector3.Lerp(objectTransform.forward, objectTransform.right, currentAngle);
        //        if (rightHitSide) rightDistance = 0;
        //        else
        //        {
        //            if (Physics.Raycast(rightRayPoint, rightAngleDirection, out hit, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers))
        //                rightDistance = hit.distance;
        //            else
        //                rightDistance = System.Single.MaxValue;
        //        }

        //        var leftAngleDirection = Vector3.Lerp(objectTransform.forward, -objectTransform.right, currentAngle);
        //        if (leftHitSide) leftDistance = 0;
        //        else
        //        {
        //            if (Physics.Raycast(leftRayPoint, leftAngleDirection, out hit, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers))
        //                leftDistance = hit.distance;
        //            else
        //                leftDistance = System.Single.MaxValue;
        //        }

        //        currentAngle += _dataUnitCfg.ObstaclesAngleStep;
        //    } while (rightDistance != System.Single.MaxValue && leftDistance != System.Single.MaxValue && currentAngle < 1.0f);

        //    if (rightDistance > leftDistance)
        //        _chosenDirection = ChosenDirection.Right;
        //    else
        //        _chosenDirection = ChosenDirection.Left;
        //    return true;
        //}

        private bool CheckObstacles()
        {

            var centerRayPoint = _unitView.ObjectTransform.position + _unitView.ObjectTransform.up * _verticalOffsetMultiplier;
            var rightRayPoint = centerRayPoint + _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth;
            var leftRayPoint = centerRayPoint - _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth;

            var rightHit = Physics.Raycast(rightRayPoint, _unitView.ObjectTransform.forward, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers);
            var leftHit = Physics.Raycast(leftRayPoint, _unitView.ObjectTransform.forward, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers);

            if (!leftHit && !rightHit)
            {
                Debug.DrawLine(rightRayPoint, centerRayPoint + _unitView.ObjectTransform.right *
                    _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.blue);
                Debug.DrawLine(leftRayPoint, centerRayPoint - _unitView.ObjectTransform.right *
                    _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.blue);
                return false;
            }

            Debug.DrawLine(rightRayPoint, centerRayPoint + _unitView.ObjectTransform.right *
                _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.red);
            Debug.DrawLine(leftRayPoint, centerRayPoint - _unitView.ObjectTransform.right *
                _dataUnitCfg.ObstaclesDetectionHalfWidth + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.red);

            float rightDistance;
            float leftDistance;
            float currentAngle = _dataUnitCfg.ObstaclesAngleStep;

            var objectTransform = _unitView.ObjectTransform;
            do
            {
                RaycastHit hit;

                var rightAngleDirection = Vector3.Lerp(objectTransform.forward, objectTransform.right, currentAngle);
                if (Physics.Raycast(rightRayPoint, rightAngleDirection, out hit, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers))
                    rightDistance = hit.distance;
                else
                    rightDistance = System.Single.MaxValue;

                var leftAngleDirection = Vector3.Lerp(objectTransform.forward, -objectTransform.right, currentAngle);
                if (Physics.Raycast(leftRayPoint, leftAngleDirection, out hit, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers))
                    leftDistance = hit.distance;
                else
                    leftDistance = System.Single.MaxValue;

                currentAngle += _dataUnitCfg.ObstaclesAngleStep;
            } while (rightDistance != System.Single.MaxValue && leftDistance != System.Single.MaxValue && currentAngle < 1.0f);

            if (rightDistance > leftDistance)
                _chosenDirection = ChosenDirection.Right;
            else
                _chosenDirection = ChosenDirection.Left;
            return true;
        }

        private bool CheckObstaclesCustom(bool isBack)
        {
            var centerRayPoint = _unitView.ObjectTransform.position + _unitView.ObjectTransform.up * _verticalOffsetMultiplier;
            var rightRayPoint = centerRayPoint + _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth;
            var leftRayPoint = centerRayPoint - _unitView.ObjectTransform.right * _dataUnitCfg.ObstaclesDetectionHalfWidth;

            //var rightHit = CustomRaycast(isBack, rightRayPoint, _unitView.ObjectTransform.forward, out _, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers);
            //var leftHit = CustomRaycast(isBack, leftRayPoint, _unitView.ObjectTransform.forward, out _, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers);

            //if (!leftHit && !rightHit)
            //{
            //    Debug.DrawLine(rightRayPoint, rightRayPoint + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.blue);
            //    Debug.DrawLine(leftRayPoint, leftRayPoint + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.blue);
            //    return false;
            //}

            //Debug.DrawLine(rightRayPoint, rightRayPoint + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.red);
            //Debug.DrawLine(leftRayPoint, leftRayPoint + _unitView.ObjectTransform.forward * _dataUnitCfg.MaxObstaclesDistance, Color.red);

            float rightDistance;
            float leftDistance;
            float currentAngle = 0;
            int  pass = 0;

            var objectTransform = _unitView.ObjectTransform;
            do
            {
                RaycastHit hit;

                var rightAngleDirection = Vector3.Lerp(objectTransform.forward, objectTransform.right, currentAngle);
                if (CustomRaycast(isBack, rightRayPoint, rightAngleDirection, out hit, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers))
                    rightDistance = hit.distance;
                else
                    rightDistance = System.Single.MaxValue;

                var leftAngleDirection = Vector3.Lerp(objectTransform.forward, -objectTransform.right, currentAngle);
                if (CustomRaycast(isBack, leftRayPoint, leftAngleDirection, out hit, _dataUnitCfg.MaxObstaclesDistance, _obstaclesLayers))
                    leftDistance = hit.distance;
                else
                    leftDistance = System.Single.MaxValue;

                currentAngle += _dataUnitCfg.ObstaclesAngleStep;
                pass++; 
            } while (rightDistance != System.Single.MaxValue && leftDistance != System.Single.MaxValue && currentAngle < 1.0f);

            if (rightDistance == System.Single.MaxValue && leftDistance == System.Single.MaxValue && pass==1) return false;

            if (rightDistance > leftDistance)
                _chosenDirection = ChosenDirection.Right;
            else 
                _chosenDirection = ChosenDirection.Left;
            return true;
        }

        private bool CustomRaycast(bool isBack, Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask)
        {
            if (!isBack)
            {
                if (Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask))
                {
                    DrawTrace(origin, direction, maxDistance, Color.red);
                    return true;
                }
                DrawTrace(origin, direction, maxDistance, Color.blue);
                return false;
            }
            if (Physics.Raycast(origin + direction * maxDistance, -direction, out hitInfo, maxDistance, layerMask))
            {
                DrawTrace(origin + direction * maxDistance, -direction, maxDistance, Color.red);
                return true;
            }
            DrawTrace(origin + direction * maxDistance, -direction, maxDistance, Color.blue);
            return false;
        }

        private void DrawTrace(Vector3 origin, Vector3 direction, float distance, Color color) =>
            Debug.DrawLine(origin, origin + direction * distance,color);

        private enum ChosenDirection
        {
            None = 0,
            Left = -1,
            Right = 1
        }
    }
}
