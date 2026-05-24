namespace Game.Menu.Options {
    using System.Collections.Generic;
    using Controls;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(GameOptionsManager))]
    public class GameOptionsUI : MonoBehaviour {
        [SerializeField] private GameObject panelOptions;
        [SerializeField] private List<OptionsSection> options;
        [SerializeField] private TMP_Text sectionTitle;
        private int _currentOptionIndex = -1;
        private OptionsSection _currentSection;
        private GameOptionsManager _gameOptionsManager;
        private bool _isActive;

        private void Awake() {
            this._gameOptionsManager = this.GetComponent<GameOptionsManager>();
            this.SelectNextOption();
        }

        private void Update() {
            if (!this._isActive) {
                return;
            }

            if (InputUtils.IsCancelKeyBoardSelected()) {
                this._gameOptionsManager.ShowMainOptions();
            }

            if (InputUtils.IsSwapNextSelected()) {
                this.SelectNextOption();
            }

            if (InputUtils.IsSwapPreviousSelected()) {
                this.SelectPreviousOption();
            }
        }

        public void SelectPreviousOption() {
            this._currentOptionIndex--;
            if (this._currentOptionIndex < 0) {
                this._currentOptionIndex = this.options.Count - 1;
            }

            this.SelectOption();
        }

        public void SelectNextOption() {
            this._currentOptionIndex++;
            if (this._currentOptionIndex >= this.options.Count) {
                this._currentOptionIndex = 0;
            }

            this.SelectOption();
        }

        private void SelectOption() {
            this._currentSection?.Hide();
            this._currentSection = this.options[this._currentOptionIndex];
            this.sectionTitle.text = this._currentSection.GetTitle();
            this._currentSection.Show();
        }

        public void Show() {
            this._isActive = true;
            this.panelOptions.SetActive(true);
            this._currentOptionIndex = 0;
            this.SelectOption();
        }

        public void Hide() {
            this.panelOptions.SetActive(false);
            this._isActive = false;
        }
    }
}
