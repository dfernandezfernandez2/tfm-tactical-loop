namespace Game.Battle.Actions {
    public class MovementAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Movement;
        public int GetApCost() => 1;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterMovementSelection();
    }
}
