namespace Game.Map.Battle.Data {
    using System;
    using UnityEngine;

    [Serializable]
    public class Tile {
        [SerializeField] private TileType type;
        [SerializeField] private TileTypeVariant variant;

        public Tile(TileType type, TileTypeVariant variant) {
            this.type = type;
            this.variant = variant;
        }

        public TileType Type => this.type;
        public TileTypeVariant Variant => this.variant;
        public bool IsRenderBellow() => this.type.IsRenderBellow();
        public bool IsWalkable() => this.type.IsWalkable();

        public bool Equals(Tile other) {
            if (other is null) {
                return false;
            }

            return this.type == other.type &&
                   this.variant == other.variant;
        }

        public override bool Equals(object obj) =>
            obj is Tile other && this.Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine(this.type, this.variant);

        public override string ToString() =>
            $"{this.type}:{this.variant}";
    }
}
