namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class ItemAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Item;
        public int GetApCost() => 0;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterObjectSelection();

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) => throw new NotImplementedException();
    }
}
