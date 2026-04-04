namespace Map {
    using UnityEngine;

    public class GridPosition {
        public GridPosition(Vector2Int gridPosition, int height) {
            this.Position = gridPosition;
            this.Height = height;
        }

        public Vector2Int Position { get; }
        public int Height { get; }
    }
}
