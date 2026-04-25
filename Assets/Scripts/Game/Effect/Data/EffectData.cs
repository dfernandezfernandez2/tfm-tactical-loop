namespace Game.Effect.Data {
    using Unit;
    using UnityEngine;

    public class EffectData {
        public BattleEffect Effect { get; set; }
        public UnitObject Target { get; set; }
        public Color Color { get; set; }
        public bool KeepActive { get; set; }
        public SoundEffectData SoundEffect { get; set; }
        public ParticleEffectConfig ParticleConfig { get; set; } = new();
    }
}
