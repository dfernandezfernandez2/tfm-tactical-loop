namespace Game.Effect.Buff {
    using global::Unit.Data;

    public class HardenEffect : BuffEffect {

        public HardenEffect(int duration, float amount) : base(duration, StatType.Def, amount) {
        }
    }
}
