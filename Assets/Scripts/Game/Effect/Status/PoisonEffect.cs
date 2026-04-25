namespace Game.Effect.Status {
    using global::Unit.Data;
    using Unit;

    public class PoisonEffect : StatusEffect {
        private readonly int _damagePerTurn;
        private int _currentDamage;

        public PoisonEffect(int duration, int initialDamage, int damagePerTurn) : base(duration) {
            this._damagePerTurn = damagePerTurn;
            this._currentDamage = initialDamage;
        }

        public override void OnTurnStart(UnitEffectController target) {
            target.AddModifier(StatType.Hp, -this._currentDamage);
            this._currentDamage += this._damagePerTurn;
        }
    }
}
