namespace Game.Battle.Item {
    using System.Collections.Generic;
    using Actions;

    public class Inventory {
        public List<InventoryItem> Items { get; } = new();

        public void Add(InventoryItem item) => this.Items.Add(item);
        public void Remove(InventoryItem item) => this.Items.Remove(item);
        public bool HasItems() => this.Items.Count > 0;
    }
}
