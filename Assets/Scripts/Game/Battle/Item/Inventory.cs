namespace Game.Battle.Item {
    using System.Collections.Generic;
    using System.Linq;
    using Actions;

    public class Inventory {
        public List<InventoryItem> Items { get; } = new();

        public void Add(Item item) {
            InventoryItem existingItem =
                this.Items.FirstOrDefault(existingItem => existingItem.GetName() == item.itemName);
            if (existingItem != null) {
                existingItem.Add(1);
            }
            else {
                this.Items.Add(new InventoryItem(item, this.Remove));
            }
        }

        private void Remove(InventoryItem item) => this.Items.Remove(item);
        public bool HasItems() => this.Items.Count > 0;
    }
}
