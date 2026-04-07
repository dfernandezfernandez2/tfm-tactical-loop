namespace Game.Battle.UI {
    using System;
    using Actions;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnitActionButtonUI : MonoBehaviour {
        [SerializeField] private TMP_Text label;

        [SerializeField] private Sprite defaultButtonImage;
        [SerializeField] private Sprite selectedButtonImage;
        [SerializeField] private Sprite disabledButtonImage;
        private Button _button;

        private bool _isSelected;

        private Action<ActionType> _onClick;
        public ActionType ActionType { get; private set; }
        public bool IsAvailable { get; private set; }

        private void Awake() {
            this._button = this.GetComponent<Button>();
            this._button.image.sprite = this.defaultButtonImage;
        }

        public void Init(ActionType actionType, Action<ActionType> onClick, bool isAvailable) {
            this.ActionType = actionType;
            this._onClick = onClick;
            this.IsAvailable = isAvailable;
            this._isSelected = false;
            this.label.text = actionType.GetName();

            this._button.onClick.RemoveAllListeners();
            this._button.onClick.AddListener(() => this._onClick(actionType));
        }

        public void SetSelected(bool isSelected) {
            this._isSelected = isSelected;
            this._button.image.sprite = this._isSelected ? this.selectedButtonImage : this.defaultButtonImage;
        }

        public void SetAvailable(bool isAvailable) {
            this.IsAvailable = isAvailable;
            this._button.image.sprite = this.IsAvailable ? this.defaultButtonImage : this.disabledButtonImage;
        }
    }
}
