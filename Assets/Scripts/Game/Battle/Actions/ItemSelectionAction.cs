namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Map;
    using Run.Data;
    using Unit;

    public class ItemSelectionAction : AbstractBasicAction {
        protected override ActionType GetActionType() => ActionType.Item;
        public override int GetApCost() => 0;

        public override IEnumerator Start(IBattleContext battleContext) {
            battleContext.EnterObjectSelection();
            yield return null;
        }

        public override bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) =>
            RunData.GetInstance().Inventory.HasItems() &&
            battleContext.IsAvailableAction(this.GetActionName() + "Action");

        public override IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy,
            DecisionResult decisionResult,
            BattleMapManager battleMapManager) => throw new NotImplementedException();
    }
}
