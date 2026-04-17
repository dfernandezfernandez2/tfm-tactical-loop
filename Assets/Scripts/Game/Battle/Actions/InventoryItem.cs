namespace Game.Battle.Actions {
    using System;
    using System.Collections;
    using IA;
    using Game.Map.Battle;
    using Unit;
    using Item;

    public class InventoryItem: IBattleAction {
        private readonly Item _item;
        private int _amount;

        public InventoryItem(Item item) {
            this._item = item;
            this._amount = 0;
        }

        public void Add(int amount) => this._amount += amount;
        private void Consume() => this._amount = Math.Max(0, this._amount - 1);
        private bool Has() => this._amount > 0;

        public string GetId() => this._item.itemName; // todo:unique id

        public string GetName() => this._item.itemName;

        public ActionType GetActionType() => ActionType.Item;

        public int GetApCost() => 0;

        public void Start(IBattleContext battleContext) => throw new NotImplementedException();

        public bool CanDoAction(UnitObject unitObject) => this.Has();

        public IEnumerator DoEnemyAction(IBattleContext battleContext, UnitObject enemy, DecisionResult decisionResult,
            BattleMapManager battleMapManager) =>
            throw new NotImplementedException(); // at this point neve could happen enemy uses object
        }
}
