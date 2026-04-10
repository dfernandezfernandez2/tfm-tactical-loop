namespace Game.Map.Battle.Data {
    using System;
    using Renderer;
    using UnityEngine;

    public class TileData {
        public TileView TileView;

        public TileData(Vector2Int position, TileType type, int height) {
            this.TileGridPosition = new GridPosition(position, height);
            this.Type = type;
        }

        public TileType Type { get; }
        public GridPosition TileGridPosition { get; }

        private bool Equals(TileData other) =>
            this.Type == other.Type && Equals(this.TileGridPosition, other.TileGridPosition);

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((TileData)obj);
        }

        public override int GetHashCode() => HashCode.Combine((int)this.Type, this.TileGridPosition);
    }
}
