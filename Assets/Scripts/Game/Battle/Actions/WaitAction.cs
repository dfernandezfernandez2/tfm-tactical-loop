namespace Game.Battle.Actions {
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class WaitAction : AbstractBasicAction {
        public override ActionType GetActionType() => ActionType.Wait;
        public override int GetApCost() => 0;
        public override void Start(IBattleContext battleContext) => battleContext.EndTurn();

        public override IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            battleContext.EndTurn();
            yield return null;
        }
    }
}
