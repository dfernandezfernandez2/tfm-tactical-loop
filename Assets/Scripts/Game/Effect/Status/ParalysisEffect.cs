namespace Game.Effect.Status {
    using System.Collections;
    using global::Unit.Data;
    using Unit;
    using UnityEngine;

    public class ParalysisEffect : StatusEffect {
        private readonly int _amount;

        public ParalysisEffect(int duration, int amount) : base(duration) => this._amount = amount;

        public override IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            to.Unit.UnitStatsModifier.AddModifier(StatType.AP, -this._amount);
            yield return controller.PlayEffect(this.CreateEffectData(to, Color.yellow));
        }

        public override IEnumerator OnExpire(UnitObject target, EffectVisualController controller) {
            target.Unit.UnitStatsModifier.AddModifier(StatType.AP, this._amount);
            controller.RemoveEffect(this, target);
            yield return null;
        }
    }
}
