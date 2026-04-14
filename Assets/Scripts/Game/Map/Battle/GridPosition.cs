namespace Game.Map.Battle {
    using System;
    using UnityEngine;

    public class GridPosition {
        public GridPosition(Vector2Int gridPosition, int height) {
            this.Position = gridPosition;
            this.Height = height;
        }

        public Vector2Int Position { get; }
        public int Height { get; }

        public Vector2Int GetDirectionTo(GridPosition target)
        {
            Vector2Int delta = target.Position - this.Position;
            return new Vector2Int(
                Math.Sign(delta.x),
                Math.Sign(delta.y)
            );
        }

        private bool Equals(GridPosition other) => this.Position.Equals(other.Position) && this.Height == other.Height;

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((GridPosition)obj);
        }

        public override int GetHashCode() => HashCode.Combine(this.Position, this.Height);

        public override string ToString() => this.Position + " - " + this.Height;
    }
}
