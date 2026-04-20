namespace Game.Core.Data {
    using Battle.Item;

    public class RunData {
        private static RunData _instance;

        private RunData() => this.Inventory = new Inventory();

        public Team Team { get; set; }
        public Inventory Inventory { get; private set; }

        public static RunData GetInstance() {
            _instance ??= new RunData();
            return _instance;
        }
    }
}
