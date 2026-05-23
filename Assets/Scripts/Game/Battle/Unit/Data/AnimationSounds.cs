namespace Game.Battle.Unit.Data {
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class AnimationSounds {
        [SerializeField] private List<AnimationSound> sounds = new();

        public AnimationSound Get(AnimationType animationType) => this.sounds.Find(sound => sound.IsFromType(animationType));
    }
}
