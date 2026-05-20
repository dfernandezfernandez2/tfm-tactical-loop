namespace Game.Menu.Options {
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class VideoOptionsSection : OptionsSection {
        private const string _resolutionIndexKey = "ResolutionIndex";
        private const string _fpsIndexKey = "FpsIndex";
        private const string _fullscreenKey = "Fullscreen";

        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown fpsDropdown;
        [SerializeField] private Toggle fullScreenToggle;

        private readonly List<Resolution> _resolutions = new();

        private void Start() {
            this.LoadResolutions();
            this.LoadFps();
            this.LoadSettings();

            this.resolutionDropdown.onValueChanged.AddListener(this.SetResolution);
            this.fpsDropdown.onValueChanged.AddListener(SetFps);
            this.fullScreenToggle.onValueChanged.AddListener(this.SetFullScreen);
        }

        public override string GetTitle() => "Video";

        private void LoadResolutions() {
            this.resolutionDropdown.ClearOptions();
            this._resolutions.Clear();

            foreach (Resolution resolution in Screen.resolutions) {
                if (!this._resolutions.Exists(r => r.width == resolution.width && r.height == resolution.height)) {
                    this._resolutions.Add(resolution);
                }
            }

            List<string> options = this._resolutions.Select(resolution => $"{resolution.width} x {resolution.height}")
                .ToList();
            this.resolutionDropdown.AddOptions(options);
        }

        private void LoadFps() {
            this.fpsDropdown.ClearOptions();
            this.fpsDropdown.AddOptions(new List<string> {
                "30 FPS",
                "60 FPS",
                "120 FPS",
                "No limit"
            });
        }

        private void LoadSettings() {
            int resolutionIndex = PlayerPrefs.GetInt(_resolutionIndexKey, this.GetCurrentResolutionIndex());
            int fpsIndex = PlayerPrefs.GetInt(_fpsIndexKey, 1);
            bool fullscreen = PlayerPrefs.GetInt(_fullscreenKey, 1) == 1;

            resolutionIndex = Mathf.Clamp(resolutionIndex, 0, this._resolutions.Count - 1);
            fpsIndex = Mathf.Clamp(fpsIndex, 0, this.fpsDropdown.options.Count - 1);

            this.resolutionDropdown.SetValueWithoutNotify(resolutionIndex);
            this.fpsDropdown.SetValueWithoutNotify(fpsIndex);
            this.fullScreenToggle.SetIsOnWithoutNotify(fullscreen);

            this.SetFullScreen(fullscreen);
            this.SetResolution(resolutionIndex);
            SetFps(fpsIndex);
        }

        private int GetCurrentResolutionIndex() {
            for (int i = 0; i < this._resolutions.Count; i++) {
                Resolution resolution = this._resolutions[i];
                if (resolution.width == Screen.currentResolution.width &&
                    resolution.height == Screen.currentResolution.height) {
                    return i;
                }
            }

            return this._resolutions.Count - 1;
        }

        private void SetResolution(int index) {
            Resolution resolution = this._resolutions[index];
            Screen.SetResolution(
                resolution.width,
                resolution.height,
                Screen.fullScreen
            );
            PlayerPrefs.SetInt(_resolutionIndexKey, index);
            PlayerPrefs.Save();
        }

        private static void SetFps(int index) {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = index switch {
                0 => 30,
                1 => 60,
                2 => 120,
                3 => -1,
                _ => 60
            };
            PlayerPrefs.SetInt(_fpsIndexKey, index);
            PlayerPrefs.Save();
        }

        private void SetFullScreen(bool fullscreen) {
            Screen.fullScreen = fullscreen;
            if (this.resolutionDropdown.value >= 0 &&
                this.resolutionDropdown.value < this._resolutions.Count) {
                Resolution resolution = this._resolutions[this.resolutionDropdown.value];

                Screen.SetResolution(
                    resolution.width,
                    resolution.height,
                    fullscreen
                );
            }

            PlayerPrefs.SetInt(_fullscreenKey, fullscreen ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
