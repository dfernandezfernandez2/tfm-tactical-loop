namespace Game.Translation {
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class TranslatorManager {
        private const string _prefsLanguageKey = "prefLanguage";
        private const string _csvPath = "Translations/translations";
        private const string _idKey = "id";
        private static readonly Dictionary<string, Dictionary<Language, string>> _translations = new();
        private static bool _isLoaded;
        public static event Action OnLanguageChanged;

        public static Language GetCurrentLanguage() {
            string savedLanguage = PlayerPrefs.GetString(_prefsLanguageKey, nameof(Language.English));
            return Enum.TryParse(savedLanguage, out Language language) ? language : Language.English;
        }

        public static void SetLanguage(Language language) {
            PlayerPrefs.SetString(_prefsLanguageKey, language.ToString());
            PlayerPrefs.Save();
            OnLanguageChanged?.Invoke();
        }

        public static string Get(string translationId) {
            LoadTranslations();
            if (string.IsNullOrWhiteSpace(translationId)) {
                return string.Empty;
            }

            if (!_translations.TryGetValue(translationId, out Dictionary<Language, string> translationsByLanguage)) {
                return translationId;
            }

            Language currentLanguage = GetCurrentLanguage();
            return translationsByLanguage.GetValueOrDefault(currentLanguage, translationId);
        }

        public static string Format(string translationId, params object[] args) =>
            string.Format(Get(translationId), args);

        private static void LoadTranslations() {
            if (_isLoaded) {
                return;
            }

            List<Dictionary<string, string>> parsedCsv = TranslationFileReader.Read(_csvPath);
            foreach (Dictionary<string, string> row in parsedCsv) {
                if (!row.TryGetValue(_idKey, out string id) || string.IsNullOrWhiteSpace(id)) {
                    throw new ArgumentException($"Missing translation id key at row {row}");
                }

                _translations[row[_idKey]] = new Dictionary<Language, string>();
                foreach ((string columnName, string value) in row) {
                    if (!Enum.TryParse(columnName, true, out Language language)) {
                        continue;
                    }

                    _translations[row[_idKey]][language] = value;
                }
            }

            _isLoaded = true;
        }
    }
}
