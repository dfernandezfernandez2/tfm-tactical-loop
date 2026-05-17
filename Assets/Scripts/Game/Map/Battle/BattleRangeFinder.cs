namespace Game.Map.Battle {
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Data;
    using Game.Battle.Item;
    using Unit;

    public class BattleRangeFinder {
        private readonly Dictionary<GridPosition, MapCell> _cells;
        private readonly BattleMapData _mapData;
        private readonly BattleMapQueryService _queryService;

        public BattleRangeFinder(BattleMapData mapData, Dictionary<GridPosition, MapCell> cells,
            BattleMapQueryService queryService) {
            this._mapData = mapData;
            this._cells = cells;
            this._queryService = queryService;
        }

        public IReadOnlyList<TileData> GetReachableTiles(GridPosition origin, TileSearchConfig config) {
            Queue<TileData> queue = new();
            Dictionary<TileData, int> costs = new();
            HashSet<TileData> reachable = new();

            TileData originTile = this._mapData.GetTile(origin.Position.x, origin.Position.y);

            UnitObject occupant = this._cells[origin].GetOccupantUnit();
            BattleTeam? currentTeam = occupant?.Team?.GetBattleTeam();

            queue.Enqueue(originTile);
            costs[originTile] = 0;

            while (queue.Count > 0) {
                TileData current = queue.Dequeue();
                int currentCost = costs[current];

                foreach (GridPosition neighbourPos in this._mapData.GetNeighbours(current.TileGridPosition)) {
                    TileData next = this._mapData.GetTile(neighbourPos.Position.x, neighbourPos.Position.y);

                    int newCost = currentCost +
                                  BattleMapQueryService.GetMovementCost(
                                      current.TileGridPosition,
                                      next.TileGridPosition
                                  );

                    if (newCost > config.Range && config.Range != -1) {
                        continue;
                    }

                    if (costs.TryGetValue(next, out int existingCost) && existingCost <= newCost) {
                        continue;
                    }

                    if (config.Target == Target.None && config.CanEnterCheck && !this._queryService.CanEnter(next.TileGridPosition)) {
                        continue;
                    }

                    costs[next] = newCost;
                    queue.Enqueue(next);

                    if (config.Target == Target.None) {
                        reachable.Add(next);
                        continue;
                    }

                    MapCell neighbourCell = this._cells[neighbourPos];
                    UnitObject unit = neighbourCell.GetOccupantUnit();

                    if (unit == null) {
                        continue;
                    }

                    BattleTeam neighbourTeam = unit.Team.GetBattleTeam();

                    bool validTarget =
                        (config.Target == Target.Ally && neighbourTeam == currentTeam) ||
                        (config.Target == Target.Enemy && neighbourTeam != currentTeam);

                    if (validTarget && (config.CanSelect == null || config.CanSelect(unit))) {
                        reachable.Add(next);
                    }
                }
            }

            return reachable.ToList().AsReadOnly();
        }
    }
}
