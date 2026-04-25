namespace Game.Battle.Actions {
    using System.Collections;
    using global::Unit.Data;
    using IA;
    using Map.Battle;
    using Unit;

    public abstract class AbstractBasicAction : IBattleAction {
        public string GetName() => this.GetActionType().GetName();

        public string GetActionName() => this.GetName();

        public abstract int GetApCost();

        public abstract void Start(IBattleContext battleContext);

        public virtual bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) =>
            unitObject.GetUnit().GetCurrentIntStat(StatType.AP) >= this.GetApCost();

        public abstract IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager);

        public abstract ActionType GetActionType();
    }
}
