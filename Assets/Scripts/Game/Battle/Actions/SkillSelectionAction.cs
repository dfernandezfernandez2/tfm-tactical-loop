namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class SkillSelectionAction : AbstractBasicAction {
        public override ActionType GetActionType() => ActionType.Skill;
        public override int GetApCost() => 0;

        public override void Start(IBattleContext battleContext) =>
            battleContext.EnterSkillSelection();

        public override bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) =>
            unitObject.GetSkillActions().Count > 0 && battleContext.IsAvailableAction(this.GetActionName() + "Action");

        public override IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager) => throw new NotImplementedException();
    }
}
