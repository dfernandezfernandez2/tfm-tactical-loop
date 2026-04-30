namespace Game.Core.Data {
    using System.Collections.Generic;
    using Battle.Item;
    using Passive;

    public class RunData {
        private static RunData _instance;

        public readonly List<IPassive> Passives = new();

        private RunData() => this.Inventory = new Inventory();
        public Team Team { get; set; }
        public Inventory Inventory { get; private set; }

        public static RunData GetInstance() {
            _instance ??= new RunData();
            return _instance;
        }

        public void AddPassive(IPassive passive) {
            if (this.HasPassive(passive)) {
                return;
            }

            this.Passives.Add(passive);
        }

        public bool HasPassive(IPassive passive) =>
            this.Passives.Contains(passive);
    }
}
