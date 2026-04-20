namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Item;
    using Map.Battle;
    using Unit;

    public class InventoryItem : IBattleAction {
        private readonly Item _item;
        private readonly Action<InventoryItem> _onConsume;
        private int _amount;

        public InventoryItem(Item item, Action<InventoryItem> onConsume) {
            this._item = item;
            this._amount = 1;
            this._onConsume = onConsume;
        }

        public string GetName() => this._item.itemName;

        public string GetActionName() => ActionType.Item.GetName() + "Action";

        public int GetApCost() => 0;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterItemSelectionTarget(this._item.target, this.OnSelect, this._item.effect.CanApply);

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

        private void OnSelect(UnitObject user, GridPosition position, BattleMapManager battleMapManager,
            IBattleContext context) {
            this._item.effect.Apply(user, position, battleMapManager);
            this.Consume();
            context.EndAction();
        }
    }
}
