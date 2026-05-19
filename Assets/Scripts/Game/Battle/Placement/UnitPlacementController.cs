namespace Game.Battle.Placement {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Controls;
    using Data;
    using Map;
    using Map.Data;
    using Map.Renderer;
    using Selection;
    using UI;
    using UnityEngine;

    public class UnitPlacementController : MonoBehaviour {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private UnitSelectorUI unitSelectorUI;
        private readonly Dictionary<GridPosition, int> _placementsByPosition = new();
        private readonly Dictionary<int, GridPosition> _positionByUnitIndex = new();
        private IReadOnlyList<TileData> _availablePositions;
        private int _currentUnitIndex;
        private Action<GridPosition> _despawnUnit;
        private bool _isInitialized;

        private Action _onPlacementFinish;

        private Team _playerTeam;
        private Action<GridPosition, HighlightColor> _selectUnit;
        private Action<TeamUnit, GridPosition> _spawnUnit;
        private IReadOnlyList<TeamUnit> _unitsPrefabToPlace;
        private Action<GridPosition> _unselectUnit;

        private UserSelector _userSelector;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private WorldRender worldRender;

        private void Awake() {
            this._userSelector =
                new UserSelector(this.mainCamera, this.worldRender, true, this.SelectPosition, this.CancelSelection);
            this.unitSelectorUI.gameObject.SetActive(false);
        }

        private void Update() {
            if (!this._isInitialized) {
                return;
            }

            this._userSelector.Update();
            if (InputUtils.IsSwapNextSelected() || InputUtils.IsScrollUpSelected()) {
                this.MoveOnNextUnitIndex();
            }

            if (InputUtils.IsSwapPreviousSelected() || InputUtils.IsScrollDownSelected()) {
                this.MoveOnPreviousUnitIndex();
            }
        }

        public void Init(Team playerTeam, Action onPlacementFinish, Action<TeamUnit, GridPosition> spawnUnit,
            Action<GridPosition> despawnUnit, Action<GridPosition, HighlightColor> selectUnit,
            Action<GridPosition> unselectUnit) {
            this._playerTeam = playerTeam;
            this._unitsPrefabToPlace = playerTeam.GetTeamUnits();
            this._availablePositions = this.battleMapManager.GetTeamTileSpawns(this._playerTeam.GetBattleTeam());
            this._currentUnitIndex = -1;
            this._placementsByPosition.Clear();
            this._positionByUnitIndex.Clear();
            this._spawnUnit = spawnUnit;
            this._despawnUnit = despawnUnit;
            this._selectUnit = selectUnit;
            this._unselectUnit = unselectUnit;
            this._onPlacementFinish = onPlacementFinish;
            this._userSelector.Init(this._availablePositions, this._availablePositions.First().TileGridPosition);
            this._userSelector.HighlightAvailablePositions(HighlightColor.Green);
            this._isInitialized = true;
            this.unitSelectorUI.gameObject.SetActive(true);
            this.unitSelectorUI.Init(playerTeam.GetTeamUnits().Select(unit => unit.Prefab).ToList().AsReadOnly());
            this.MoveOnNextUnitIndex();
        }

        private void SelectPosition(GridPosition position) {
            // case player re select position for unit, despawn it
            if (this._positionByUnitIndex.ContainsKey(this._currentUnitIndex)) {
                this.CancelSelection();
            }

            // case player selected an occupied positon
            if (this._placementsByPosition.TryGetValue(position, out int index)) {
                this.CancelSelection(index);
            }

            TeamUnit teamUnit = this._unitsPrefabToPlace[this._currentUnitIndex];
            this._positionByUnitIndex.Add(this._currentUnitIndex, position);
            this._placementsByPosition.Add(position, this._currentUnitIndex);
            this._spawnUnit(teamUnit, position);
            this.MoveOnNextUnitIndex();
            this._selectUnit(position, this._playerTeam.GetBattleTeam().GetHighlightColor());
            // case last unit has selected
            if (this._placementsByPosition.Count == this._unitsPrefabToPlace.Count) {
                this._userSelector.UnhighlightAvailablePositions();
                this._isInitialized = false;
                this.unitSelectorUI.End();
                this.unitSelectorUI.gameObject.SetActive(false);
                this._onPlacementFinish.Invoke();
            }
        }

        private void MoveOnNextUnitIndex() => this.MoveUnitIndex(1);

        private void MoveOnPreviousUnitIndex() => this.MoveUnitIndex(-1);

        private void MoveUnitIndex(int direction) {
            this.unitSelectorUI.SetSelected(this._currentUnitIndex, false);
            this._currentUnitIndex =
                (this._currentUnitIndex + direction + this._unitsPrefabToPlace.Count) % this._unitsPrefabToPlace.Count;
            this.unitSelectorUI.SetSelected(this._currentUnitIndex, true);
        }

        private void CancelSelection() => this.CancelSelection(this._currentUnitIndex);

        private void CancelSelection(int index) {
            if (!this._positionByUnitIndex.TryGetValue(index, out GridPosition position)) {
                return;
            }

            this._unselectUnit.Invoke(position);
            this._despawnUnit(position);
            this._positionByUnitIndex.Remove(index);
            this._placementsByPosition.Remove(position);
        }
    }
}
