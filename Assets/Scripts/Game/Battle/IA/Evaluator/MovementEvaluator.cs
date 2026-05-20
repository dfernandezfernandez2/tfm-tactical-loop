namespace Game.Battle.IA.Evaluator {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Actions;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;

    public class MovementEvaluator : AbstractActionEvaluator<MovementSelectionAction> {
        private const int _maxCandidatePositions = 8;

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
            UnitObject closestTarget =
                DecisionUtilities.GetClosestEnemy(context.TurnOrder, context.Enemy, context.CurrentPosition);
            IReadOnlyList<TileData> reachableTiles =
                this.BattleMapManager.GetReachableTiles(context.CurrentPosition, config);
            IEnumerable<GridPosition> positions = reachableTiles.Select(tile => tile.TileGridPosition)
                .Where(position => !position.Equals(context.CurrentPosition));
            return closestTarget == null
                ? positions.Take(_maxCandidatePositions)
                : positions.OrderByDescending(position => GetPositionScore(context, position, closestTarget))
                    .Take(_maxCandidatePositions);
        }

        protected override float GetScore(ActionContext context, MovementSelectionAction action,
            DecisionResult decision) => 0f;

        private static float GetPositionScore(ActionContext context, GridPosition position, UnitObject target) {
            int distance = DecisionUtilities.GetDistance(position, target.Unit.GridPosition);
            return context.Enemy.data.isRanged
                ? ScoreRangedPosition(context.Enemy.Actions.GetMaxSkillRange(), distance)
                : ScoreMeleePosition(distance);
        }

        private static float ScoreMeleePosition(int distance) => distance == 1 ? 150f : -distance * 20f;

        private static float ScoreRangedPosition(int attackRange, int distance) {
            int minSafeDistance = Math.Max(2, (int)Math.Ceiling(attackRange * 0.5f));
            int distanceToIdeal = Math.Abs(distance - attackRange);
            float score = 0f;
            if (distance <= 1) {
                score -= 250f;
            }

            if (distance <= minSafeDistance) {
                score -= 100f;
            }

            if (distance <= attackRange) {
                score += 50f;
            }

            score -= distanceToIdeal * 15f;
            return score;
        }
    }
}
