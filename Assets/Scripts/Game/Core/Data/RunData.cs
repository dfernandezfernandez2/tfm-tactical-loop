namespace Game.Core.Data {
    public class RunData {
        private static RunData _instance;

        public Team Team { get; set; }

        public static RunData GetInstance() {
            _instance ??= new RunData();
            return _instance;
        }
    }
}
