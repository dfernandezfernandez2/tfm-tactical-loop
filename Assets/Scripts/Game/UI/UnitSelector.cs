namespace Game.UI {
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Color = UnityEngine.Color;

    [RequireComponent(typeof(Image))]
    public class UnitSelector : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
        private Image _image;
        public Action<int> OnConfirm;

        public Action<int> OnSelect;

        public int Index { get; set; }

        private void Awake() => this._image = this.GetComponent<Image>();

        public void OnPointerClick(PointerEventData eventData) => this.ConfirmSelect();

        public void OnPointerEnter(PointerEventData eventData) => this.Select();

        private void Select() => this.OnSelect?.Invoke(this.Index);

        private void ConfirmSelect() => this.OnConfirm?.Invoke(this.Index);

        public void DoSelect() {
            Color color = this._image.color;
            color.a = 1f;
            this._image.color = color;
        }

        public void DoUnSelect() {
            Color color = this._image.color;
            color.a = 0.5f;
            this._image.color = color;
        }
    }
}
