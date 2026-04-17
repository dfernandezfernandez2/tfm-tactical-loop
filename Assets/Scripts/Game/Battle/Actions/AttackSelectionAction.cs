namespace Game.Battle.Actions {
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public class AttackSelectionAction : AbstractBasicAction {
        public override ActionType GetActionType() => ActionType.Attack;
        public override int GetApCost() => 1;

        public override void Start(IBattleContext battleContext) =>
            battleContext.EnterAttackTargetSelection();

        public override IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            UnitObject target = battleMapManager.GetUnit(decisionResult.TargetPosition);
            yield return BattleSequenceExecutor.ExecuteBasicAttack(enemy, target, decisionResult.TargetPosition,
                battleMapManager);
        }
    }
}
