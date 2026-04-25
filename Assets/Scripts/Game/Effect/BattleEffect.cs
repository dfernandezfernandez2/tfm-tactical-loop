namespace Game.Effect {
    using System;
    using System.Collections;
    using Data;
    using Unit;
    using UnityEngine;

    public abstract class BattleEffect {
        private int _remainingTurns;
        protected Guid ID;

        protected BattleEffect(int duration) {
            this._remainingTurns = duration;
            this.ID = Guid.NewGuid();
        }

        public virtual IEnumerator OnApply(UnitObject from, UnitObject to, EffectVisualController controller) {
            yield return null;
        }

        public virtual IEnumerator OnTurnStart(UnitObject target, EffectVisualController controller) {
            yield return null;
        }

        public virtual IEnumerator OnTurnEnd(UnitObject target, EffectVisualController controller) {
            yield return null;
        }

        public virtual IEnumerator OnExpire(UnitObject target, EffectVisualController controller) {
            yield return null;
        }

        public void DecreaseDuration() => this._remainingTurns--;
        public bool IsExpired() => this._remainingTurns <= 0;

        protected virtual EffectData CreateEffectData(UnitObject target, Color color, string soundEffectName = null) =>
            new() {
                Effect = this,
                Target = target,
                Color = color,
                KeepActive = true,
                SoundEffect = this.CreateSoundEffectData(soundEffectName),
                ParticleConfig = this.CreateParticleEffectConfig()
            };

        protected virtual SoundEffectData CreateSoundEffectData(string soundEffectName) {
            if (soundEffectName == null) {
                return null;
            }

            return new SoundEffectData {
                Name = soundEffectName,
                Volume = 1f,
                WaitUntilFinished = true
            };
        }

        protected virtual ParticleEffectConfig CreateParticleEffectConfig() =>
            new() {
                heightOffset = 0.10f,

                startLifetime = 1.4f,
                startSpeed = 0.10f,
                startSize = 0.24f,

                maxParticles = 120,
                rateOverTime = 14f,
                burstCount = 36,

                shapeType = ParticleSystemShapeType.Circle,
                shapeRadius = 0.50f,
                shapeRadiusThickness = 0.08f,

                velocity = new Vector3(0f, 0.28f, 0f),

                alphaStart = 0.15f,
                alphaPeak = 0.9f,
                alphaPeakTime = 0.20f,
                alphaEnd = 0f,

                sizeStart = 0.45f,
                sizePeak = 1.6f,
                sizePeakTime = 0.40f,
                sizeEnd = 0f,

                textureType = EffectTextureType.SoftCircleTexture,
                size = 64
            };
    }
}
