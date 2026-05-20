namespace Game.Battle.Unit {
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Effect;
    using Effect.Status;
    using UnityEngine;

    public class UnitEffectController : MonoBehaviour {
        private readonly List<BattleEffect> _effects = new();
        private StatusEffect _status;
        private UnitObject _unitObject;

        public EffectVisualController VisualController { get; private set; }

        public void Awake() => this.VisualController = FindFirstObjectByType<EffectVisualController>();

        public void Init(UnitObject unitObject) => this._unitObject = unitObject;

        public IEnumerator ApplyEffect(BattleEffect effect) {
            yield return this.ApplyEffect(effect, this._unitObject);
        }

        public IEnumerator ApplyEffect(BattleEffect effect, UnitObject target) {
            if (effect is StatusEffect statusEffect) {
                yield return this.ReplaceStatus(statusEffect);
            }
            else {
                this._effects.Add(effect);
            }

            yield return effect.OnApply(this._unitObject, target, this.VisualController);
        }

        private IEnumerator ReplaceStatus(StatusEffect newStatus) {
            if (this._status != null) {
                yield return this._status.OnExpire(this._unitObject, this.VisualController);
                this._effects.Remove(this._status);
            }

            this._status = newStatus;
            this._effects.Add(newStatus);
        }

        public IEnumerator OnTurnStart() {
            foreach (BattleEffect effect in this._effects.ToList()) {
                yield return effect.OnTurnStart(this._unitObject, this.VisualController);
                effect.DecreaseDuration();
                if (!effect.IsExpired()) {
                    continue;
                }

                yield return effect.OnExpire(this._unitObject, this.VisualController);
                this._effects.Remove(effect);
                if (this._status == effect) {
                    this._status = null;
                }
            }
        }

        public IEnumerator OnTurnEnd() {
            foreach (BattleEffect effect in this._effects.ToList()) {
                yield return effect.OnTurnEnd(this._unitObject, this.VisualController);
            }
        }
    }
}
