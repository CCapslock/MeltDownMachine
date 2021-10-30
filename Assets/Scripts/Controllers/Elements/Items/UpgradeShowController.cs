using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Mans
{
    internal sealed class UpgradeShowController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("UpgradeShowController");
        private const string _nameRes = "Items/Upgrades/EquipPlace";
        private UpgradeModel _upgradeModel;
        private Dictionary<ItemCfg, Image> placeUpgradesTransform = new Dictionary<ItemCfg, Image>();
        private RectTransform _equipPlace;
        private Vector3 _originalPositionPlace;

        internal UpgradeShowController(UpgradeModel upgradeModel)
        {
            var data = CreateGameObject(Reference.Canvas, _nameRes);
            _equipPlace = data.gameObject.GetComponent<RectTransform>();
            _originalPositionPlace = _equipPlace.position;
            _upgradeModel = upgradeModel;
            foreach (var item in _upgradeModel.allPlaces)
            {
                //Debug.Log($"Place:{item.Value.name}");
                var currentPlace = data.gameObject.transform.Find(item.Value.name);
                if (currentPlace != null)
                {
                    if (currentPlace.gameObject.TryGetComponent(out Image image))
                    {
                        placeUpgradesTransform.Add(item.Value, image);
                        image.enabled = false;
                    }
                }
                else Debug.LogWarning($"Dont find the transform Place:{item.Value.name}");
            }
            _upgradeModel.EvtAddItem += AddItem;
            _upgradeModel.EvtRemoveItem += RemoveItem;
        }

        protected override void OnDispose()
        {
            _upgradeModel.EvtAddItem -= AddItem;
            _upgradeModel.EvtRemoveItem -= RemoveItem;
        }

        private void AddItem(UpgradeItemCfg upgradeItemCfg, bool isHave)
        {
            if (isHave) return;
            if (placeUpgradesTransform.TryGetValue(upgradeItemCfg.PlaceOfUpgrade, out Image image))
            {
                image.enabled = true;
                image.sprite = upgradeItemCfg.Sprite;

                DOTween.Sequence().Append(_equipPlace.DOShakePosition(2f, 30f))
                .Append(_equipPlace.DOMove(_originalPositionPlace, 1));
            }
            else Debug.LogWarning($"There is no installable place:{upgradeItemCfg.PlaceOfUpgrade.name}");
        }

        private void RemoveItem(UpgradeItemCfg upgradeItemCfg)
        {
            if (placeUpgradesTransform.TryGetValue(upgradeItemCfg.PlaceOfUpgrade, out Image image))
            {
                image.enabled = false;
            }
            else Debug.LogWarning($"There is no installable place:{upgradeItemCfg.PlaceOfUpgrade.name}");
        }
    }
}
