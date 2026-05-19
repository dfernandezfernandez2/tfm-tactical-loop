namespace Game.Effect.Status {
    using System.Collections;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    public class SlowEffect : StatusEffect {
        private readonly int _amount;

        public SlowEffect(int duration, int amount) : base(duration) => this._amount = amount;

        public override IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            to.Unit.UnitStatsModifier.AddModifier(StatType.Movement, -this._amount);
            yield return controller.PlayEffect(this.CreateEffectData(to, Color.saddleBrown));
        }

        public override IEnumerator OnExpire(UnitObject target, EffectVisualController controller) {
            target.Unit.UnitStatsModifier.AddModifier(StatType.Movement, this._amount);
            controller.RemoveEffect(this, target);
            yield return null;
        }
    }
}
