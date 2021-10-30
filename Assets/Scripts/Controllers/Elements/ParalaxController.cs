using UnityEngine;

namespace Mans
{
    internal class ParalaxController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak("ParallaxController");
        private Transform _transform;
        private Transform _targetTransform;
        private Vector3 _offset;
        private Quaternion _offsetRotation;
        private float _offsetDistance;
        private ParalaxCfg _paralaxCfg;

        internal ParalaxController(Transform transform, Transform targetTransform, ParalaxCfg paralaxCfg)
        {
            _paralaxCfg = paralaxCfg;
            _transform = transform;
            _targetTransform = targetTransform;


            _offset = _transform.position - _targetTransform.position;
            _offsetRotation = _transform.rotation * Quaternion.Inverse(_targetTransform.rotation);
            _offsetDistance = _offset.magnitude;

            

        }

        public void Execute(float deltaTime)
        {
            MakePosition(deltaTime);
        }

        private void MakePosition(float deltaTime)
        {
            if (_targetTransform != null)
            {

                //_transform.position =
                //    Vector3.Lerp(_transform.position,
                //    _targetTransform.position + _offsetDistance * _targetTransform.up + _targetTransform.forward * _paralaxCfg.AddPositionForward
                //    + _targetTransform.right * _paralaxCfg.AddPositionRight
                //    , deltaTime * _paralaxCfg.ElasticPower);

                _transform.position =
                    Vector3.Lerp(_transform.position,
                      _targetTransform.position +  Quaternion.Euler(_paralaxCfg.SeeAngle) * Vector3.up *_offsetDistance

                    , deltaTime * _paralaxCfg.ElasticPower);

                _transform.rotation = Quaternion.Lerp(_transform.rotation, 
                    Quaternion.LookRotation( _targetTransform.position- _transform.position), 
                    
                    deltaTime * _paralaxCfg.ElasticPower);

                //_transform.LookAt(_targetTransform.position, _transform.up);

                //_transform.rotation=Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(_targetTransform.position, Vector3.up),
                //    deltaTime * _paralaxCfg.ElasticPower);
                //_transform.LookAt(_targetTransform.position,Vector3.up);
            }
        }
    }
}