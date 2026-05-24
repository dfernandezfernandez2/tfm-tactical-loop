namespace Game.Translation {
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TMP_Text))]
    public class TranslatedText : MonoBehaviour {
        [SerializeField] private string id;
        private TMP_Text _text;

        private void Awake() => this._text = this.GetComponent<TMP_Text>();

        private void OnEnable() {
            TranslatorManager.OnLanguageChanged += this.Refresh;
            this.Refresh();
        }

        private void OnDisable() => TranslatorManager.OnLanguageChanged -= this.Refresh;

        public void SetId(string translationId) {
            this.id = translationId;
            this.Refresh();
        }

        private void Refresh() {
            if (this._text == null) {
                this._text = this.GetComponent<TMP_Text>();
            }

            this._text.text = TranslatorManager.Get(this.id);
        }
    }
}
