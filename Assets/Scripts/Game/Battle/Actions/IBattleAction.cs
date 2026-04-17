namespace Game.Battle.Actions {
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public interface IBattleAction {
        public string GetId();
        public string GetName();
        public ActionType GetActionType();
        public int GetApCost();
        public void Start(IBattleContext battleContext);
        public bool CanDoAction(UnitObject unitObject);

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager);
    }
}
