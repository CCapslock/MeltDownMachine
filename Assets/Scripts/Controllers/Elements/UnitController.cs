using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Mans
{
    internal sealed class UnitController : ControllerBasic, IInitialization
    {
        #region Fields

        private const int _shieldItemID = 2;
        ControlLeak _controlLeak = new ControlLeak("UnitController");
        private UnitModel _unit;
        private DataUnitCfg _unitData;
        private IInteractive _iInteractive;
        private GameObject _gameObject;
        private SubscriptionField<int> _scores;
        private EffectsModel _effectsModel;

        #endregion


        #region Init

        internal UnitController(UnitModel unit, SubscriptionField<int> scores, IInteractive iInteractive, TypeUnit typeItem, DataUnitCfg unitData)
        {
            _unit = unit;
            _scores = scores;
            _iInteractive = iInteractive;
            _unitData = unitData;
            _unit.packInteractiveData.Value = new PackInteractiveData(_unitData.AttackPower, typeItem);
            _gameObject = (_iInteractive as MonoBehaviour).gameObject;
            _unit.MaxSpeed.Value = unitData.MaxSpeed;
            _unit.powerJump.Value = unitData.PowerJump;
            _iInteractive.evtAttack += Attacked;
            _iInteractive.evtInteractive += OutInteractive;
            _unit.evtDecLives += DecLive;
        }

        protected override void OnDispose()
        {
            _unit.evtDecLives -= DecLive;
            if (_iInteractive != null)
                _iInteractive.evtInteractive -= OutInteractive;
        }

        public void Initialization()
        {
            _unit.HP = _unitData.MaxLive;
            _scores.Value = 0;
        }

        #endregion


        #region Game

        private void DecLive()
        {
            if (!_unitData.DestroyEffects.RuntimeKeyIsValid()) return;

            CreateGameObjectAddressable(_unitData.DestroyEffects, Reference.ActiveElements, _gameObject.transform.position, _gameObject.transform.rotation,
            (obj)=>
            {
                GameObject.Destroy(obj.gameObject, _unitData.TimeViewDestroyEffects);
            },TypeCreateForObject.UpPassParent
            );
        }


        private (int, bool) Attacked(PackInteractiveData pack)
        {
            int addScores = 0;
            bool isDead = false;
            if (_unit.HP != 0 && !_unit.isShielded.Value)
            {                
                //Debug.Log($"{(_iInteractive as MonoBehaviour).gameObject} Atacked by {pack.typeItem} ");
                _unit.HP -= pack.attackPower;
                if (_unit.HP == 0)
                {
                    addScores = _unitData.AddScores;
                    isDead = true;
                }
            }
            return (addScores, isDead);
        }

        private void OutInteractive(IInteractive ui, bool isEnter)
        {
            if (isEnter)
            {
                var result = ui.Attack(_unit.packInteractiveData.Value);
                if (result.isDead)
                {
                    if (ui is IUnitView)
                    {
                        var uv = ui as IUnitView;
                        var type = uv.GetTypeItem();
                        _unit.killTypeItem.Value = type;
                    }
                }
                //_scores.Value += result.scores;
                _unit.HP += result.scores;
            }
        }

        #endregion
    }
}
