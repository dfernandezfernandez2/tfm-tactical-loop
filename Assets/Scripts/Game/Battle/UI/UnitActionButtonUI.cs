namespace Game.Battle.UI {
    using System;
    using Actions;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class UnitActionButtonUI : MonoBehaviour, IPointerEnterHandler {
        [SerializeField] private TMP_Text label;
        [SerializeField] private Image highLight;

        private Button _button;

        private Action<ActionType> _onClick;
        private Action _onSelect;
        public ActionType ActionType { get; private set; }
        public bool IsAvailable { get; private set; }

        private void Awake() => this._button = this.GetComponent<Button>();

        public void Init(ActionType actionType, Action<ActionType> onClick, Action onSelect, bool isAvailable) {
            this.ActionType = actionType;
            this._onClick = onClick;
            this._onSelect = onSelect;
            this.SetAvailable(isAvailable);
            this.label.text = actionType.GetName();
        }

        public void SetSelected(bool isSelected) => this.highLight.gameObject.SetActive(isSelected);

        public void SetAvailable(bool isAvailable) {
            this.IsAvailable = isAvailable;
            this._button.onClick.RemoveAllListeners();
            Color color = this._button.image.color;
            color.a = isAvailable ? 1f : 0.4f;
            this._button.image.color = color;
            if (isAvailable) {
                this._button.onClick.AddListener(() => this._onClick(this.ActionType));
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (this.IsAvailable) {
                this._onSelect.Invoke();
            }
        }

    }
}
