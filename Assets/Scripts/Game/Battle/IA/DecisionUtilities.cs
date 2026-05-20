namespace Game.Battle.IA {
    using System.Collections.Generic;
    using System.Linq;
    using Map.Data;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    public static class DecisionUtilities {
        public static bool IsWeakTarget(UnitObject target) =>
            target.Unit.GetCurrentStat(StatType.Hp) < target.Unit.GetMaxStat(StatType.Hp) * 0.5f;

        public static int GetDistance(GridPosition a, GridPosition b) =>
            Mathf.Abs(a.Position.x - b.Position.x) +
            Mathf.Abs(a.Position.y - b.Position.y);

        public static UnitObject GetClosestEnemy(IReadOnlyList<UnitObject> turnOrder, UnitObject unitObject,
            GridPosition currentPosition) =>
            turnOrder.Where(unit => unit != unitObject)
                .Where(unit => unit.Team.GetBattleTeam() != unitObject.Team.GetBattleTeam())
                .Where(unit => !unit.Unit.IsDead())
                .OrderBy(unit => GetDistance(currentPosition, unit.Unit.GridPosition)).FirstOrDefault();
    }
}
