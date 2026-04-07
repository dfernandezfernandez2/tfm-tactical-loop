namespace Game.Battle.Actions {
    public interface IBattleAction {
        public ActionType GetActionType();
        public int GetApCost();
        public bool CanBeExecuted(UnitTurnState unitTurnState) => unitTurnState.CanDoAction(this);
        public void Start(UnitTurnState unitTurnState, IBattleContext battleContext);
    }
}
