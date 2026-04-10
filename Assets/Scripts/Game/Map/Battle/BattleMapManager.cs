namespace Game.Map.Battle {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Unit;
    using UnityEngine;

    public class BattleMapManager : MonoBehaviour {
        private readonly Dictionary<GridPosition, MapCell> _cells = new();

        private BattleMapData _mapData;

        public void Initialize(BattleMapData mapData) {
            this._mapData = mapData;
            this._cells.Clear();
            this._mapData.ForEach(data => this._cells[data.TileGridPosition] = new MapCell(data));
        }

        public void UnitMove(GridPosition from, GridPosition to) {
            MapCell cell = this._cells.GetValueOrDefault(from);
            MapCell toCell = this._cells.GetValueOrDefault(to);
            UnitObject unitObject = cell.GetOccupantUnit();
            cell.ClearOccupantUnit();
            toCell.SetOccupantUnit(unitObject);
        }

        public IReadOnlyList<TileData> GetReachableTiles(GridPosition origin, int movement) {
            Queue<TileData> queue = new();

            Dictionary<TileData, int> costs = new();
            HashSet<TileData> reachable = new();

            TileData originTile = this._mapData.GetTile(origin.Position.x, origin.Position.y);

            queue.Enqueue(originTile);
            costs[originTile] = 0;

            while (queue.Count > 0) {
                TileData current = queue.Dequeue();
                int currentCost = costs[current];

                foreach (GridPosition neighbourPos in this._mapData.GetNeighbours(current.TileGridPosition)) {
                    TileData next = this._mapData.GetTile(neighbourPos.Position.x, neighbourPos.Position.y);

                    if (!this.CanEnter(next.TileGridPosition)) {
                        continue;
                    }

                    int newCost = currentCost + GetMovementCost(current.TileGridPosition, next.TileGridPosition);

                    if (newCost > movement) {
                        continue;
                    }

                    if (costs.TryGetValue(next, out int existingCost) && existingCost <= newCost) {
                        continue;
                    }

                    costs[next] = newCost;
                    queue.Enqueue(next);

                    reachable.Add(next);
                }
            }

            return reachable.ToList().AsReadOnly();
        }

        private static int GetMovementCost(GridPosition origin, GridPosition target) => 1;

        private bool CanEnter(GridPosition position) {
            if (!this._mapData.IsInside(position.Position.x, position.Position.y)) {
                return false;
            }

            MapCell cell = this._cells.GetValueOrDefault(position);
            return cell != null && cell.IsWalkable();
        }

        // A* algorithm
        public IReadOnlyList<GridPosition> FindPath(GridPosition origin, GridPosition target) {
            List<NodeGrid> openList = new();
            List<NodeGrid> closedList = new();
            Dictionary<GridPosition, NodeGrid> allNodes = new();
            NodeGrid originNode = GetOrCreateNode(origin, allNodes);
            originNode.GCost = 0;
            originNode.HCost = GetHeuristicCost(origin, target);
            originNode.FCost = originNode.GCost + originNode.HCost;
            openList.Add(originNode);
            while (openList.Count > 0) {
                NodeGrid current = GetNodeWithLowestFValue(openList);
                if (current.GridPosition.Equals(target)) {
                    return ReconstructPath(current);
                }

                openList.Remove(current);
                closedList.Add(current);
                foreach (GridPosition neighbour in this._mapData.GetNeighbours(current.GridPosition)) {
                    if (!this.CanEnter(neighbour) && !neighbour.Equals(target)) {
                        continue;
                    }

                    NodeGrid neighbourNode = GetOrCreateNode(neighbour, allNodes);
                    if (closedList.Contains(neighbourNode)) {
                        continue;
                    }

                    int tentativeGCost =
                        current.GCost + GetMovementCost(current.GridPosition, neighbourNode.GridPosition);
                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                    else if (tentativeGCost >= neighbourNode.GCost) {
                        continue; // this path is no better
                    }

                    neighbourNode.Parent = current;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = GetHeuristicCost(neighbour, target);
                    neighbourNode.FCost = neighbourNode.GCost + neighbourNode.HCost;
                }
            }

            return new List<GridPosition>().AsReadOnly();
        }

        private static NodeGrid GetOrCreateNode(GridPosition gridPosition,
            Dictionary<GridPosition, NodeGrid> allNodes) {
            if (allNodes.TryGetValue(gridPosition, out NodeGrid existingNode)) {
                return existingNode;
            }

            NodeGrid newNode = new(gridPosition);
            allNodes[gridPosition] = newNode;
            return newNode;
        }

        private static int GetHeuristicCost(GridPosition origin, GridPosition target) =>
            // Manhattan distance
            Mathf.Abs(origin.Position.x - target.Position.x) +
            Mathf.Abs(origin.Position.y - target.Position.y);

        private static NodeGrid GetNodeWithLowestFValue(List<NodeGrid> openList) =>
            openList
                .OrderBy(n => n.FCost)
                .ThenBy(n => n.HCost)
                .First();

        private static IReadOnlyList<GridPosition> ReconstructPath(NodeGrid origin) {
            LinkedList<GridPosition> path = new();
            NodeGrid current = origin;
            while (current != null) {
                path.AddFirst(current.GridPosition);
                current = current.Parent;
            }

            return path.ToList().AsReadOnly();
        }

        public UnitObject GetUnit(GridPosition gridPosition) {
            MapCell cell = this._cells.GetValueOrDefault(gridPosition);
            return cell?.GetOccupantUnit();
        }

        private class NodeGrid : IEquatable<NodeGrid> {
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
}
