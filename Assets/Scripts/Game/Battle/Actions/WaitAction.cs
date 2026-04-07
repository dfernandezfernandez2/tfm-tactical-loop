namespace Game.Battle.Actions {
    public class WaitAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Wait;
        public int GetApCost() => 0;
        public void Start(UnitTurnState unitTurnState, IBattleContext battleContext) => battleContext.EndTurn();
    }
}
