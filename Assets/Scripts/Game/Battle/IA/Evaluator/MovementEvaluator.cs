namespace Game.Battle.IA.Evaluator {
    using System.Collections.Generic;
    using System.Linq;
    using Actions;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;

    public class MovementEvaluator : AbstractActionEvaluator<MovementSelectionAction> {
        public MovementEvaluator(BattleMapManager battleMapManager) : base(battleMapManager) {
        }

        protected override IEnumerable<DecisionResult> GetDecisions(ActionContext context,
            MovementSelectionAction action) =>
            this.GetCandidateMovementPositions(context)
                .Select(moveTarget => new DecisionResult(action, moveTarget));

        private IEnumerable<GridPosition> GetCandidateMovementPositions(ActionContext context) {
            TileSearchConfig config = new() {
                Range = context.Enemy.Unit.GetCurrentIntStat(StatType.Movement)
            };
            IReadOnlyList<TileData> reachableTiles =
                this.BattleMapManager.GetReachableTiles(context.CurrentPosition, config);
            UnitObject closestTarget = GetClosestEnemy(context);
            if (closestTarget == null) {
                return reachableTiles.Select(x => x.TileGridPosition).Where(x => !x.Equals(context.CurrentPosition))
                    .Take(6);
            }

            return reachableTiles.Select(x => x.TileGridPosition).Where(x => !x.Equals(context.CurrentPosition))
                .OrderBy(x => GetDistance(x, closestTarget.Unit.GridPosition)).Take(6);
        }

        private static UnitObject GetClosestEnemy(ActionContext context) =>
            context.TurnOrder.Where(unit => unit != context.Enemy)
                .Where(unit => unit.Team.GetBattleTeam() != context.Enemy.Team.GetBattleTeam())
                .Where(unit => !unit.Unit.IsDead())
                .OrderBy(unit => GetDistance(context.CurrentPosition, unit.Unit.GridPosition)).FirstOrDefault();

        protected override float GetScore(ActionContext context, MovementSelectionAction action,
            DecisionResult decision) => 0f;
    }
}
