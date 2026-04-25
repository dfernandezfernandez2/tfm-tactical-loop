namespace Game.Effect.Debuff {
    using global::Unit.Data;
    using Unit;

    public class DebuffEffect : BattleEffect {

        private readonly StatType _statType;
        private readonly float _amount;

        public DebuffEffect(int duration, StatType statType, float amount) : base(duration) {
            this._statType = statType;
            this._amount = amount;
        }

        public override void OnApply(UnitEffectController target) => target.AddModifier(this._statType, -this._amount);

        public override void OnExpire(UnitEffectController target) => target.AddModifier(this._statType, this._amount);
    }
}
