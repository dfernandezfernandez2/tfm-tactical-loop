namespace Game.Battle.Effect.Status {
    using System.Collections;
    using Audio;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    public class PoisonEffect : StatusEffect {
        private readonly int _damagePerTurn;
        private readonly string _soundOnApply;
        private readonly string _soundOnStart;
        private int _currentDamage;

        public PoisonEffect(int duration, int initialDamage, int damagePerTurn, string soundOnApply = "effect",
            string soundOnStart = "poison_turn") : base(duration) {
            this._damagePerTurn = damagePerTurn;
            this._currentDamage = initialDamage;
            this._soundOnApply = soundOnApply;
            this._soundOnStart = soundOnStart;
        }

        public override IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            yield return controller.PlayEffect(this.CreateEffectData(to, Color.purple, this._soundOnApply, 0.25f));
        }

        public override IEnumerator OnTurnStart(UnitObject target, EffectVisualController controller) {
            target.Unit.UnitStatsModifier.AddModifier(StatType.Hp, -this._currentDamage);
            yield return AudioManager.Instance.PlaySound(this._soundOnStart);
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
