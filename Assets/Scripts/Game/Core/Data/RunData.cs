namespace Game.Core.Data {
    using Battle.Item;

    public class RunData {
        private static RunData _instance;

        public Team Team { get; set; }
        public Inventory Inventory { get; private set; }

        private RunData() => this.Inventory = new Inventory();

        public static RunData GetInstance() {
            _instance ??= new RunData();
            return _instance;
        }
    }
}
