namespace Game.Core {
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioManager : MonoBehaviour {
        private const string _musicKey = "MusicVolume";
        private const string _effectsKey = "EffectsVolume";
        private const string _muteKey = "FullMute";

        [SerializeField] private AudioMixer audioMixer;
        public static AudioManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null) {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            this.ApplySavedSettings();
        }

        private void ApplySavedSettings() {
            if (GetMute()) {
                this.audioMixer.SetFloat("MusicVolume", -80f);
                this.audioMixer.SetFloat("EffectsVolume", -80f);
                return;
            }

            this.SetMusicVolume(GetCurrentMusicVolume());
            this.SetEffectsVolume(GetCurrentEffectsVolume());
        }

        public void SetMusicVolume(float value) {
            if (GetMute()) {
                this.audioMixer.SetFloat("MusicVolume", -80f);
            }
            else {
                this.audioMixer.SetFloat("MusicVolume", VolumeConversion(value));
            }

            PlayerPrefs.SetFloat(_musicKey, value);
            PlayerPrefs.Save();
        }

        public void SetEffectsVolume(float value) {
            if (GetMute()) {
                this.audioMixer.SetFloat("EffectsVolume", -80f);
            }
            else {
                this.audioMixer.SetFloat("EffectsVolume", VolumeConversion(value));
            }

            PlayerPrefs.SetFloat(_effectsKey, value);
            PlayerPrefs.Save();
        }

        public void SetMute(bool muted) {
            PlayerPrefs.SetInt(_muteKey, muted ? 1 : 0);
            PlayerPrefs.Save();
            this.ApplySavedSettings();
        }

        public static float GetCurrentMusicVolume() => PlayerPrefs.GetFloat(_musicKey, 1f);

        public static float GetCurrentEffectsVolume() => PlayerPrefs.GetFloat(_effectsKey, 1f);

        public static bool GetMute() => PlayerPrefs.GetInt(_muteKey, 0) > 0;

        private static float VolumeConversion(float value) => Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }
}
