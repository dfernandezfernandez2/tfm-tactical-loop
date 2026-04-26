namespace Game.Effect.Buff {
    using global::Unit.Data;
    using UnityEngine;

    public class CourageEffect : BuffEffect {
        public CourageEffect(int duration, float amount) : base(duration, StatType.Atk, amount, Color.red) {
        }
    }
}
