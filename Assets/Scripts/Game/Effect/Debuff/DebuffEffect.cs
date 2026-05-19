namespace Game.Effect.Debuff {
    using System.Collections;
    using Data;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    public class DebuffEffect : BattleEffect {
        private readonly float _amount;
        private readonly Color _color;
        private readonly string _soundEffectName;
        private readonly StatType _statType;

        public DebuffEffect(int duration, StatType statType, float amount, Color color,
            string soundEffectName = "debuff") :
            base(duration) {
            this._statType = statType;
            this._amount = amount;
            this._color = color;
            this._soundEffectName = soundEffectName;
        }

        public override IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            yield return controller.PlayEffect(this.CreateEffectData(to, this._color, this._soundEffectName));
            to.Unit.UnitStatsModifier.AddModifier(this._statType, -this._amount);
        }

        public override IEnumerator OnExpire(UnitObject target, EffectVisualController controller) {
            target.Unit.UnitStatsModifier.AddModifier(this._statType, this._amount);
            controller.RemoveEffect(this, target);
            yield return null;
        }

        protected override ParticleEffectConfig CreateParticleEffectConfig() =>
            new() {
                velocity = new Vector3(0f, -0.1f, 0f),
                textureType = EffectTextureType.DebufferTexture
            };
    }
}
