namespace Game.Menu {
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class MenuButtonUI : MonoBehaviour, IPointerEnterHandler {
        [SerializeField] private Image image;
        [SerializeField] private Sprite selectedSpriteBackground;

        private Button _button;
        private int _index;
        private Action<int> _onSelect;

        private void Awake() => this._button = this.GetComponent<Button>();

        private void Start() {
            // remove navigation to avoid problems with keyboard and ui buttons
            Navigation nav = this._button.navigation;
            nav.mode = Navigation.Mode.None;
            this._button.navigation = nav;
        }

        public void OnPointerEnter(PointerEventData eventData) => this._onSelect?.Invoke(this._index);

        public void Init(int index, Action<int> action) {
            this._onSelect = action;
            this._index = index;
        }

        public void Select() {
            this.image.sprite = this.selectedSpriteBackground;
            Color color = this.image.color;
            color.a = 1f;
            this.image.color = color;
        }

        public void UnSelect() {
            this.image.sprite = null;
            Color color = this.image.color;
            color.a = 0f;
            this.image.color = color;
        }

        public void DoOnClick() => this._button.onClick.Invoke();
    }
}
