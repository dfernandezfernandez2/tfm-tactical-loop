namespace Game.Menu.Options {
    using System;
    using System.Collections.Generic;
    using Controls;
    using Translation;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable]
    public struct LanguageEntry : IEquatable<LanguageEntry> {
        [FormerlySerializedAs("Language")] public Language language;
        [FormerlySerializedAs("LanguageUI")] public LanguageUI languageUI;

        public bool Equals(LanguageEntry other) =>
            this.language == other.language && Equals(this.languageUI, other.languageUI);

        public override bool Equals(object obj) => obj is LanguageEntry other && this.Equals(other);

        public override int GetHashCode() => HashCode.Combine((int)this.language, this.languageUI);
    }

    public class LanguageOptionsSection : OptionsSection {
        [SerializeField] private List<LanguageEntry> languageEntries;

        private LanguageEntry _currentLanguage;
        private int _currentOptionIndex;

        private void Awake() {
            this._currentLanguage = this.FindLanguageEntry(TranslatorManager.GetCurrentLanguage());
            this._currentOptionIndex = this.languageEntries.FindIndex(entry => entry.Equals(this._currentLanguage));
            if (this._currentOptionIndex < 0) {
                this._currentOptionIndex = 0;
                this._currentLanguage = this.languageEntries[0];
            }

            this._currentLanguage.languageUI.Select();
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
                this.SelectLanguage(this._currentLanguage.language);
            }
        }

        public override string GetTitle() => TranslatorManager.Get("menu.options.language");

        private void SelectOption() {
            this._currentLanguage.languageUI.UnSelect();
            this._currentLanguage = this.languageEntries[this._currentOptionIndex];
            this._currentLanguage.languageUI.Select();
        }

        public void SelectLanguage(Language language) {
            this._currentLanguage.languageUI.UnSelect();
            this._currentLanguage = this.FindLanguageEntry(language);
            this._currentOptionIndex = this.languageEntries.FindIndex(entry => entry.Equals(this._currentLanguage));
            this._currentLanguage.languageUI.Select();
            TranslatorManager.SetLanguage(language);
        }

        public void SelectLanguage(string language) {
            Enum.TryParse(language, out Language languageEnum);
            this.SelectLanguage(languageEnum);
        }

        private LanguageEntry FindLanguageEntry(Language language) =>
            this.languageEntries.Find(entry => entry.language == language);
    }
}
