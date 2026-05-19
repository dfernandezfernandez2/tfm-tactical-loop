namespace Game.Battle.IA.Evaluator {
    using System.Collections.Generic;
    using Actions;
    using Item;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;
    using Unit.Skills;

    public class SkillEvaluator : AbstractActionEvaluator<SkillAction> {
        public SkillEvaluator(BattleMapManager battleMapManager) : base(battleMapManager) {
        }

        protected override IEnumerable<DecisionResult> GetDecisions(ActionContext context, SkillAction action) {
            Skill skill = action.Skill;
            if (!action.CanDoAction(null, context.Enemy)) {
                yield break;
            }

            if (skill.target == Target.Self) {
                if (action.CanSelect(context.Enemy)) {
                    yield return new DecisionResult(action, context.Enemy.Unit.GridPosition);
                }

                yield break;
            }

            TileSearchConfig config = new() {
                Range = skill.range,
                CanEnterCheck = false,
                Target = skill.target,
                CanSelect = action.CanSelect,
                ApplyHeightLineOfSight = skill.applyHeightLineOfSight,
                RequiresLineOfSight = true,
                SourceTeam = context.Enemy.Team.GetBattleTeam()
            };
            foreach (TileData reachableTile in
                     this.BattleMapManager.GetReachableTiles(context.CurrentPosition, config)) {
                yield return new DecisionResult(action, reachableTile.TileGridPosition);
            }
        }

        protected override float GetScore(ActionContext context, SkillAction action, DecisionResult decision) {
            UnitObject target = this.BattleMapManager.GetUnit(decision.TargetPosition);
            if (target == null) {
                return 0f;
            }

            Skill skill = action.Skill;
            float score = 0f;
            switch (skill.target) {
                case Target.Enemy: {
                    score += 35f;
                    if (DecisionUtilities.IsWeakTarget(target)) {
                        score += 20f;
                    }

                    int targetHp = target.Unit.GetCurrentIntStat(StatType.Hp);
                    int userAtk = context.Enemy.Unit.GetCurrentIntStat(StatType.Atk);
                    if (targetHp <= userAtk * 2) {
                        score += 40f;
                    }

                    break;
                }
                case Target.Ally or Target.Self: {
                    float hp = target.Unit.GetCurrentStat(StatType.Hp);
                    float maxHp = target.Unit.GetMaxStat(StatType.Hp);
                    if (hp < maxHp * 0.5f) {
                        score += 45f;
                    }

                    if (target == context.Enemy) {
                        score += 10f;
                    }

                    break;
                }
            }

            score -= skill.apCost * 5f;
            score -= skill.manaCost * 0.5f;
            return score;
        }
    }
}
