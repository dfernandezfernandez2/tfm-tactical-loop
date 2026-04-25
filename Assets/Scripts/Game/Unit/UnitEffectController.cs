namespace Game.Unit {
    using System.Collections.Generic;
    using System.Linq;
    using Effect;
    using Effect.Status;
    using global::Unit.Data;

    public class UnitEffectController {
        private readonly List<BattleEffect> _effects = new();
        private readonly Dictionary<StatType, float> _modifiers = new();
        private StatusEffect _status;

        public void ApplyEffect(BattleEffect effect) {
            if (effect is StatusEffect statusEffect) {
                this.ReplaceStatus(statusEffect);
            }
            else {
                this._effects.Add(effect);
            }

            effect.OnApply(this);
        }

        private void ReplaceStatus(StatusEffect newStatus) {
            if (this._status != null) {
                this._effects.Remove(this._status);
            }

            this._status = newStatus;
            this._effects.Add(newStatus);
        }

        public void AddModifier(StatType statType, float value) {
            if (this._modifiers.TryAdd(statType, value)) {
                return;
            }

            float modifier = this._modifiers[statType];
            modifier += value;
            this._modifiers[statType] = modifier;
        }

        public float GetModifier(StatType statType) => this._modifiers.GetValueOrDefault(statType, 0f);

        public void OnTurnStart() {
            foreach (BattleEffect effect in this._effects.ToList()) {
                effect.OnTurnStart(this);
            }
        }

        public void OnTurnEnd() {
            foreach (BattleEffect effect in this._effects.ToList()) {
                effect.OnTurnEnd(this);
                effect.DecreaseDuration();
                if (!effect.IsExpired()) {
                    continue;
                }

                effect.OnExpire(this);
                this._effects.Remove(effect);
                if (this._status == effect) {
                    this._status = null;
                }
            }
        }
    }
}
