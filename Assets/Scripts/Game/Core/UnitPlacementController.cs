namespace Game.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Map.Battle;
    using Map.Battle.Data;
    using Map.Battle.Renderer;
    using Unit;
    using UnityEngine;

    public class UnitPlacementController : MonoBehaviour {
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private WorldRender worldRender;

        private UserSelector _userSelector;

        private Team _playerTeam;
        private IReadOnlyList<TileData> _availablePositions;
        private IReadOnlyList<UnitObject> _unitsPrefabToPlace;
        private Dictionary<GridPosition, UnitObject> _placements = new();
        private int _currentUnitIndex;
        private bool _isInitialized;

        private Action _onPlacementFinish;
        private Action<UnitObject, GridPosition> _spawnUnit;
        private Action<GridPosition> _despawnUnit;

        private void Awake() => this._userSelector = new UserSelector(this.mainCamera, this.worldRender, false, this.SelectPosition, null);

        private void Update() {
            if (!this._isInitialized) {
                return;
            }
            this._userSelector.Update();
        }

        public void Init(Team playerTeam, Action onPlacementFinish, Action<UnitObject, GridPosition> spawnUnit, Action<GridPosition> despawnUnit) {
            this._playerTeam = playerTeam;
            this._unitsPrefabToPlace = playerTeam.GetUnitObjectsPrefabs();
            this._availablePositions = this.battleMapManager.GetTeamTileSpawns(this._playerTeam.GetBattleTeam());
            this._currentUnitIndex = 0;
            this._placements.Clear();
            this._spawnUnit = spawnUnit;
            this._despawnUnit = despawnUnit;
            this._onPlacementFinish = onPlacementFinish;
            this._userSelector.Init(this._availablePositions, this._availablePositions.First().TileGridPosition);
            this._userSelector.HighlightAvailablePositions(HighlightColor.Green);
            this._isInitialized = true;
        }

        private void SelectPosition(GridPosition position) {
            if (this._placements.ContainsKey(position)) {
                this._despawnUnit(position);
                this._placements.Remove(position);
            }

            UnitObject unitPrefab = this._unitsPrefabToPlace[this._currentUnitIndex];
            this._placements.Add(position, unitPrefab);
            this._spawnUnit(unitPrefab, position);
            this.MoveOnNextUnitIndex();
            if (this._placements.Keys.Count == this._unitsPrefabToPlace.Count) {
                this._userSelector.UnhighlightAvailablePositions();
                this._isInitialized = false;
                this._onPlacementFinish.Invoke();
            }
        }

        private void MoveOnNextUnitIndex() => this._currentUnitIndex  = (this._currentUnitIndex + 1) % this._unitsPrefabToPlace.Count;
        private void MoveOnPreviousUnitIndex() => this._currentUnitIndex = (this._currentUnitIndex - 1 + this._unitsPrefabToPlace.Count) % this._unitsPrefabToPlace.Count;
    }
}
