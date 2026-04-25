namespace Game.Effect {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Data;
    using Unit;
    using UnityEngine;

    internal struct ActiveEffectData : IEquatable<ActiveEffectData> {
        public BattleEffect Effect;
        public ParticleSystem ParticleSystem;

        public bool Equals(ActiveEffectData other) => Equals(this.Effect, other.Effect);

        public override bool Equals(object obj) => obj is ActiveEffectData other && this.Equals(other);

        public override int GetHashCode() => this.Effect != null ? this.Effect.GetHashCode() : 0;
    }

    [RequireComponent(typeof(AudioSource))]
    public class EffectVisualController : MonoBehaviour {
        [SerializeField] private ParticleSystem particleSystemPrefab;
        private readonly Dictionary<UnitObject, List<ActiveEffectData>> _activeEffects = new();
        private readonly Dictionary<string, AudioClip> _soundCache = new();

        private AudioSource _audioSource;

        public void Awake() => this._audioSource = this.GetComponent<AudioSource>();

        public IEnumerator PlayEffect(EffectData effectData) {
            ParticleEffectConfig config = effectData.ParticleConfig ?? new ParticleEffectConfig();

            ParticleSystem ps = Instantiate(this.particleSystemPrefab, this.transform);

            ParticleSystemRenderer particleSystemRenderer = ps.GetComponent<ParticleSystemRenderer>();
            if (particleSystemRenderer != null) {
                Material material = new(particleSystemRenderer.material) {
                    mainTexture = EffectTextureFactory.GetTexture(config.textureType, config.size)
                };
                particleSystemRenderer.material = material;
            }

            ps.gameObject.transform.position =
                effectData.Target.transform.position + (Vector3.up * config.heightOffset);

            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ParticleSystem.MainModule main = ps.main;
            main.playOnAwake = false;
            main.startColor = new ParticleSystem.MinMaxGradient(effectData.Color);
            main.startLifetime = config.startLifetime;
            main.startSpeed = config.startSpeed;
            main.startSize = config.startSize;
            main.duration = effectData.KeepActive ? 999f : config.duration;
            main.loop = effectData.KeepActive;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.maxParticles = config.maxParticles;

            ParticleSystem.EmissionModule emission = ps.emission;
            emission.enabled = true;
            emission.rateOverTime = effectData.KeepActive ? config.rateOverTime : 0f;
            emission.SetBursts(effectData.KeepActive
                ? Array.Empty<ParticleSystem.Burst>()
                : new[] { new ParticleSystem.Burst(0f, config.burstCount) });

            ParticleSystem.ShapeModule shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = config.shapeType;
            shape.radius = config.shapeRadius;
            shape.radiusThickness = config.shapeRadiusThickness;

            ParticleSystem.VelocityOverLifetimeModule velocity = ps.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            velocity.x = new ParticleSystem.MinMaxCurve(config.velocity.x);
            velocity.y = new ParticleSystem.MinMaxCurve(config.velocity.y);
            velocity.z = new ParticleSystem.MinMaxCurve(config.velocity.z);

            Gradient gradient = new();
            gradient.SetKeys(
                new[] {
                    new GradientColorKey(effectData.Color, 0f),
                    new GradientColorKey(effectData.Color, 1f)
                },
                new[] {
                    new GradientAlphaKey(config.alphaStart, 0f),
                    new GradientAlphaKey(config.alphaPeak, config.alphaPeakTime),
                    new GradientAlphaKey(config.alphaEnd, 1f)
                }
            );

            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = ps.colorOverLifetime;
            colorOverLifetime.enabled = true;
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = ps.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(
                    new Keyframe(0f, config.sizeStart),
                    new Keyframe(config.sizePeakTime, config.sizePeak),
                    new Keyframe(1f, config.sizeEnd)
                )
            );

            ps.Play();

            if (effectData.SoundEffect != null) {
                yield return this.PlaySound(effectData.SoundEffect);
            }

            if (effectData.KeepActive) {
                if (!this._activeEffects.ContainsKey(effectData.Target)) {
                    this._activeEffects[effectData.Target] = new List<ActiveEffectData>();
                }

                this._activeEffects[effectData.Target].Add(new ActiveEffectData {
                    Effect = effectData.Effect,
                    ParticleSystem = ps
                });
                yield break;
            }

            yield return new WaitUntil(() => !ps.IsAlive(true));
            Destroy(ps.gameObject);
        }

        public void RemoveEffect(BattleEffect effect, UnitObject target) {
            ActiveEffectData effectData =
                this._activeEffects[target].Find(activeEffect => activeEffect.Effect.Equals(effect));
            Destroy(effectData.ParticleSystem);
            this._activeEffects[target].Remove(effectData);
        }

        public IEnumerator PlaySound(SoundEffectData soundEffectData) {
            AudioClip clip = this.GetSound(soundEffectData.Name);
            if (!clip) {
                yield break;
            }

            this._audioSource.PlayOneShot(clip, soundEffectData.Volume);
            if (!soundEffectData.WaitUntilFinished) {
                yield break;
            }

            yield return new WaitForSeconds(clip.length);
        }

        private AudioClip GetSound(string soundName) {
            if (string.IsNullOrEmpty(soundName)) {
                return null;
            }

            if (this._soundCache.TryGetValue(soundName, out AudioClip clip)) {
                return clip;
            }

            clip = Resources.Load<AudioClip>($"Sounds/{soundName}");
            if (!clip) {
                return null;
            }

            this._soundCache[soundName] = clip;
            return clip;
        }
    }
}
