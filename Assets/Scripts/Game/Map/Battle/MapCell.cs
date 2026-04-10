namespace Game.Map.Battle {
    using Data;
    using Renderer;
    using Unit;

    public class MapCell {
        private readonly TileData _tileData;
        private UnitObject _occupantUnit;

        public MapCell(TileData tileData) => this._tileData = tileData;

        private bool IsOccupied() => this._occupantUnit != null;
        public bool IsWalkable() => this._tileData.Type.IsWalkable() && !this.IsOccupied();
        public UnitObject GetOccupantUnit() => this._occupantUnit;
        public void SetOccupantUnit(UnitObject unit) => this._occupantUnit = unit;
        public void ClearOccupantUnit() => this._occupantUnit = null;
        public void HighlightCell(HighlightColor color) => this._tileData?.TileView.Highlight(color);
        public void UnHighlightCell() => this._tileData?.TileView.Unhighlight();
    }
}
