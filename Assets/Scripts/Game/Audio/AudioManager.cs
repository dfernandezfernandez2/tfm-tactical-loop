namespace Game.Audio {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioManager : MonoBehaviour {
        private const string _musicKey = "MusicVolume";
        private const string _effectsKey = "EffectsVolume";
        private const string _muteKey = "FullMute";

        [Header("Mixer")] [SerializeField] private AudioMixer audioMixer;

        [Header("Audio Sources")] [SerializeField]
        private AudioSource musicSource;

        [SerializeField] private AudioSource effectsSource;
        private readonly Dictionary<string, AudioClip> _musicCache = new();

        private readonly Dictionary<string, AudioClip> _soundCache = new();

        public static AudioManager Instance { get; private set; }

        private void Awake() {
            if (Instance != null) {
                Destroy(this.gameObject);
                return;
            }

            Instance = this;
        }

        private void Start() => this.ApplySavedSettings();

        private void ApplySavedSettings() {
            if (GetMute()) {
                this.audioMixer.SetFloat(_musicKey, -80f);
                this.audioMixer.SetFloat(_effectsKey, -80f);
                return;
            }

            this.SetMusicVolume(GetCurrentMusicVolume());
            this.SetEffectsVolume(GetCurrentEffectsVolume());
        }

        public void SetMusicVolume(float value) {
            if (GetMute()) {
                this.audioMixer.SetFloat(_musicKey, -80f);
            }
            else {
                this.audioMixer.SetFloat(_musicKey, VolumeConversion(value));
            }

            PlayerPrefs.SetFloat(_musicKey, value);
            PlayerPrefs.Save();
        }

        public void SetEffectsVolume(float value) {
            if (GetMute()) {
                this.audioMixer.SetFloat(_effectsKey, -80f);
            }
            else {
                this.audioMixer.SetFloat(_effectsKey, VolumeConversion(value));
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

        public IEnumerator PlaySound(string soundName, float volume = 1f, bool wait = false) {
            AudioClip clip = this.GetSound(soundName);
            if (clip == null) {
                yield break;
            }

            this.effectsSource.PlayOneShot(clip, volume);
            if (!wait) {
                yield break;
            }

            yield return new WaitForSeconds(clip.length);
        }

        public void PlayMusic(string musicName, bool loop = true) {
            AudioClip clip = this.GetMusic(musicName);
            if (clip == null) {
                return;
            }

            if (this.musicSource.clip == clip && this.musicSource.isPlaying) {
                return;
            }

            this.musicSource.clip = clip;
            this.musicSource.loop = loop;
            this.musicSource.Play();
        }

        public void StopMusic() {
            this.musicSource.Stop();
            this.musicSource.clip = null;
        }

        private AudioClip GetMusic(string musicName) {
            if (string.IsNullOrWhiteSpace(musicName)) {
                return null;
            }

            if (this._musicCache.TryGetValue(musicName, out AudioClip clip)) {
                return clip;
            }

            clip = Resources.Load<AudioClip>($"Music/{musicName}");
            if (!clip) {
                return null;
            }

            this._musicCache[musicName] = clip;
            return clip;
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
