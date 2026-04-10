namespace Game.Unit {
    public class HealResult {
        private readonly int _amount;

        public HealResult(int amount) => this._amount = amount;

        public int GetAmount() => this._amount;
    }
}
