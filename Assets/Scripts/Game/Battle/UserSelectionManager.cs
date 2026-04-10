namespace Game.Battle {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Map.Battle;
    using Map.Battle.Data;
    using Map.Battle.Renderer;
    using UnityEngine;

    public enum SelectionType {
        Attack,
        Movement,
        None
    }

    public static class SelectionTypeExtensions {
        public static HighlightColor GetHighlightColor(this SelectionType selectionType) =>
            selectionType switch {
                SelectionType.Attack => HighlightColor.Red,
                SelectionType.Movement => HighlightColor.Blue,
                _ => HighlightColor.None
            };
    }

    public class UserSelectionManager : MonoBehaviour {
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private WorldRender worldRender;
        private IReadOnlyCollection<TileData> _availablePositions;

        private SelectionType _currentSelectionType = SelectionType.None;
        private TileData _currentTileData;
        private GridPosition _currentUnitPosition;
        private Vector3 _lastClickPosition;
        private List<GridPosition> _reachablePositions;
        private GridPosition _startPosition;

        private void Update() {
            if (this._currentSelectionType == SelectionType.None) {
                return;
            }

            this.HandleMovementSelection();
            this.HandleClickMovement();
            this.HandleConfirm();
            this.HandleCancel();
        }

        public event Action<GridPosition> OnSelect;
        public event Action OnCancel;

        public void StartSelection(SelectionType selectionType, IReadOnlyCollection<TileData> availablePositions,
            GridPosition currentUnitPosition) {
            this._currentSelectionType = selectionType;
            this._startPosition = currentUnitPosition;
            this._currentUnitPosition = currentUnitPosition;
            this._availablePositions = availablePositions;
            this._reachablePositions = new List<GridPosition>();
            foreach (TileData tileData in this._availablePositions) {
                this._reachablePositions.Add(tileData.TileGridPosition);
            }

            this._lastClickPosition = InputUtils.GetClickPosition();

            this.HighlightAvailablePositions();
        }

        private void EndSelection() {
            this.UnhighlightAvailablePositions();
            this._currentSelectionType = SelectionType.None;
        }

        private void HighlightAvailablePositions() {
            foreach (TileData tileData in this._availablePositions) {
                tileData.TileView.Highlight(this._currentSelectionType.GetHighlightColor());
            }
        }

        private void UnhighlightAvailablePositions() {
            foreach (TileData tileData in this._availablePositions) {
                tileData.TileView.Unhighlight();
            }
        }

        private void SelectPosition(GridPosition position) {
            this.EndSelection();
            this.OnSelect?.Invoke(position);
            this.ClearEvents();
        }

        private void CancelSelection() {
            this.EndSelection();
            this.OnCancel?.Invoke();
            this.ClearEvents();
        }

        private void ClearEvents() {
            this.OnSelect = null;
            this.OnCancel = null;
        }

        private void HandleConfirm() {
            if ((InputUtils.IsEnterClickSelected() || InputUtils.IsEnterSelected()) &&
                !Equals(this._startPosition, this._currentUnitPosition)) {
                this.SelectPosition(this._currentUnitPosition);
            }
        }

        private void HandleCancel() {
            if (InputUtils.IsCancelSelected()) {
                this.CancelSelection();
            }
        }

        private Vector2Int GetPositionFromClick() {
            Vector3 clickPosition = InputUtils.GetClickPosition();
            Vector3 mouseWorld = this.mainCamera.ScreenToWorldPoint(clickPosition);
            return this.worldRender.WorldToGrid(mouseWorld);
        }

        private void HandleMovementSelection() {
            Vector2Int direction = InputUtils.GetInputDirection();
            if (direction == Vector2Int.zero) {
                return;
            }

            GridPosition nextPosition = this.FindNextPositionInDirection(direction);
            this.MoveSelected(nextPosition);
        }

        private void MoveSelected(GridPosition nextPosition) {
            this._currentTileData?.TileView.UnSelect();
            this._currentUnitPosition = nextPosition;
            TileData tile = this._availablePositions
                .First(tile => tile.TileGridPosition.Equals(nextPosition));
            tile.TileView.Select();
            this._currentTileData = tile;
        }

        private GridPosition FindNextPositionInDirection(Vector2Int direction) {
            GridPosition current = this._currentUnitPosition;
            return this._reachablePositions
                .OrderBy(candidate => GetCandidateScore(candidate, current, direction))
                .First();
        }

        private static float GetCandidateScore(GridPosition candidate, GridPosition current, Vector2Int direction) {
            if (candidate.Equals(current)) {
                return float.MaxValue;
            }

            Vector2Int delta = candidate.Position - current.Position;
            if (!IsCandidateInDirection(delta, direction)) {
                return float.MaxValue;
            }

            float score = CalculateDirectionScore(delta, direction);
            return score;
        }

        private static bool IsCandidateInDirection(Vector2Int delta, Vector2Int direction) =>
            (delta.x * direction.x) + (delta.y * direction.y) > 0;

        private static float CalculateDirectionScore(Vector2Int delta, Vector2Int direction) {
            int xDistance = Mathf.Abs(delta.x);
            int yDistance = Mathf.Abs(delta.y);
            return IsHorizontalMovement(direction) ? (xDistance * 2f) + yDistance : (yDistance * 2f) + xDistance;
        }

        private static bool IsHorizontalMovement(Vector2Int direction) =>
            direction == Vector2Int.right || direction == Vector2Int.left;

        private void HandleClickMovement() {
            Vector3 clickPosition = InputUtils.GetClickPosition();
            if (clickPosition == this._lastClickPosition) {
                return;
            }

            this._lastClickPosition = clickPosition;
            Vector2Int vectorClickPosition = this.GetPositionFromClick();
            GridPosition gridPosition =
                this._reachablePositions.Find(gridPosition => gridPosition.Position.Equals(vectorClickPosition));
            if (gridPosition == null) {
                return;
            }

            this.MoveSelected(gridPosition);
        }
    }
}
