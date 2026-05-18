namespace Game.Map.Battle.Renderer {
    using System.Collections.Generic;
    using Data;

    public class BattleMapViewManager {
        private readonly Dictionary<GridPosition, MapCell> _cells;

        public BattleMapViewManager(Dictionary<GridPosition, MapCell> cells) => this._cells = cells;

        public void ApplyTransparencyView() {
            foreach (MapCell cell in this._cells.Values) {
                if (cell.GetOccupantUnit() != null) {
                    cell.SetVisibleBelow(false);
                }
                else {
                    if (cell.IsWalkable()) {
                        cell.SetTransparent(true);
                    }
                    else {
                        cell.SetVisible(false);
                    }
                }
            }
        }

        public void ApplyDefaultView() {
            foreach (MapCell cell in this._cells.Values) {
                if (cell.GetOccupantUnit() != null) {
                    cell.SetVisible(true);
                    cell.SetTransparent(false);
                }
                else {
                    if (cell.IsWalkable()) {
                        cell.SetTransparent(false);
                    }
                    else {
                        cell.SetVisible(true);
                    }
                }
            }
        }
    }
}
