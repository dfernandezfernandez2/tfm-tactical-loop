namespace Game.Battle.Actions {
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class WaitAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Wait;
        public int GetApCost() => 0;
        public void Start(IBattleContext battleContext) => battleContext.EndTurn();

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            battleContext.EndTurn();
            yield return null;
        }
    }
}
