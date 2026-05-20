namespace Game.Run {
    using System.Collections.Generic;
    using System.Linq;
    using Battle.Actions;
    using Battle.Item;

    public class Inventory {
        public List<InventoryItemAction> Items { get; } = new();

        public void Add(Item item) {
            InventoryItemAction existingItemAction =
                this.Items.FirstOrDefault(existingItem => existingItem.GetName() == item.itemName);
            if (existingItemAction != null) {
                existingItemAction.Add(1);
            }
            else {
                this.Items.Add(new InventoryItemAction(item, this.Remove));
            }
        }

        private void Remove(InventoryItemAction itemAction) => this.Items.Remove(itemAction);
        public bool HasItems() => this.Items.Count > 0;
    }
}
