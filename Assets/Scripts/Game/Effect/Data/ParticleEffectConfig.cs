namespace Game.Effect.Data {
    using System;
    using Effect;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable]
    public class ParticleEffectConfig {
        [FormerlySerializedAs("HeightOffset")] public float heightOffset = 0.2f;

        [FormerlySerializedAs("StartLifetime")]
        public float startLifetime = 0.65f;

        [FormerlySerializedAs("StartSpeed")] public float startSpeed = 0.05f;

        [FormerlySerializedAs("StartSize")] public float startSize = 0.15f;

        [FormerlySerializedAs("Duration")] public float duration = 0.6f;

        [FormerlySerializedAs("MaxParticles")] public int maxParticles = 300;

        [FormerlySerializedAs("RateOverTime")] public float rateOverTime = 20f;

        [FormerlySerializedAs("BurstCount")] public short burstCount = 56;

        [FormerlySerializedAs("ShapeType")] public ParticleSystemShapeType shapeType = ParticleSystemShapeType.Box;

        [FormerlySerializedAs("ShapeRadius")] public float shapeRadius = 0.2f;

        [FormerlySerializedAs("ShapeScale")] public Vector3 shapeScale = new(0.75f, 1f, 1f);

        [FormerlySerializedAs("ShapeRadiusThickness")]
        public float shapeRadiusThickness = 0.1f;

        [FormerlySerializedAs("Velocity")] public Vector3 velocity = new(0f, 0.1f, 0f);

        [FormerlySerializedAs("AlphaStart")] public float alphaStart;

        [FormerlySerializedAs("AlphaPeak")] public float alphaPeak = 1f;

        [FormerlySerializedAs("AlphaPeakTime")]
        public float alphaPeakTime = 0.12f;

        [FormerlySerializedAs("AlphaEnd")] public float alphaEnd;

        [FormerlySerializedAs("SizeStart")] public float sizeStart = 0.6f;

        [FormerlySerializedAs("SizePeak")] public float sizePeak = 1.6f;

        [FormerlySerializedAs("SizePeakTime")] public float sizePeakTime = 0.3f;

        [FormerlySerializedAs("SizeEnd")] public float sizeEnd;

        [FormerlySerializedAs("TextureType")]
        public EffectTextureType textureType = EffectTextureType.CircleTexture;
    }
}
