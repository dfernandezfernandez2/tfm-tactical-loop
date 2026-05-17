namespace Game.Map.Battle {
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Unit;

    public class BattleMapUnitRegistry {
        private readonly Dictionary<GridPosition, MapCell> _cells;

        public BattleMapUnitRegistry(Dictionary<GridPosition, MapCell> cells) => this._cells = cells;

        public void InitUnit(UnitObject unitObject) {
            GridPosition position = unitObject.Unit.GridPosition;
            this._cells[position].SetOccupantUnit(unitObject);
        }

        public void DespawnUnit(UnitObject unitObject) {
            GridPosition position = unitObject.Unit.GridPosition;
            this._cells[position].ClearOccupantUnit();
        }

        public void MoveUnit(GridPosition from, GridPosition to) {
            MapCell originCell = this._cells[from];
            MapCell targetCell = this._cells[to];

            UnitObject unitObject = originCell.GetOccupantUnit();

            originCell.ClearOccupantUnit();
            targetCell.SetOccupantUnit(unitObject);
        }

        public UnitObject GetUnit(GridPosition position) {
            MapCell cell = this._cells.GetValueOrDefault(position);
            return cell?.GetOccupantUnit();
        }

        public IReadOnlyList<UnitObject> GetUnitsAround(GridPosition position, BattleMapData mapData) {
            IReadOnlyCollection<GridPosition> neighbours = mapData.GetNeighbours(position);

            return neighbours
                .Select(neighbour => this._cells.GetValueOrDefault(neighbour))
                .Where(cell => cell != null)
                .Select(cell => cell.GetOccupantUnit())
                .Where(unit => unit != null)
                .ToList()
                .AsReadOnly();
        }
    }
}
