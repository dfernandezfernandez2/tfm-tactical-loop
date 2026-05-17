namespace Game.Map.Battle.Data {
    using System;

    public class NodeGrid : IEquatable<NodeGrid> {
        public readonly GridPosition GridPosition;
        public int FCost;
        public int GCost;
        public int HCost;
        public NodeGrid Parent;

        public NodeGrid(GridPosition gridPosition) {
            this.GridPosition = gridPosition;
            this.GCost = int.MaxValue;
            this.HCost = 0;
            this.FCost = int.MaxValue;
            this.Parent = null;
        }

        public bool Equals(NodeGrid other) {
            if (other is null) {
                return false;
            }

            return ReferenceEquals(this, other) || Equals(this.GridPosition, other.GridPosition);
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            if (ReferenceEquals(this, obj)) {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((NodeGrid)obj);
        }

        public override int GetHashCode() => HashCode.Combine(this.GridPosition);
    }
}
