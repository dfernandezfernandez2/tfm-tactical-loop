namespace Game.Battle.Actions {
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public interface IBattleAction {
        public string GetName();
        public string GetActionName();
        public int GetApCost();
        public void Start(IBattleContext battleContext);
        public bool CanDoAction(IBattleContext battleContext, UnitObject unitObject);

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager);
    }
}
