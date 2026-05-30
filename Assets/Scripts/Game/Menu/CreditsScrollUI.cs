namespace Game.Menu {
    using System;
    using TMPro;
    using Translation;
    using UnityEngine;

    public class CreditsScrollUI : MonoBehaviour {
        [Header("Translation")] [SerializeField]
        private string creditsTranslationId = "credits.text";

        [Header("Scroll")] [SerializeField] private float scrollSpeed = 80f;

        [SerializeField] private float startOffset = 120f;
        [SerializeField] private float endOffset = 120f;

        [Header("Text")] [SerializeField] private TMP_Text creditsText;

        [SerializeField] private RectTransform creditsTextRectTransform;
        [SerializeField] private float horizontalMargin = 160f;

        [Header("Canvas")] [SerializeField] private RectTransform canvasRectTransform;

        private float _endY;
        private bool _finished;

        private float _startY;

        private void Awake() {
            this.creditsTextRectTransform.anchorMin = new Vector2(0f, 0.5f);
            this.creditsTextRectTransform.anchorMax = new Vector2(1f, 0.5f);
            this.creditsTextRectTransform.pivot = new Vector2(0.5f, 0.5f);
            this.creditsTextRectTransform.sizeDelta =
                new Vector2(-this.horizontalMargin * 2f, this.creditsText.preferredHeight);
        }

        private void Update() {
            if (this._finished) {
                return;
            }

            Vector2 position = this.creditsTextRectTransform.anchoredPosition;
            position.y += this.scrollSpeed * Time.unscaledDeltaTime;
            this.creditsTextRectTransform.anchoredPosition = position;

            if (position.y >= this._endY) {
                this._finished = true;
                this.OnCreditsFinished?.Invoke();
            }
        }

        private void OnEnable() {
            TranslatorManager.OnLanguageChanged += this.RefreshText;
            this.PlayFromStart();
        }

        private void OnDisable() => TranslatorManager.OnLanguageChanged -= this.RefreshText;
        public event Action OnCreditsFinished;

        public void PlayFromStart() {
            this._finished = false;
            this.RefreshText();
            this.CalculateLimits();
            this.creditsTextRectTransform.anchoredPosition = new Vector2(0f, this._startY);
        }

        private void RefreshText() {
            this.creditsText.text = TranslatorManager.Get(this.creditsTranslationId);
            this.creditsText.ForceMeshUpdate();

            this.creditsTextRectTransform.sizeDelta =
                new Vector2(-this.horizontalMargin * 2f, this.creditsText.preferredHeight);

            this.CalculateLimits();
        }

        private void CalculateLimits() {
            float canvasHeight = this.canvasRectTransform.rect.height;
            if (canvasHeight <= 0f) {
                canvasHeight = Screen.height;
            }

            float textHeight = Mathf.Max(this.creditsTextRectTransform.rect.height, this.creditsText.preferredHeight);

            this._startY = (-canvasHeight * 0.5f) - (textHeight * 0.5f) - this.startOffset;
            this._endY = (canvasHeight * 0.5f) + (textHeight * 0.5f) + this.endOffset;
        }
    }
}
