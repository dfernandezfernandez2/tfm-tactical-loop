namespace Game.Effect.Recover {
    using System.Collections;
    using Battle.UI;
    using Data;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    public class RecoverEffect : BattleEffect {
        private readonly float _amount;
        private readonly Color _color;
        private readonly CombatTextType _combatTextType;
        private readonly string _soundEffectName;
        private readonly StatType _statType;

        public RecoverEffect(StatType statType, float amount, Color color, CombatTextType combatTextType,
            string soundEffectName = "heal") :
            base(0) {
            this._statType = statType;
            this._amount = amount;
            this._color = color;
            this._combatTextType = combatTextType;
            this._soundEffectName = soundEffectName;
        }

        public override IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            to.PlayText(this._amount.ToString(), this._combatTextType);
            yield return controller.PlayEffect(this.CreateEffectData(to, this._color, this._soundEffectName));
            to.Unit.UnitStatsModifier.AddModifier(this._statType, this._amount);
        }

        public override IEnumerator OnExpire(UnitObject target, EffectVisualController controller) {
            yield return null;
        }

        protected override EffectData CreateEffectData(UnitObject target, Color color, string soundEffectName = null) =>
            new() {
                Effect = this,
                Target = target,
                Color = color,
                KeepActive = false,
                SoundEffect = this.CreateSoundEffectData(soundEffectName),
                ParticleConfig = this.CreateParticleEffectConfig()
            };
    }
}
