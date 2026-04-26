namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class SkillSelectionAction : AbstractBasicAction {
        protected override ActionType GetActionType() => ActionType.Skill;
        public override int GetApCost() => 0;

        public override IEnumerator Start(IBattleContext battleContext) {
            battleContext.EnterSkillSelection();
            yield return null;
        }

        public override bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) =>
            unitObject.Actions.GetSkillActions().Count > 0 &&
            battleContext.IsAvailableAction(this.GetActionName() + "Action");

        public override IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager) => throw new NotImplementedException();
    }
}
