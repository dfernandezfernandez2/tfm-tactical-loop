namespace Game.Battle.Actions {
    public class ItemAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Item;
        public int GetApCost() => 0;

        public void Start(UnitTurnState unitTurnState, IBattleContext battleContext) =>
            battleContext.EnterObjectSelection();
    }
}
