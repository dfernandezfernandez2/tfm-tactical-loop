namespace Game.Battle.Unit.Data {
    using System;
    using System.Collections;
    using Audio;
    using UnityEngine;

    [Serializable]
    public class AnimationSound {
        [SerializeField] private AnimationType animationType;
        [SerializeField] private string soundName;
        [SerializeField] private float volume = 1f;

        public bool IsFromType(AnimationType type) => this.animationType == type;

        public IEnumerator Play(bool wait = false) {
            yield return AudioManager.Instance.PlaySound(this.soundName, this.volume, wait);
        }
    }
}
