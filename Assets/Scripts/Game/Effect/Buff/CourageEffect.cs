namespace Game.Effect.Buff {
    using global::Unit.Data;

    public class CourageEffect : BuffEffect {

        public CourageEffect(int duration, float amount) : base(duration, StatType.Atk, amount) {
        }
    }
}
