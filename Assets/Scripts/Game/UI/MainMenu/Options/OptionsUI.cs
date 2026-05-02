namespace Game.UI.MainMenu.Options {
    using System.Collections.Generic;
    using Core;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(MainMenuManager))]
    public class OptionsUI : MonoBehaviour {
        [SerializeField] private Canvas optionsCanvas;
        [SerializeField] private List<OptionsSection> options;
        [SerializeField] private TMP_Text sectionTitle;
        private int _currentOptionIndex = -1;
        private OptionsSection _currentSection;

        private MainMenuManager _mainMenuManager;

        private void Awake() {
            this._mainMenuManager = this.GetComponent<MainMenuManager>();
            this.SelectNextOption();
        }

        private void Update() {
            if (InputUtils.IsCancelKeyBoardSelected()) {
                this._mainMenuManager.ReturnToMenu();
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
            this.optionsCanvas.transform.gameObject.SetActive(true);
            this._currentOptionIndex = 0;
            this.SelectOption();
        }

        public void Hide() => this.optionsCanvas.transform.gameObject.SetActive(false);
    }
}
