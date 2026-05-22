namespace Game.Battle.Effect.Recover {
    using UI;
    using Unit.Data;
    using UnityEngine;

    public class ExecutionEffect : RecoverEffect {
        public ExecutionEffect(float amount) : base(StatType.Hp, -amount, Color.darkRed, CombatTextType.Hit,
            "execution") {
        }
    }
}
