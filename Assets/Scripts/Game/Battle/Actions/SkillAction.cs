namespace Game.Battle.Actions {
    using System.Collections;
    using System.Linq;
    using Battle;
    using IA;
    using Game.Map.Battle;
    using global::Unit.Data;
    using Unit;
    using Unit.Skills;
    using Unit.Skills.Effects;

    public class SkillAction : IBattleAction {

        private readonly Skill _skill;

        public SkillAction(Skill skill) => this._skill = skill;

        public string GetName() => this._skill.skillName;

        public string GetActionName() => ActionType.Skill.GetName() + "Action";

        public int GetApCost() => this._skill.apCost;

        public IEnumerator Start(IBattleContext battleContext) {
            yield return battleContext.EnterSelectionTarget(this._skill.target, this.OnSelect, this.CanSelect, this._skill.range);
        }

        public bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) {
            bool hasAp = unitObject.Unit.GetCurrentIntStat(StatType.AP) >= this._skill.apCost;
            bool hasMana = unitObject.Unit.GetCurrentIntStat(StatType.Mp) >= this._skill.manaCost;
            return hasAp && hasMana;
        }

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            yield return this.ExecuteSkill(enemy, decisionResult.TargetPosition, battleMapManager);

            battleContext.ApCostApply(this);
        }

        private bool CanSelect(UnitObject target) {
            if (this._skill.effects == null || this._skill.effects.Count == 0) {
                return true;
            }
            return this._skill.effects.Exists(effect => effect.CanApply(target));
        }

        private IEnumerator OnSelect(SelectionData data) {
            yield return this.ExecuteSkill(data.User, data.Position, data.BattleMapManager);
            data.Context.EndAction();
        }

        private IEnumerator ExecuteSkill(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            yield return user.PlaySkill(this._skill, target);
            user.Unit.AddStat(StatType.Mp, -this._skill.manaCost);
            UnitObject unitObjectTarget = battleMapManager.GetUnit(target);
            foreach (SkillEffect effect in this._skill.effects.Where(effect => effect.CanApply(unitObjectTarget))) {
                yield return effect.Apply(user, target, battleMapManager);
            }
        }

    }
}
