namespace Game.Map.Battle {
    using System.Collections.Generic;
    using Core;
    using Data;
    using Renderer;
    using Unit;
    using UnityEngine;

    public class BattleMapManager : MonoBehaviour {
        private readonly Dictionary<GridPosition, MapCell> _cells = new();
        private BattleMapHighlighter _highlighter;
        private bool _isTransparencyViewActive;
        private BattleMapData _mapData;
        private BattlePathfinder _pathfinder;
        private BattleMapQueryService _queryService;
        private BattleRangeFinder _rangeFinder;
        private BattleMapUnitRegistry _unitRegistry;
        private BattleMapViewManager _viewManager;

        private void Update() {
            if (this._mapData == null) {
                return;
            }

            this.HandleTransparencyViewInput();
        }

        public void Initialize(BattleMapData mapData) {
            this._mapData = mapData;
            this._cells.Clear();

            this._mapData.ForEach(data =>
                this._cells[data.TileGridPosition] = new MapCell(data));

            this._queryService = new BattleMapQueryService(this._mapData, this._cells);
            this._unitRegistry = new BattleMapUnitRegistry(this._cells);
            this._highlighter = new BattleMapHighlighter(this._cells);
            this._rangeFinder = new BattleRangeFinder(this._mapData, this._cells, this._queryService);
            this._pathfinder = new BattlePathfinder(this._mapData, this._queryService);
            this._viewManager = new BattleMapViewManager(this._cells);
        }

        public GridPosition GetMapCenterPosition() => this._mapData.GetCenter();

        public void InitUnit(UnitObject unitObject) =>
            this._unitRegistry.InitUnit(unitObject);

        public void DespawnUnit(UnitObject unitObject) =>
            this._unitRegistry.DespawnUnit(unitObject);

        public void UnitMove(GridPosition from, GridPosition to, bool select) {
            this._unitRegistry.MoveUnit(from, to);
            this._highlighter.UnHighlight(from);
            this._highlighter.HighlightUnit(to);

            if (select) {
                this._highlighter.Select(to);
            }
        }

        public UnitObject GetUnit(GridPosition position) =>
            this._unitRegistry.GetUnit(position);

        public IReadOnlyList<UnitObject> GetUnitsAround(GridPosition position) =>
            this._unitRegistry.GetUnitsAround(position, this._mapData);

        public IReadOnlyList<TileData> GetReachableTiles(GridPosition origin, TileSearchConfig config) =>
            this._rangeFinder.GetReachableTiles(origin, config);

        public IReadOnlyList<GridPosition> FindPath(GridPosition origin, GridPosition target) =>
            this._pathfinder.FindPath(origin, target);

        public bool IsAvailablePosition(GridPosition position) =>
            this._queryService.IsAvailablePosition(position);

        public IReadOnlyList<TileData> GetTeamTileSpawns(BattleTeam team) =>
            this._mapData.GetTeamSpawns(team);

        public void Highlight(GridPosition position, HighlightColor color) =>
            this._highlighter.Highlight(position, color);

        public void UnHighlight(GridPosition position) =>
            this._highlighter.UnHighlight(position);

        public void Select(GridPosition position) =>
            this._highlighter.Select(position);

        public void UnSelect(GridPosition position) =>
            this._highlighter.UnSelect(position);

        public void HighlightUnits() =>
            this._highlighter.HighlightUnits();

        public void UnHighlightUnits() =>
            this._highlighter.UnHighlightUnits();

        private void HandleTransparencyViewInput() {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
                this.ApplyTransparencyView();
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) {
                this.ApplyDefaultView();
            }
        }

        private void ApplyTransparencyView() {
            if (this._isTransparencyViewActive) {
                return;
            }

            this._isTransparencyViewActive = true;
            this._viewManager.ApplyTransparencyView();
        }

        private void ApplyDefaultView() {
            if (!this._isTransparencyViewActive) {
                return;
            }

            this._isTransparencyViewActive = false;
            this._viewManager.ApplyDefaultView();
        }
    }
}
