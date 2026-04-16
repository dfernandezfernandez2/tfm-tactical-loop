namespace Game.Battle.Actions {
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class AttackAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Attack;
        public int GetApCost() => 1;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterAttackTargetSelection();

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            UnitObject target = battleMapManager.GetUnit(decisionResult.TargetPosition);
            yield return BattleSequenceExecutor.ExecuteBasicAttack(enemy, target, decisionResult.TargetPosition);
        }
    }
}
