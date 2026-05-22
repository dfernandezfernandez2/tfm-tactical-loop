namespace Game.Battle.Selection {
    using System;
    using System.Collections.Generic;
    using Map;
    using Map.Data;
    using Map.Renderer;
    using UI;
    using Unit;
    using UnityEngine;

    public enum SelectionType {
        Attack,
        Movement,
        Default,
        None
    }

    public static class SelectionTypeExtensions {
        public static HighlightColor GetHighlightColor(this SelectionType selectionType) =>
            selectionType switch {
                SelectionType.Attack => HighlightColor.Red,
                SelectionType.Movement => HighlightColor.Blue,
                SelectionType.Default => HighlightColor.Green,
                _ => HighlightColor.None
            };
    }

    public class UserSelectionManager : MonoBehaviour {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private WorldRender worldRender;
        [SerializeField] private UnitInfoPanelUI unitInfoPanel;

        private IReadOnlyCollection<TileData> _availablePositions;
        private SelectionType _currentSelectionType = SelectionType.None;
        private List<GridPosition> _reachablePositions;
        private GridPosition _startPosition;
        private UserSelector _userSelector;

        private void Awake() => this._userSelector = new UserSelector(this.mainCamera, this.worldRender, true,
            this.SelectPosition, this.CancelSelection, this.OnMoveSelection);

        private void Update() {
            if (this._currentSelectionType == SelectionType.None) {
                return;
            }

            this._userSelector.Update();
        }

        public event Action<GridPosition> OnSelect;
        public event Action OnCancel;

        public void StartSelection(SelectionType selectionType, IReadOnlyList<TileData> availablePositions,
            GridPosition currentUnitPosition) {
            this._currentSelectionType = selectionType;
            this._startPosition = currentUnitPosition;
            this._userSelector.Init(availablePositions, currentUnitPosition);
            this._userSelector.HighlightAvailablePositions(this._currentSelectionType.GetHighlightColor());
        }

        private void EndSelection() {
            this.unitInfoPanel.gameObject.SetActive(false);
            this._userSelector.UnhighlightAvailablePositions();
            this._currentSelectionType = SelectionType.None;
        }

        private void SelectPosition(GridPosition position) {
            if (Equals(this._startPosition, position)) {
                return;
            }

            this.EndSelection();
            this.OnSelect?.Invoke(position);
            this.ClearEvents();
        }

        private void CancelSelection() {
            this.unitInfoPanel.gameObject.SetActive(false);
            this.EndSelection();
            this.OnCancel?.Invoke();
            this.ClearEvents();
        }

        private void ClearEvents() {
            this.OnSelect = null;
            this.OnCancel = null;
        }

        private void OnMoveSelection(GridPosition position) {
            UnitObject unitObject = this.battleMapManager.GetUnit(position);
            if (unitObject != null) {
                this.unitInfoPanel.gameObject.SetActive(true);
                this.unitInfoPanel.SetUnitInfo(unitObject);
            }
            else {
                this.unitInfoPanel.gameObject.SetActive(false);
            }
        }
    }
}
