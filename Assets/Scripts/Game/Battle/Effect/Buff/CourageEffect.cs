namespace Game.Battle.Effect.Buff {
    using Unit.Data;
    using UnityEngine;

    public class CourageEffect : BuffEffect {
        public CourageEffect(int duration, float amount) : base(duration, StatType.Atk, amount, Color.red) {
        }
    }
}
