namespace Game.Map.Battle {
    using System.Collections.Generic;
    using Data;

    public class BattleMapQueryService {
        private readonly Dictionary<GridPosition, MapCell> _cells;
        private readonly BattleMapData _mapData;

        public BattleMapQueryService(BattleMapData mapData, Dictionary<GridPosition, MapCell> cells) {
            this._mapData = mapData;
            this._cells = cells;
        }

        private bool IsInside(GridPosition position) =>
            this._mapData.IsInside(position.Position.x, position.Position.y);

        public bool IsAvailablePosition(GridPosition position) {
            MapCell cell = this._cells.GetValueOrDefault(position);
            return cell != null && cell.IsWalkable();
        }

        public bool CanEnter(GridPosition position) {
            if (!this.IsInside(position)) {
                return false;
            }

            MapCell cell = this._cells.GetValueOrDefault(position);
            return cell != null && cell.IsWalkable();
        }

        public static int GetMovementCost(GridPosition origin, GridPosition target) => 1;
    }
}
