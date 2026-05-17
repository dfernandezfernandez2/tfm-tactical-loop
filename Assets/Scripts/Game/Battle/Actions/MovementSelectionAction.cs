namespace Game.Battle.Actions {
    using System.Collections;
    using System.Collections.Generic;
    using IA;
    using Map.Battle;
    using Map.Battle.Data;
    using Unit;

    public class MovementSelectionAction : AbstractBasicAction {
        protected override ActionType GetActionType() => ActionType.Movement;
        public override int GetApCost() => 1;

        public override IEnumerator Start(IBattleContext battleContext) {
            battleContext.EnterMovementSelection();
            yield return null;
        }

        public override IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            IReadOnlyList<GridPosition> path =
                battleMapManager.FindPath(enemy.Unit.GridPosition, decisionResult.TargetPosition);
            yield return BattleSequenceExecutor.ExecuteMovement(enemy, path,
                (position, gridPosition) => battleMapManager.UnitMove(position, gridPosition, true));
        }
    }
}
