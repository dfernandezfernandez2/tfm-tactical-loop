namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using Core.Data;
    using IA;
    using Map.Battle;
    using Unit;

    public class ItemSelectionAction : AbstractBasicAction {
        public override ActionType GetActionType() => ActionType.Item;
        public override int GetApCost() => 0;

        public override void Start(IBattleContext battleContext) =>
            battleContext.EnterObjectSelection();

        public override bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) =>
            RunData.GetInstance().Inventory.HasItems() &&
            battleContext.IsAvailableAction(this.GetActionName() + "Action");

        public override IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager) => throw new NotImplementedException();
    }
}
