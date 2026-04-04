namespace Map.Battle.Data {
    using UnityEngine;

    public class TileData {
        public TileData(Vector2Int position, TileType type, int height) {
            this.TileGridPosition = new GridPosition(position, height);
            this.Type = type;
        }

        public TileType Type { get; }
        public GridPosition TileGridPosition { get; }
    }
}
