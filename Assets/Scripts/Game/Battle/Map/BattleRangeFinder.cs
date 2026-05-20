namespace Game.Battle.Map {
    using System.Collections.Generic;
    using System.Linq;
    using Battle.Data;
    using Data;
    using Item;
    using Unit;
    using UnityEngine;

    public class BattleRangeFinder {
        private readonly Dictionary<GridPosition, MapCell> _cells;
        private readonly BattleLineOfSight _lineOfSight;
        private readonly BattleMapData _mapData;
        private readonly BattleMapQueryService _queryService;

        public BattleRangeFinder(BattleMapData mapData, Dictionary<GridPosition, MapCell> cells,
            BattleMapQueryService queryService) {
            this._mapData = mapData;
            this._cells = cells;
            this._queryService = queryService;
            this._lineOfSight = new BattleLineOfSight(mapData);
        }

        public IReadOnlyList<TileData> GetReachableTiles(GridPosition origin, TileSearchConfig config) {
            Queue<TileData> queue = new();
            Dictionary<TileData, int> costs = new();
            HashSet<TileData> reachable = new();

            TileData originTile = this._mapData.GetTile(origin.Position.x, origin.Position.y);

            UnitObject occupant = this._cells[origin].GetOccupantUnit();
            BattleTeam? currentTeam = config.SourceTeam ?? occupant?.Team?.GetBattleTeam();

            queue.Enqueue(originTile);
            costs[originTile] = 0;
            bool isMovement = config.Target == Target.None;

            while (queue.Count > 0) {
                TileData current = queue.Dequeue();
                int currentCost = costs[current];

                foreach (GridPosition neighbourPos in this._mapData.GetNeighbours(current.TileGridPosition)) {
                    TileData next = this._mapData.GetTile(neighbourPos.Position.x, neighbourPos.Position.y);

                    int movementCost = isMovement
                        ? BattleMapQueryService.GetMovementCost(
                            current.TileGridPosition,
                            next.TileGridPosition
                        )
                        : 1;
                    int newCost = currentCost + movementCost;

                    if (newCost > config.Range && config.Range != -1) {
                        continue;
                    }

                    if (costs.TryGetValue(next, out int existingCost) && existingCost <= newCost) {
                        continue;
                    }

                    if (isMovement && config.CanEnterCheck &&
                        !this._queryService.CanEnter(current.TileGridPosition, next.TileGridPosition)) {
                        continue;
                    }

                    costs[next] = newCost;
                    queue.Enqueue(next);

                    if (isMovement) {
                        reachable.Add(next);
                        continue;
                    }

                    MapCell neighbourCell = this._cells[neighbourPos];
                    UnitObject unit = neighbourCell.GetOccupantUnit();

                    if (unit == null) {
                        continue;
                    }

                    BattleTeam neighbourTeam = unit.Team.GetBattleTeam();

                    if (IsValidTarget(config, neighbourTeam, currentTeam) && CanSelect(config, unit) &&
                        this.IsValidLineOfSight(config, originTile, next)) {
                        reachable.Add(next);
                    }
                }
            }

            return reachable.ToList().AsReadOnly();
        }

        private static bool CanSelect(TileSearchConfig config, UnitObject unit) =>
            config.CanSelect == null || config.CanSelect(unit);

        private static bool IsValidTarget(TileSearchConfig config, BattleTeam neighbourTeam, BattleTeam? currentTeam) =>
            (config.Target == Target.Ally && neighbourTeam == currentTeam) ||
            (config.Target == Target.Enemy && neighbourTeam != currentTeam) ||
            config.Target == Target.Any;

        private bool IsValidLineOfSight(TileSearchConfig config, TileData from, TileData to) =>
            !config.RequiresLineOfSight || this._lineOfSight.HasLineOfSight(from, to, config.ApplyHeightLineOfSight);

        public IReadOnlyList<GridPosition> GetPositionsAround(GridPosition origin, int radius,
            bool includeOrigin = false) {
            List<GridPosition> positions = new();
            for (int x = -radius; x <= radius; x++) {
                int maxY = radius - Mathf.Abs(x);
                for (int y = -maxY; y <= maxY; y++) {
                    if (x == 0 && y == 0 && !includeOrigin) {
                        continue;
                    }

                    int targetX = origin.Position.x + x;
                    int targetY = origin.Position.y + y;
                    if (!this._mapData.IsInside(targetX, targetY)) {
                        continue;
                    }

                    TileData tile = this._mapData.GetTile(targetX, targetY);
                    if (tile == null) {
                        continue;
                    }

                    positions.Add(tile.TileGridPosition);
                }
            }

            return positions.AsReadOnly();
        }
    }
}
