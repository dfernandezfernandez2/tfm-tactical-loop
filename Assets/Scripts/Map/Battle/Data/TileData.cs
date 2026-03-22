namespace Map.Battle.Data {
    using UnityEngine;

    public class TileData {
        public TileData(Vector2Int position, TileType type, int height) {
            this.Position = position;
            this.Type = type;
            this.Height = height;
        }

        public Vector2Int Position { get; }
        public TileType Type { get; }
        public int Height { get; }
    }
}
