namespace Game.Battle.Map.Data {
    using System;
    using Renderer;
    using UnityEngine;

    public class TileData {
        public TileView TileView;

        public TileData(Vector2Int position, Tile tile, int height) {
            this.TileGridPosition = new GridPosition(position, height);
            this.Tile = tile;
        }

        public Tile Tile { get; }
        public GridPosition TileGridPosition { get; }

        private bool Equals(TileData other) =>
            this.Tile == other.Tile && Equals(this.TileGridPosition, other.TileGridPosition);

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((TileData)obj);
        }

        public override int GetHashCode() => HashCode.Combine(this.Tile, this.TileGridPosition);
    }
}
