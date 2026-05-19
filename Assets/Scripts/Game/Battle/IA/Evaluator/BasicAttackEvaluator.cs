namespace Game.Battle.IA.Evaluator {
    using System.Collections.Generic;
    using System.Linq;
    using Actions;
    using Item;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;

    public class BasicAttackEvaluator : AbstractActionEvaluator<AttackSelectionAction> {
        public BasicAttackEvaluator(BattleMapManager battleMapManager) : base(battleMapManager) {
        }

        protected override IEnumerable<DecisionResult>
            GetDecisions(ActionContext context, AttackSelectionAction action) {
            UnitObject killableTarget = this.GetKillableTarget(context);
            // in case on can kill target do the action
            if (killableTarget != null) {
                yield return new DecisionResult(action, killableTarget.Unit.GridPosition);
            }

            // in case on no kill target search possible targets by the basic attack
            IEnumerable<UnitObject> targets = this.GetAttackableTargets(context)
                .Where(target => killableTarget == null || target != killableTarget);
            foreach (UnitObject target in targets) {
                yield return new DecisionResult(action, target.Unit.GridPosition);
            }
        }

        protected override float GetScore(ActionContext context, AttackSelectionAction action,
            DecisionResult decision) {
            UnitObject target = this.BattleMapManager.GetUnit(decision.TargetPosition);
            if (target == null) {
                return 0f;
            }

            int damage = context.Enemy.Unit.UnitDamageResolver.EstimateDamage(target.Unit);
            int hp = target.Unit.GetCurrentIntStat(StatType.Hp);
            float score = 20f;
            score += damage * 10f;
            if (damage >= hp) {
                score += 100f;
            }

            if (DecisionUtilities.IsWeakTarget(target)) {
                score += 15f;
            }

            return score;
        }

        private IEnumerable<UnitObject> GetAttackableTargets(ActionContext context) {
            TileSearchConfig tileSearchConfig = new() {
                CanEnterCheck = false,
                CanSelect = unit => !unit.Unit.IsDead(),
                Range = context.Enemy.Unit.GetCurrentIntStat(StatType.Range),
                Target = Target.Enemy,
                SourceTeam = context.Enemy.Team.GetBattleTeam()
            };
            IReadOnlyList<TileData> reachableTiles =
                this.BattleMapManager.GetReachableTiles(context.CurrentPosition, tileSearchConfig);
            return reachableTiles.Select(tile => this.BattleMapManager.GetUnit(tile.TileGridPosition))
                .Where(unitObject => unitObject != null && unitObject.Unit != null)
                .OrderBy(unitObject => unitObject.Unit.GetCurrentIntStat(StatType.Hp));
        }

        private UnitObject GetKillableTarget(ActionContext context) =>
            this.GetAttackableTargets(context)
                .FirstOrDefault(target =>
                    context.Enemy.Unit.UnitDamageResolver.EstimateDamage(target.Unit) >=
                    target.Unit.GetCurrentIntStat(StatType.Hp));
    }
}
