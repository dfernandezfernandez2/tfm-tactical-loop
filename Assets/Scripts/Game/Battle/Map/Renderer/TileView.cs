namespace Game.Battle.Map.Renderer {
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
        [SerializeField] private SpriteRenderer transparentSpriteRenderer;
        private readonly Stack<HighlightColor> _highlightStack = new();
        private readonly List<TileView> _tilesBelow = new();
        private HighlightColor _currentHighlightColor;
        private Color _defaultColor;

        private void Awake() => this._defaultColor = this.spriteRenderer.color;

        public void SetTilesBelow(List<TileView> tilesBelow) {
            this._tilesBelow.Clear();
            this._tilesBelow.AddRange(tilesBelow);
        }

        public void Highlight(HighlightColor color) {
            this._highlightStack.Push(color);
            this.ApplyColor(color);
        }

        public void Unhighlight() {
            if (this._highlightStack.Count == 0) {
                return;
            }

            this._highlightStack.Pop();
            if (this._highlightStack.Count > 0) {
                HighlightColor previous = this._highlightStack.Peek();
                this.ApplyColor(previous);
            }
            else {
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
                HighlightColor.Orange => Color.pink,
                _ => this._defaultColor
            };
            this.spriteRenderer.color = color;
        }

        public void Select() {
            Color color = this._currentHighlightColor switch {
                HighlightColor.Blue => new Color(0.0f, 0.0f, 1f, 1f),
                HighlightColor.Red => new Color(0.7f, 0.0f, 0.0f, 1f),
                HighlightColor.Green => Color.darkGreen,
                HighlightColor.Yellow => Color.darkGoldenRod,
                HighlightColor.Orange => Color.deepPink,
                _ => this._defaultColor
            };
            this.spriteRenderer.color = color;
        }

        public void UnSelect() => this.ApplyColor(this._currentHighlightColor);

        public void SetTransparent(bool transparent) {
            if (transparent) {
                this.transparentSpriteRenderer.gameObject.SetActive(true);
                this.spriteRenderer.gameObject.SetActive(false);
            }
            else {
                this.spriteRenderer.gameObject.SetActive(true);
                this.transparentSpriteRenderer.gameObject.SetActive(false);
            }

            foreach (TileView tileView in this._tilesBelow) {
                tileView.SetVisible(!transparent);
            }
        }

        public void SetVisible(bool visible) {
            this.spriteRenderer.gameObject.SetActive(visible);
            this.SetVisibleBelow(visible);
        }

        public void SetVisibleBelow(bool visible) {
            foreach (TileView tileView in this._tilesBelow) {
                tileView.SetVisible(visible);
            }
        }

        public void SetupSortingOrder(int sortOrder) {
            this.spriteRenderer.sortingOrder = sortOrder;
            this.transparentSpriteRenderer.sortingOrder = sortOrder;
        }
    }
}
