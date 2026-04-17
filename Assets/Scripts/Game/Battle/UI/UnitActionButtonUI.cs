namespace Game.Battle.UI {
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UnitActionButtonUI : MonoBehaviour, IPointerEnterHandler {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Image highLight;

        private Button _button;
        private Func<bool> _canDoAction;

        private Action _onClick;
        private Action _onSelect;
        public bool IsAvailable { get; private set; }

        private void Awake() => this._button = this.GetComponent<Button>();

        public void OnPointerEnter(PointerEventData eventData) {
            if (this.IsAvailable) {
                this._onSelect.Invoke();
            }
        }

        public void Init(string actionName, Action onClick, Action onSelect, Func<bool> canDoAction) {
            this._onClick = onClick;
            this._onSelect = onSelect;
            this._canDoAction = canDoAction;
            this.RefreshIsAvailable();
            this.label.text = actionName;
        }

        public void SetSelected(bool isSelected) => this.highLight.gameObject.SetActive(isSelected);

        public void RefreshIsAvailable() {
            bool isAvailable = this._canDoAction();
            this.IsAvailable = isAvailable;
            this._button.onClick.RemoveAllListeners();
            Color color = this._button.image.color;
            color.a = isAvailable ? 1f : 0.4f;
            this._button.image.color = color;
            if (isAvailable) {
                this._button.onClick.AddListener(() => this._onClick());
            }
        }

        public void OnEnter() => this._onClick();

    }
}
