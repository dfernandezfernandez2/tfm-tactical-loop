namespace Game.Battle.Effect.Buff {
    using Unit.Data;
    using UnityEngine;

    public class HardenEffect : BuffEffect {
        public HardenEffect(int duration, float amount) : base(duration, StatType.Def, amount, Color.blue) {
        }
    }
}
