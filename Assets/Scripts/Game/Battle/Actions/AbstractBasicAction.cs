namespace Game.Battle.Actions {
    using System.Collections;
    using IA;
    using Map.Battle;
    using Unit;

    public abstract class AbstractBasicAction : IBattleAction {
        public string GetId() => this.GetActionType().GetName();

        public string GetName() => this.GetActionType().GetName();

        public abstract ActionType GetActionType();

        public abstract int GetApCost();

        public abstract void Start(IBattleContext battleContext);
        public bool CanDoAction(UnitObject unitObject) => unitObject.GetUnit().CanUseAp(this.GetApCost());

        public abstract IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager);
    }
}
