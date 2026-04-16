namespace Game.Battle.Actions {
    using System.Collections;
    using System.Collections.Generic;
    using IA;
    using Map.Battle;
    using Unit;

    public class MovementAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Movement;
        public int GetApCost() => 1;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterMovementSelection();

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) {
            IReadOnlyList<GridPosition> path =
                battleMapManager.FindPath(enemy.GetUnit().GetGridPosition(), decisionResult.TargetPosition);
            yield return BattleSequenceExecutor.ExecuteMovement(enemy, path,
                (position, gridPosition) => battleMapManager.UnitMove(position, gridPosition, true));
        }
    }
}
