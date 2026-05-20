namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Item;
    using Map;
    using Map.Data;
    using Unit;

    public class InventoryItemAction : IBattleAction {
        private readonly Item _item;
        private readonly Action<InventoryItemAction> _onConsume;
        private int _amount;

        public InventoryItemAction(Item item, Action<InventoryItemAction> onConsume) {
            this._item = item;
            this._amount = 1;
            this._onConsume = onConsume;
        }

        public string GetName() => this._item.itemName;

        public string GetActionName() => ActionType.Item.GetName() + "Action";

        public int GetApCost() => 0;

        public IEnumerator Start(IBattleContext battleContext) {
            TileSearchConfig config = new() {
                CanEnterCheck = false,
                Target = this._item.target,
                CanSelect = this._item.effect.CanApply
            };
            yield return battleContext.EnterSelectionTarget(config, this.OnSelect);
        }

        public bool CanDoAction(IBattleContext battleContext, UnitObject unitObject) => true;

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) =>
            throw new NotImplementedException(); // at this point never could happen enemy uses object

        public int GetAmount() => this._amount;

        public void Add(int amount) => this._amount += amount;

        private void Consume() {
            this._amount = Math.Max(0, this._amount - 1);
            if (!this.Has()) {
                this._onConsume(this);
            }
        }

        private bool Has() => this._amount > 0;

        private IEnumerator OnSelect(SelectionData inventorySelectionData) {
            yield return this._item.effect.Apply(inventorySelectionData.User, inventorySelectionData.Position,
                inventorySelectionData.BattleMapManager);
            this.Consume();
            inventorySelectionData.Context.EndAction();
            yield return null;
        }
    }
}
