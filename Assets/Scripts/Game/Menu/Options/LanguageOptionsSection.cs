namespace Game.Menu.Options {
    using System;
    using System.Collections.Generic;
    using Controls;
    using UnityEngine;

    [Serializable]
    public struct LanguageEntry {
        public string Name;
        public LanguageUI LanguageUI;
    }

    public class LanguageOptionsSection : OptionsSection {
        private const string _languageKey = "Language";

        [SerializeField] private List<LanguageEntry> languageEntries;

        private LanguageEntry _currentLanguage;
        private int _currentOptionIndex;

        private void Awake() {
            this._currentLanguage = PlayerPrefs.HasKey(_languageKey)
                ? this.FindLanguageEntry(PlayerPrefs.GetString(_languageKey))
                : this.languageEntries[0];
            this._currentOptionIndex = this.languageEntries.FindIndex(entry => entry.Equals(this._currentLanguage));
            this._currentLanguage.LanguageUI.Select();
        }

        private void Update() {
            if (InputUtils.IsRightSelected()) {
                this._currentOptionIndex++;
                if (this._currentOptionIndex >= this.languageEntries.Count) {
                    this._currentOptionIndex = 0;
                }

                this.SelectOption();
            }

            if (InputUtils.IsLeftSelected()) {
                this._currentOptionIndex--;
                if (this._currentOptionIndex < 0) {
                    this._currentOptionIndex = this.languageEntries.Count - 1;
                }

                this.SelectOption();
            }

            if (InputUtils.IsEnterSelected()) {
                this.SelectLanguage(this._currentLanguage.Name);
            }
        }

        public override string GetTitle() => "Language";

        private void SelectOption() {
            this._currentLanguage.LanguageUI.UnSelect();
            this._currentLanguage = this.languageEntries[this._currentOptionIndex];
            this._currentLanguage.LanguageUI.Select();
        }

        public void SelectLanguage(string language) {
            this._currentLanguage.LanguageUI.UnSelect();
            this._currentLanguage = this.FindLanguageEntry(language);
            this._currentOptionIndex = this.languageEntries.FindIndex(entry => entry.Equals(this._currentLanguage));
            this._currentLanguage.LanguageUI.Select();
            PlayerPrefs.SetString(_languageKey, language);
            PlayerPrefs.Save();
        }

        private LanguageEntry FindLanguageEntry(string language) =>
            this.languageEntries.Find(entry => entry.Name.Equals(language));
    }
}
