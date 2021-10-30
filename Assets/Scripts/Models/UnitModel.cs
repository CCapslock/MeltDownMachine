using System;

namespace Mans
{
    internal enum StateUnit
    {
        None,
        Normal,
        Falling,
        Fail,
        StandUp
            
    }

    public sealed class UnitModel
    {
        private ControlLeak _controlLeak = new ControlLeak("UnitMData");

        internal event Action evtKill = delegate { };
        internal event Action evtLives = delegate { };
        internal event Action evtDecLives = delegate { };

        private int _hp = -1;
        internal int HP
        {
            get => _hp;
            set
            {
                if (_hp != value && (_hp > -1 || value > 0 || value == -1000))
                {
                    if (_hp < value)
                    {
                        _hp = value;
                        evtLives.Invoke();
                    }

                    if (_hp > value)
                    {
                        evtDecLives.Invoke();
                        evtLives.Invoke();

                        if ((_hp > 0 && value <= 0) || value == -1000)
                        {
                            evtKill();
                        }

                        _hp = value;
                        _hp = _hp < 0 ? 0 : _hp;

                    }

                }
            }
        }

        internal SubscriptionField<float> MaxSpeed { get; }
        internal SubscriptionField<float> powerJump { get; }
        internal SubscriptionField<PackInteractiveData> packInteractiveData { get; }
        internal SubscriptionField<bool> isOnGround { get; }
        internal SubscriptionField<bool> isShielded { get; }
        internal SubscriptionField<(TypeUnit typeUnit, int cfg)> killTypeItem { get; }
        internal SubscriptionField<float> CoefficientDrift { get; }

        internal SubscriptionField<StateUnit> StateUnit { get; }



        public UnitModel()
        {
            packInteractiveData = new SubscriptionField<PackInteractiveData>();
            isOnGround = new SubscriptionField<bool>();
            MaxSpeed = new SubscriptionField<float>();
            powerJump = new SubscriptionField<float>();
            killTypeItem = new SubscriptionField<(TypeUnit typeUnit, int cfg)>();
            isShielded = new SubscriptionField<bool>();
            CoefficientDrift = new SubscriptionField<float> { Value = 1 };
            StateUnit = new SubscriptionField<StateUnit> {Value=Mans.StateUnit.Normal };
        }
    }
}