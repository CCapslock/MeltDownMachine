using UnityEngine;
using RootMotion.Dynamics;
using System;

namespace Mans
{
    internal sealed class PuppetController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak("PuppetController");
        private int _animatorFalling = Animator.StringToHash("Falling");
        private int _animatorMove = Animator.StringToHash("Move");

        private UnitModel _unitModel;
        private Animator _animator;
        private IInteractive _interactive;
        private PuppetMaster _puppetMaster;
        private DataUnitCfg _dataUnitCfg;

        private PuppetCfg _puppetCfg;
        private IReadOnlySubscriptionField<bool> _isJump;
        private IUnitView _unitView;
        private Collider[] _colliders;
        private AnimatorManView _animatorManView;
        private float _currentVelocity = 0;

        private Transform _skeleton;
        private Vector3 _differenceSkeleton;
        private Quaternion _startAngle;

        internal PuppetController(UnitModel unitModel, IReadOnlySubscriptionField<bool> isJump, IUnitView unitView, 
            IInteractive interactive, Transform folderPuppet, DataUnitCfg dataUnitCfg, PuppetCfg puppetCfg)
        {
            _unitModel = unitModel;
            _isJump = isJump;
            _unitView = unitView;
            _interactive = interactive;
            _dataUnitCfg = dataUnitCfg;
            _puppetCfg = puppetCfg;
            _animator = folderPuppet.GetComponentInChildren<Animator>();
            _animatorManView = _animator.gameObject.GetComponent<AnimatorManView>();
            _animatorManView.evtStandUpComplete += StandUpComplete;
            _animatorManView.evtFailComplete += FailComplete;

            _puppetMaster = folderPuppet.GetComponentInChildren<PuppetMaster>();
            _skeleton = folderPuppet.GetComponentInChildren<TagSkeleton>().transform;
            _differenceSkeleton = _skeleton.transform.localPosition - _unitView.ObjectTransform.localPosition;
            _startAngle = _unitView.ObjectTransform.rotation;

            _colliders = _unitView.ObjectTransform.GetComponents<Collider>();

            _interactive.evtAttack += Attack;
            _isJump.Subscribe(Jump);
        }

        protected override void OnDispose()
        {
            _animatorManView.evtStandUpComplete -= StandUpComplete;
            _animatorManView.evtFailComplete -= FailComplete;
            _interactive.evtAttack -= Attack;
            _isJump.UnSubscribe(Jump);
        }

        private void Jump(bool isJump)
        {
            if (!isJump) return;
            if (_unitModel.StateUnit.Value == StateUnit.Normal || _unitModel.StateUnit.Value == StateUnit.StandUp ||
                _unitModel.StateUnit.Value == StateUnit.Falling) return;

            _unitView.ObjectTransform.localPosition = _skeleton.localPosition -
                Quaternion.FromToRotation(_skeleton.transform.up, _unitView.ObjectTransform.transform.up) * _differenceSkeleton;

            _animator.SetBool(_animatorFalling, false);
            _unitModel.StateUnit.Value = StateUnit.StandUp;
        }

        private void StandUpComplete()
        {
            _unitModel.StateUnit.Value = StateUnit.Normal;
            _colliders[1].enabled = true;
        }

        private void FailComplete()
        {
            _unitModel.StateUnit.Value = StateUnit.Fail;
        }

        private (int, bool) Attack(PackInteractiveData arg)
        {
            if (_unitModel.StateUnit.Value == StateUnit.Normal || _unitModel.StateUnit.Value == StateUnit.StandUp)
            {
                _animator.SetBool(_animatorFalling, true);
                _unitModel.StateUnit.Value = StateUnit.Falling;
                _puppetMaster.pinWeight = _puppetCfg.WeightFall;
                _puppetMaster.mappingWeight = _puppetCfg.MappingWeghtFall;
                _colliders[1].enabled = false;
            }
            return (0, false);
        }

        public void Execute(float deltaTime)
        {
            //if (Input.GetButtonDown("Fire1"))
            //{
            //    _unitView.ObjectTransform.localPosition = _skeleton.localPosition 
            //        ;
            //}
            //if (Input.GetButtonDown("Fire2"))
            //{
            //    _unitView.ObjectTransform.localPosition = _skeleton.localPosition -
            //        Quaternion.FromToRotation(_skeleton.transform.up, _unitView.ObjectTransform.transform.up) * _differenceSkeleton;
            //}


            if (_unitModel.StateUnit.Value == StateUnit.Normal || _unitModel.StateUnit.Value == StateUnit.StandUp)
            {
                if (!_puppetMaster.pinWeight.Equals(_puppetCfg.WeightNormal) || !_puppetMaster.mappingWeight.Equals(_puppetCfg.MappingWeghtNormal))
                {
                    _puppetMaster.pinWeight = Mathf.MoveTowards(_puppetMaster.pinWeight, _puppetCfg.WeightNormal, deltaTime * _puppetCfg.PowerMoveToNormal);
                    _puppetMaster.mappingWeight = Mathf.MoveTowards(_puppetMaster.mappingWeight, _puppetCfg.MappingWeghtNormal, deltaTime * _puppetCfg.PowerMoveToNormal);
                }
                var velocity = _unitView.ObjectRigidbody.velocity.Change(y: 0);
                _currentVelocity = Mathf.MoveTowards(_currentVelocity, velocity.magnitude / _dataUnitCfg.MaxSpeed, deltaTime*10);
                _animator.SetFloat(_animatorMove, _currentVelocity);
            }

        }
    }
}
