namespace Game.Map.Battle {
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using UnityEngine;

    public class BattlePathfinder {
        private readonly BattleMapData _mapData;
        private readonly BattleMapQueryService _queryService;

        public BattlePathfinder(BattleMapData mapData, BattleMapQueryService queryService) {
            this._mapData = mapData;
            this._queryService = queryService;
        }

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
                    if (!this._queryService.CanEnter(neighbour) && !neighbour.Equals(target)) {
                        continue;
                    }

                    NodeGrid neighbourNode = GetOrCreateNode(neighbour, allNodes);

                    if (closedList.Contains(neighbourNode)) {
                        continue;
                    }

                    int tentativeGCost =
                        current.GCost +
                        BattleMapQueryService.GetMovementCost(current.GridPosition, neighbourNode.GridPosition);

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                    else if (tentativeGCost >= neighbourNode.GCost) {
                        continue;
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
    }
}
