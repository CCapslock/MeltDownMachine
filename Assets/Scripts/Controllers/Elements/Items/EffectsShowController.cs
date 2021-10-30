using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Mans
{
    internal sealed class EffectsShowController : ControllerBasic, IExecute
    {
        private ControlLeak _controlLeak = new ControlLeak("EffectsShowController");
        private const string _nameRes = "Items/Effects/EffectsPlace";
        private const string _nameResEffect = "Items/Effects/Effect";
        private EffectsModel _effectsModel;
        private Dictionary<int, EffectsImageData> _effectsImageData = new Dictionary<int, EffectsImageData>();
        private List<int> _orderEffects=new List<int>();
        //private int countEffects;
        private float heightImage;
        private Transform folder;

        internal EffectsShowController(EffectsModel effectsModel)
        {
            var data = CreateGameObject(Reference.Canvas, _nameRes);
            folder = data.gameObject.transform;
            _effectsModel = effectsModel;
            var go = CreateGameObject(folder, _nameResEffect).gameObject;
            _effectsImageData.Add(0, new EffectsImageData { gameObject = go });
            heightImage = go.GetComponent<RectTransform>().rect.height;
            go.SetActive(false);
            _effectsModel.EvtAddItem += AddItem;
            _effectsModel.EvtRemoveItem += RemoveItem;
        }

        protected override void OnDispose()
        {
            _effectsModel.EvtAddItem -= AddItem;
            _effectsModel.EvtRemoveItem -= RemoveItem;
        }

        private void AddItem(EffectsItemCfg effectsItemCfg, bool isHave)
        {
            if (!_effectsImageData.ContainsKey(effectsItemCfg.Id))
            {
                var go = GameObject.Instantiate(_effectsImageData[0].gameObject, folder);
                go.SetActive(true);
                var image = go.transform.GetChild(1).GetComponent<Image>();
                var text = go.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                image.sprite = effectsItemCfg.Sprite;
                //go.transform.localPosition = new Vector3(0, -_effectsImageData.Count * heightImage, 0);
                _effectsImageData.Add(effectsItemCfg.Id, new EffectsImageData { gameObject = go, text = text, endTime = Time.time + effectsItemCfg.Time });
                _orderEffects.Add(effectsItemCfg.Id);
                UpdatePositionItems();
            }
            else
            {
                _effectsImageData[effectsItemCfg.Id].endTime += effectsItemCfg.Time;
            }
            //Debug.Log($"Add EffectsShow:{effectsItemCfg.Id}");
            UpdateTimeEffect(effectsItemCfg.Id);
        }

        private void UpdatePositionItems()
        {
            for (int i = 0; i < _orderEffects.Count; i++)
            {
                _effectsImageData[_orderEffects[i]] .gameObject.transform.localPosition = new Vector3(0, -i * heightImage, 0);
            }
        }

        private void RemoveItem(EffectsItemCfg effectsItemCfg)
        {
            if (_effectsImageData.TryGetValue(effectsItemCfg.Id, out EffectsImageData currentEffects))
            {
                GameObject.Destroy(currentEffects.gameObject);
                _effectsImageData.Remove(effectsItemCfg.Id);
                _orderEffects.Remove(effectsItemCfg.Id);
            }
            UpdatePositionItems();
        }

        public void Execute(float deltaTime)
        {
            foreach (var item in _effectsImageData)
            {
                var key = item.Key;
                if (key != 0) UpdateTimeEffect(key);
            }
        }

        private void UpdateTimeEffect(int i)
        {
            var count = (int)(_effectsImageData[i].endTime - Time.time + 1);
            if (_effectsImageData[i].currentCount != count)
            {
                _effectsImageData[i].currentCount = count;
                _effectsImageData[i].text.text = count.ToString();
            }
        }
    }
}
