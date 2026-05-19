namespace Game.Battle.Map {
    using System.Collections.Generic;
    using System.Linq;
    using Battle.Data;
    using Data;
    using Renderer;

    public class BattleMapHighlighter {
        private readonly Dictionary<GridPosition, MapCell> _cells;

        public BattleMapHighlighter(Dictionary<GridPosition, MapCell> cells) => this._cells = cells;

        public void Highlight(GridPosition position, HighlightColor color) =>
            this._cells.GetValueOrDefault(position)?.HighlightCell(color);

        public void UnHighlight(GridPosition position) =>
            this._cells.GetValueOrDefault(position)?.UnHighlightCell();

        public void Select(GridPosition position) =>
            this._cells.GetValueOrDefault(position)?.Select();

        public void UnSelect(GridPosition position) =>
            this._cells.GetValueOrDefault(position)?.UnSelect();

        public void HighlightUnits() {
            foreach (MapCell cell in this._cells.Values.Where(cell => cell.GetOccupantUnit() != null)) {
                this.HighlightUnit(cell);
            }
        }

        public void UnHighlightUnits() {
            foreach (MapCell cell in this._cells.Values.Where(cell => cell.GetOccupantUnit() != null)) {
                cell.UnHighlightCell();
            }
        }

        public void HighlightUnit(GridPosition position) {
            MapCell cell = this._cells.GetValueOrDefault(position);

            if (cell?.GetOccupantUnit() == null) {
                return;
            }

            this.HighlightUnit(cell);
        }

        private void HighlightUnit(MapCell cell) {
            HighlightColor color = cell.GetOccupantUnit().Team.GetBattleTeam() == BattleTeam.Player
                ? HighlightColor.Yellow
                : HighlightColor.Orange;

            cell.HighlightCell(color);
        }
    }
}
