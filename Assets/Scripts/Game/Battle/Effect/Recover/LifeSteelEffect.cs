namespace Game.Battle.Effect.Recover {
    using UI;
    using Unit.Data;
    using UnityEngine;

    public class LifeSteelEffect : RecoverEffect {
        public LifeSteelEffect(float amount) : base(StatType.Hp, amount, Color.orange, CombatTextType.Heal) {
        }
    }
}
