namespace Game.Core {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Map.Battle;
    using Map.Battle.Data;
    using Map.Battle.Renderer;
    using UnityEngine;

    public class UserSelector {
        private readonly bool _enableCancel;
        private readonly Camera _mainCamera;
        private readonly Action _onCancel;
        private readonly Action<GridPosition> _onSelect;
        private readonly WorldRender _worldRender;
        private IReadOnlyList<TileData> _availablePositions;

        private TileData _currentTileData;
        private GridPosition _currentUnitPosition;

        private Vector3 _lastClickPosition;
        private List<GridPosition> _reachablePositions;

        public UserSelector(Camera mainCamera, WorldRender worldRender, bool enableCancel,
            Action<GridPosition> onSelect, Action onCancel) {
            this._mainCamera = mainCamera;
            this._worldRender = worldRender;
            this._enableCancel = enableCancel;
            this._onSelect = onSelect;
            this._onCancel = onCancel;
        }

        public void Init(IReadOnlyList<TileData> availablePositions, GridPosition currentUnitPosition) {
            this._lastClickPosition = InputUtils.GetClickPosition();
            this._availablePositions = availablePositions;
            this._currentUnitPosition = currentUnitPosition;
            this._currentTileData = null;
            this._reachablePositions = this._availablePositions.Select(tileData => tileData.TileGridPosition).ToList();
        }

        public void Update() {
            this.HandleMovementSelection();
            this.HandleClickMovement();
            this.HandleConfirm();
            if (this._enableCancel) {
                this.HandleCancel();
            }
        }

        public void HighlightAvailablePositions(HighlightColor color) {
            foreach (TileData tileData in this._availablePositions) {
                tileData.TileView.Highlight(color);
            }
        }

        public void UnhighlightAvailablePositions() {
            foreach (TileData tileData in this._availablePositions) {
                tileData.TileView.Unhighlight();
            }
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

        private Vector2Int GetPositionFromClick() {
            Vector3 clickPosition = InputUtils.GetClickPosition();
            Vector3 mouseWorld = this._mainCamera.ScreenToWorldPoint(clickPosition);
            return this._worldRender.WorldToGrid(mouseWorld);
        }

        private void HandleConfirm() {
            if (InputUtils.IsEnterClickSelected() || InputUtils.IsEnterSelected()) {
                this._onSelect(this._currentUnitPosition);
            }
        }

        private void HandleCancel() {
            if (InputUtils.IsCancelSelected()) {
                this._onCancel();
            }
        }
    }
}
