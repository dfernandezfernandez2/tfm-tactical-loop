namespace Game.Effect.Status {
    using System.Collections;
    using global::Unit.Data;
    using Unit;
    using UnityEngine;

    public class PoisonEffect : StatusEffect {
        private readonly int _damagePerTurn;
        private int _currentDamage;

        public PoisonEffect(int duration, int initialDamage, int damagePerTurn) : base(duration) {
            this._damagePerTurn = damagePerTurn;
            this._currentDamage = initialDamage;
        }

        public override IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            yield return controller.PlayEffect(this.CreateEffectData(to, Color.mediumPurple));
        }

        public override IEnumerator OnTurnStart(UnitObject target, EffectVisualController controller) {
            target.Unit.UnitStatsModifier.AddModifier(StatType.Hp, -this._currentDamage);
            yield return target.PlayDamage(this._currentDamage);
            this._currentDamage += this._damagePerTurn;
            yield return null;
        }

        public override IEnumerator OnExpire(UnitObject target, EffectVisualController controller) {
            controller.RemoveEffect(this, target);
            yield return null;
        }
    }
}
