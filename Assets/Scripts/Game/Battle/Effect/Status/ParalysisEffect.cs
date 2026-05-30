namespace Game.Battle.Effect.Status {
    using System.Collections;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    public class ParalysisEffect : StatusEffect {
        private readonly int _amount;
        private readonly string _soundOnApply;

        public ParalysisEffect(int duration, int amount, string soundOnApply = "effect") : base(duration) {
            this._amount = amount;
            this._soundOnApply = soundOnApply;
        }

        public override IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            to.Unit.UnitStatsModifier.AddModifier(StatType.AP, -this._amount);
            yield return controller.PlayEffect(this.CreateEffectData(to, Color.yellow, this._soundOnApply, 0.25f));
        }

        public override IEnumerator OnExpire(UnitObject target, EffectVisualController controller) {
            target.Unit.UnitStatsModifier.AddModifier(StatType.AP, this._amount);
            controller.RemoveEffect(this, target);
            yield return null;
        }
    }
}
