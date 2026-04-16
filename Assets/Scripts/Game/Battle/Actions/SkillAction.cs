namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class SkillAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Skill;
        public int GetApCost() => 0;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterSkillSelection();

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) => throw new NotImplementedException();
    }
}
