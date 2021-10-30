using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mans
{
    internal sealed class LivesCanvasController : ControllerBasic
    {
        private ControlLeak _controlLeak = new ControlLeak("LivesCanvasController");
        private UnitModel _unit;
        private Transform _position;
        private List<GameObject> _livesIco=new List<GameObject>();
        private GameObject _prefabFlagIco;
        private float _widthIco = 25f;

        internal LivesCanvasController(UnitModel unit)
        {
            _unit = unit;
            _unit.evtLives += UpdateLives;
            _position = GameObject.FindObjectOfType<TagPosLives>().transform;
            _prefabFlagIco = LoadResources.GetValue<GameObject>("Canvas/LivesIco");
        }

        

        private void UpdateLives()
        {
            while (_livesIco.Count> _unit.HP)
            {
                GameObject.Destroy(_livesIco[_livesIco.Count - 1]);
                _livesIco.RemoveAt(_livesIco.Count - 1);
            }

            for (int i = 0; i < _unit.HP; i++)
            {
                if (_livesIco.Count < i + 1)
                {
                    var go = GameObject.Instantiate(_prefabFlagIco, _position);
                    go.transform.localPosition = new Vector3(i * _widthIco, 0, 0);
                    _livesIco.Add(go);
                }
            }
        }
    }
}