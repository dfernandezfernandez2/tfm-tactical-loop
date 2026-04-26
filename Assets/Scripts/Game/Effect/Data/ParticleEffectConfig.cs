namespace Game.Effect.Data {
    using System;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable]
    public class ParticleEffectConfig {
        [FormerlySerializedAs("HeightOffset")] public float heightOffset = 0.25f;

        [FormerlySerializedAs("StartLifetime")]
        public float startLifetime = 0.8f;

        [FormerlySerializedAs("StartSpeed")] public float startSpeed = 0.25f;
        [FormerlySerializedAs("StartSize")] public float startSize = 0.12f;
        [FormerlySerializedAs("Duration")] public float duration = 0.75f;
        [FormerlySerializedAs("MaxParticles")] public int maxParticles = 80;

        [FormerlySerializedAs("RateOverTime")] public float rateOverTime = 8f;
        [FormerlySerializedAs("BurstCount")] public short burstCount = 24;

        [FormerlySerializedAs("ShapeType")] public ParticleSystemShapeType shapeType = ParticleSystemShapeType.Circle;
        [FormerlySerializedAs("ShapeRadius")] public float shapeRadius = 0.45f;

        [FormerlySerializedAs("ShapeRadiusThickness")]
        public float shapeRadiusThickness = 0.15f;

        [FormerlySerializedAs("Velocity")] public Vector3 velocity = new(0f, 0.65f, 0f);

        [FormerlySerializedAs("AlphaStart")] public float alphaStart;
        [FormerlySerializedAs("AlphaPeak")] public float alphaPeak = 1f;

        [FormerlySerializedAs("AlphaPeakTime")]
        public float alphaPeakTime = 0.15f;

        [FormerlySerializedAs("AlphaEnd")] public float alphaEnd;

        [FormerlySerializedAs("SizeStart")] public float sizeStart = 0.4f;
        [FormerlySerializedAs("SizePeak")] public float sizePeak = 1.2f;
        [FormerlySerializedAs("SizePeakTime")] public float sizePeakTime = 0.35f;
        [FormerlySerializedAs("SizeEnd")] public float sizeEnd;

        [FormerlySerializedAs("TextureType")]
        public EffectTextureType textureType = EffectTextureType.SoftCircleTexture;

        [FormerlySerializedAs("Size")] public int size = 32;
    }
}
