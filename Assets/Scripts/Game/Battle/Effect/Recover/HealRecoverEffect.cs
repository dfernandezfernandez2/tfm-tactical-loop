namespace Game.Battle.Effect.Recover {
    using UI;
    using Unit.Data;
    using UnityEngine;

    public class HealRecoverEffect : RecoverEffect {
        public HealRecoverEffect(float amount) : base(StatType.Hp, amount, Color.lightGreen, CombatTextType.Heal) {
        }
    }
}
