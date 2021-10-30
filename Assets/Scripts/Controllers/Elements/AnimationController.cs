using System.Linq;
using UnityEngine;

namespace Mans
{
    internal sealed class AnimationController : ControllerBasic, IController, IExecute, IInitialization
    {
        private ControlLeak _controlLeak = new ControlLeak("AnimationController");
        private IUnitView _unitView;
        private AnimationCfg _animationCfg;
        private AnimationData _animationData;
        private float _currentFrame;
        private bool _isStop;
        private IReadOnlySubscriptionField<TypeAnimation> _typeAnimation;
        private bool isMirrored;

        internal AnimationController(IReadOnlySubscriptionField<TypeAnimation> typeAnimation, IUnitView unitView, AnimationCfg animationCfg)
        {
            _typeAnimation = typeAnimation;
            _unitView = unitView;
            _typeAnimation.Subscribe(SetAnimation);
            _animationCfg = animationCfg;
        }

        protected override void OnDispose()
        {
            _typeAnimation.UnSubscribe(SetAnimation);
        }

        public void Initialization()
        {
            SetAnimation(TypeAnimation.Idle);
        }

        private void SetAnimation(TypeAnimation typeAnimation)
        {
            _animationData = _animationCfg.animationData.Where(ad => ad.typeAnimation == typeAnimation).FirstOrDefault();
            if (_animationData == null) Debug.LogWarning($"Dont set animation {typeAnimation} on {(_unitView as MonoBehaviour).name} object");
            _currentFrame = 0;
            _isStop = false;
        }

        private void UpdateSprite()
        {
            //unitView.objectSpriteRednderer.sprite = _animationData.sprites[(int)_currentFrame];
        }
        

        public void Execute(float deltaTime)
        {
            Animation(deltaTime);
            SetMirror();
        }

        private void SetMirror()
        {
            if (_animationCfg.IsMirror)
            {
                if (_unitView.ObjectRigidbody.velocity.x < 0 && !isMirrored)
                {
                    isMirrored = true;
                    _unitView.ObjectTransform.localScale = new Vector3(
                        -_unitView.ObjectTransform.localScale.x,
                        _unitView.ObjectTransform.localScale.y,
                        _unitView.ObjectTransform.localScale.z
                        );
                }
                if (_unitView.ObjectRigidbody.velocity.x > 0 && isMirrored)
                {
                    isMirrored = false;
                    _unitView.ObjectTransform.localScale = new Vector3(
                        -_unitView.ObjectTransform.localScale.x,
                        _unitView.ObjectTransform.localScale.y,
                        _unitView.ObjectTransform.localScale.z
                        );
                }
            }
        }

        private void Animation(float deltaTime)
        {
            if (!_isStop)
            {
                _currentFrame += _animationCfg.Speed * deltaTime;
//                Debug.Log($"Animation: {(_unitView as MonoBehaviour).gameObject.name} currentFrame:{_currentFrame}");
                if (_currentFrame >= _animationData.sprites.Length)
                {
                    if (_animationData.isLoop)
                    {
                        //_currentFrame -= _animationData.sprites.Length;
                        _currentFrame = _currentFrame % _animationData.sprites.Length;
                    }
                    else
                    { 
                        _currentFrame = _animationData.sprites.Length - 1;
                        _isStop = true;
                    }
                }
            }
            if (!_isStop) UpdateSprite();
        }
    }
}