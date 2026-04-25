namespace Game.Effect.Status {
    using global::Unit.Data;
    using Unit;

    public class SlowEffect : StatusEffect {
        private readonly int _amount;

        public SlowEffect(int duration, int amount) : base(duration) => this._amount = amount;

        public override void OnApply(UnitEffectController target) =>
            target.AddModifier(StatType.Movement, -this._amount);

        public override void OnExpire(UnitEffectController target) =>
            target.AddModifier(StatType.Movement, this._amount);
    }
}
