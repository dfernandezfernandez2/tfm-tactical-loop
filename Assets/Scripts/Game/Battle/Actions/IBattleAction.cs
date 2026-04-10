namespace Game.Battle.Actions {
    public interface IBattleAction {
        public ActionType GetActionType();
        public int GetApCost();
        public void Start(IBattleContext battleContext);
    }
}
