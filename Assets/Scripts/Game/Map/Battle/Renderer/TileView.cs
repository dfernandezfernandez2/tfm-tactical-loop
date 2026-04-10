namespace Game.Map.Battle.Renderer {
    using UnityEngine;

    public enum HighlightColor {
        Red,
        Blue,
        None
    }

    public class TileView : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private HighlightColor _currentHighlightColor;
        private Color _defaultColor;

        private void Awake() => this._defaultColor = this.spriteRenderer.color;

        public void Highlight(HighlightColor color) => this.ApplyColor(color);

        public void Unhighlight() => this.ApplyColor(HighlightColor.None);

        private void ApplyColor(HighlightColor highlightColor) {
            this._currentHighlightColor = highlightColor;
            Color color = highlightColor switch {
                HighlightColor.Blue => new Color(0.0f, 0.5f, 1f, 1f),
                HighlightColor.Red => new Color(1f, 0.0f, 0.0f, 1f),
                _ => this._defaultColor
            };
            this.spriteRenderer.color = color;
        }

        public void Select() {
            Color color = this._currentHighlightColor switch {
                HighlightColor.Blue => new Color(0.0f, 0.0f, 1f, 1f),
                HighlightColor.Red => new Color(0.7f, 0.0f, 0.0f, 1f),
                _ => this._defaultColor
            };
            this.spriteRenderer.color = color;
        }

        public void UnSelect() => this.ApplyColor(this._currentHighlightColor);
    }
}
