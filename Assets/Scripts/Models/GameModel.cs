
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mans
{
    public sealed class GameModel:IReadOnlyGameModel
    {
        #region Fields 

        public SubscriptionField<GameState> GameState { get; }
        public SubscriptionField<int> Scores { get; }
        public SubscriptionField<int> CurrentLevel { get; }
        public SubscriptionField<int> GroundLayersBits { get; }
        public SubscriptionField<int> ObstacleLayersBits { get; }

        #endregion


        #region ReadOnlyFields

        IReadOnlySubscriptionField<GameState> IReadOnlyGameModel.GameState => GameState;

        IReadOnlySubscriptionField<int> IReadOnlyGameModel.Scores => Scores;

        IReadOnlySubscriptionField<int> IReadOnlyGameModel.CurrentLevel => CurrentLevel;

        IReadOnlySubscriptionField<int> IReadOnlyGameModel.GroundLayersBits => GroundLayersBits;

        IReadOnlySubscriptionField<int> IReadOnlyGameModel.ObstacleLayersBits => ObstacleLayersBits;

        #endregion


        #region ChangeUnitsView

        private Dictionary<(TypeUnit,int),List<IUnitView>> _unitsView=new Dictionary<(TypeUnit, int), List<IUnitView>>();
        public event Action<(TypeUnit type, int cfg), bool> EvtChangeUnitsView;

            
        public void AddUnitView(IUnitView unitView)
        {
            var type = unitView.GetTypeItem();
            if (!_unitsView.ContainsKey(type)) _unitsView[type] = new List<IUnitView>();
            _unitsView[type].Add(unitView);
            EvtChangeUnitsView?.Invoke(type, true);
        }

        public void RemoveUnitView(IUnitView unitView)
        {
            var type = unitView.GetTypeItem();
            if (_unitsView.ContainsKey(type))
            {
                _unitsView[type].Remove(unitView);
                EvtChangeUnitsView?.Invoke(type, false);
            }
            else Debug.LogWarning($"Dont remove {type} key");
        }

        public IEnumerable<IUnitView> GetUnitsView()
        {
            foreach (var item in _unitsView)
                for (int i = 0; i < item.Value.Count; i++)
                    if (item.Value[i].ObjectTransform != null) yield return item.Value[i];
        }

        public IEnumerable<IUnitView> GetUnitsView((TypeUnit, int) type)
        {
            if (_unitsView.TryGetValue(type , out List<IUnitView> item))
                for (int i = 0; i < item.Count; i++)
                    if (item[i].ObjectTransform!=null) yield return item[i];
        }

        #endregion


        #region Init

        public GameModel()
        {
            GameState = new SubscriptionField<GameState>();
            Scores = new SubscriptionField<int>();
            CurrentLevel = new SubscriptionField<int> { Value = 0 };
            GroundLayersBits = new SubscriptionField<int>();
            ObstacleLayersBits = new SubscriptionField<int>();
        }

        #endregion
    }
}