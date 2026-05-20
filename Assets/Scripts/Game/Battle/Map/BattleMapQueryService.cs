namespace Game.Battle.Map {
    using System.Collections.Generic;
    using Data;
    using UnityEngine;

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

        public bool CanEnter(GridPosition origin, GridPosition target) {
            if (!this.IsInside(target)) {
                return false;
            }

            MapCell cell = this._cells.GetValueOrDefault(target);
            if (cell == null || !cell.IsWalkable()) {
                return false;
            }

            int heightDifference = Mathf.Abs(target.Height - origin.Height);
            return heightDifference <= 1;
        }

        public static int GetMovementCost(GridPosition origin, GridPosition target) {
            int cost = 1;
            int heightDifference = target.Height - origin.Height;
            if (heightDifference > 0) {
                cost = 2;
            }

            return cost;
        }
    }
}
