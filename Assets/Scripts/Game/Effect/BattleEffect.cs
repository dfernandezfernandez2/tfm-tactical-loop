namespace Game.Effect {
    using Unit;

    public abstract class BattleEffect {
        private int _remainingTurns;

        protected BattleEffect(int duration) => this._remainingTurns = duration;

        public virtual void OnApply(UnitEffectController target) {
        }

        public virtual void OnTurnStart(UnitEffectController target) {
        }

        public virtual void OnTurnEnd(UnitEffectController target) {
        }

        public virtual void OnExpire(UnitEffectController target) {
        }

        public void DecreaseDuration() => this._remainingTurns--;
        public bool IsExpired() => this._remainingTurns <= 0;
    }
}
