namespace Game.Menu.Options {
    using Audio;
    using Translation;
    using UnityEngine;
    using UnityEngine.UI;

    public class AudioOptionsSection : OptionsSection {
        [SerializeField] private Scrollbar effectsScrollbar;
        [SerializeField] private Scrollbar musicScrollbar;
        [SerializeField] private Toggle fullMuteToggle;

        private void Start() {
            this.LoadSettings();
            this.effectsScrollbar.onValueChanged.AddListener(SetEffectsVolume);
            this.musicScrollbar.onValueChanged.AddListener(SetMusicVolume);
            this.fullMuteToggle.onValueChanged.AddListener(SetFullMute);
        }

        public override string GetTitle() => TranslatorManager.Get("menu.options.audio");

        private void LoadSettings() {
            this.effectsScrollbar.SetValueWithoutNotify(AudioManager.GetCurrentEffectsVolume());
            this.musicScrollbar.SetValueWithoutNotify(AudioManager.GetCurrentMusicVolume());
            this.fullMuteToggle.SetIsOnWithoutNotify(AudioManager.GetMute());
        }

        private static void SetMusicVolume(float value) => AudioManager.Instance.SetMusicVolume(value);

        private static void SetEffectsVolume(float value) => AudioManager.Instance.SetEffectsVolume(value);

        private static void SetFullMute(bool muted) => AudioManager.Instance.SetMute(muted);
    }
}
