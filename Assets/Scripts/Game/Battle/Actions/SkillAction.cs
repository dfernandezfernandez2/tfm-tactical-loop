namespace Game.Battle.Actions {
    using System.Collections;
    using System.Linq;
    using IA;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;
    using Unit.Skills;

    public class SkillAction : IBattleAction {
        public SkillAction(Skill skill) => this.Skill = skill;

        public Skill Skill { get; }

        public string GetName() => this.Skill.skillName;

        public string GetActionName() => ActionType.Skill.GetName() + "Action";

        public int GetApCost() => this.Skill.apCost;

        public IEnumerator Start(IBattleContext battleContext) {
            TileSearchConfig config = new() {
                Range = this.Skill.range,
                CanEnterCheck = false,
                Target = this.Skill.target,
                CanSelect = this.CanSelect,
                ApplyHeightLineOfSight = this.Skill.applyHeightLineOfSight,
                RequiresLineOfSight = true
            };
            yield return battleContext.EnterSelectionTarget(config, this.OnSelect, this.Skill.selectionType);
        }

        public bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) {
            bool hasAp = unitObject.Unit.GetCurrentIntStat(StatType.AP) >= this.Skill.apCost;
            bool hasMana = unitObject.Unit.GetCurrentIntStat(StatType.Mp) >= this.Skill.manaCost;
            return hasAp && hasMana;
        }

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            yield return this.ExecuteSkill(enemy, decisionResult.TargetPosition, battleMapManager);

            battleContext.ApCostApply(this);
        }

        public bool CanSelect(UnitObject target) {
            if (this.Skill.effects == null || this.Skill.effects.Count == 0) {
                return true;
            }

            return this.Skill.effects.Exists(effect => effect.CanApply(target));
        }

        private IEnumerator OnSelect(SelectionData data) {
            yield return this.ExecuteSkill(data.User, data.Position, data.BattleMapManager);
            data.Context.EndAction();
        }

        private IEnumerator ExecuteSkill(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            yield return user.PlaySkill(this.Skill, target);
            user.Unit.AddStat(StatType.Mp, -this.Skill.manaCost);
            UnitObject unitObjectTarget = battleMapManager.GetUnit(target);
            foreach (SkillEffect effect in this.Skill.effects.Where(effect => effect.CanApply(unitObjectTarget))) {
                if (!effect.CanApply(unitObjectTarget)) {
                    continue;
                }

                yield return effect.Apply(user, target, battleMapManager);
            }
        }
    }
}
