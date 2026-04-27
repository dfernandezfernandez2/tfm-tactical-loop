namespace Game.Map.Run.UI {
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class NodeConnectionUI : MonoBehaviour {
        [SerializeField] private float thickness = 4f;

        private RectTransform _from;
        private Image _image;
        private RectTransform _parent;

        private RectTransform _rectTransform;
        private RectTransform _to;

        private void Awake() {
            this._rectTransform = this.GetComponent<RectTransform>();
            this._image = this.GetComponent<Image>();
        }

        public void Init(RectTransform from, RectTransform to, RectTransform parent) {
            this._from = from;
            this._to = to;
            this._parent = parent;
        }

        public void Refresh() {
            Vector2 from = this.GetLocalCenter(this._from);
            Vector2 to = this.GetLocalCenter(this._to);

            Vector2 direction = to - from;
            float distance = direction.magnitude;

            this._rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            this._rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            this._rectTransform.pivot = new Vector2(0.5f, 0.5f);

            this._rectTransform.anchoredPosition = from + (direction * 0.5f);
            this._rectTransform.sizeDelta = new Vector2(distance, this.thickness);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            this._rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }

        public void Select() {
            this._image.color = Color.yellow;
            this.SetThickness(this.thickness * 1.5f);
        }

        public void UnSelect() {
            this._image.color = Color.white;
            this.SetThickness(this.thickness);
        }

        private void SetThickness(float value) =>
            this._rectTransform.sizeDelta = new Vector2(
                this._rectTransform.sizeDelta.x,
                value
            );

        private Vector2 GetLocalCenter(RectTransform target) {
            Vector3 worldCenter = target.TransformPoint(target.rect.center);
            return this._parent.InverseTransformPoint(worldCenter);
        }
    }
}
