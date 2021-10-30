
using System.Collections.Generic;

namespace Mans
{
    interface IReadOnlyGameModel
    {
        IReadOnlySubscriptionField<GameState> GameState { get; }
        IReadOnlySubscriptionField<int> Scores { get; }
        IReadOnlySubscriptionField<int> CurrentLevel { get; }
        IReadOnlySubscriptionField<int> GroundLayersBits { get; }
        IReadOnlySubscriptionField<int> ObstacleLayersBits { get; }

        void AddUnitView(IUnitView unitView);
        void RemoveUnitView(IUnitView unitView);
        IEnumerable<IUnitView> GetUnitsView();
        IEnumerable<IUnitView> GetUnitsView((TypeUnit, int) type);
    }
}