namespace Game.Map.Battle.Renderer {
    using System.Collections.Generic;
    using UnityEngine;

    public enum HighlightColor {
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
        None
    }

    public class TileView : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private readonly Stack<HighlightColor> _highlightStack = new();
        private HighlightColor _currentHighlightColor;
        private Color _defaultColor;

        private void Awake() => this._defaultColor = this.spriteRenderer.color;

        public void Highlight(HighlightColor color) {
            this._highlightStack.Push(color);
            this.ApplyColor(color);
        }

        public void Unhighlight() {
            this._highlightStack.Pop();
            if (this._highlightStack.Count > 0) {
                HighlightColor previous = this._highlightStack.Peek();
                this.ApplyColor(previous);
            } else {
                this.ApplyColor(HighlightColor.None);
            }
        }

        private void ApplyColor(HighlightColor highlightColor) {
            this._currentHighlightColor = highlightColor;
            Color color = highlightColor switch {
                HighlightColor.Blue => new Color(0.0f, 0.5f, 1f, 1f),
                HighlightColor.Red => new Color(1f, 0.0f, 0.0f, 1f),
                HighlightColor.Green => new Color(0.4f, 1f, 0.2f, 1f),
                HighlightColor.Yellow => Color.softYellow,
                HighlightColor.Orange => Color.darkOrange,
                _ => this._defaultColor
            };
            this.spriteRenderer.color = color;
        }

        public void Select() {
            Color color = this._currentHighlightColor switch {
                HighlightColor.Blue => new Color(0.0f, 0.0f, 1f, 1f),
                HighlightColor.Red => new Color(0.7f, 0.0f, 0.0f, 1f),
                HighlightColor.Green => Color.darkGreen,
                _ => this._defaultColor
            };
            this.spriteRenderer.color = color;
        }

        public void UnSelect() => this.ApplyColor(this._currentHighlightColor);
    }
}
