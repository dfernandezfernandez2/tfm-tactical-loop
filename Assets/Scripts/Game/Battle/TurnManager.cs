namespace Game.Battle {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Actions;
    using Map.Battle;
    using Map.Battle.Data;
    using UI;
    using Unit;
    using UnityEngine;

    public class TurnManager : MonoBehaviour, IBattleContext {
        [SerializeField] private UnitActionPanelUI unitActionPanelUI;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private UserSelectionManager userSelectionManager;

        private readonly Dictionary<ActionType, IBattleAction> _actions = new();

        private UnitObject _unitTurnObject;
        private UnitTurnState _unitTurnState;

        public void Awake() {
            foreach (ActionType actionType in Enum.GetValues(typeof(ActionType))) {
                this._actions.Add(actionType, actionType.GetBattleAction());
            }
        }

        /**
         * Communicates to UI
         */
        public void EnterMovementSelection() {
            int currentUnitMovement = this._unitTurnObject.GetCurrentMovement();
            GridPosition currentUnitGridPosition = this._unitTurnObject.GetGridPosition();
            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, currentUnitMovement);
            this.userSelectionManager.OnSelect +=
                position => this.StartCoroutine(this.HandleMovementSelection(position));
            this.userSelectionManager.OnCancel += this.HandleCancelAction;
            this.userSelectionManager.StartSelection(SelectionType.Movement, reachableTiles, currentUnitGridPosition);
        }

        public void EnterAttackTargetSelection() => throw new NotImplementedException();

        public void EndTurn() => this.unitActionPanelUI.Hide();

        public void EnterObjectSelection() => throw new NotImplementedException();

        public void EnterSkillSelection() => throw new NotImplementedException();

        public void ApCostApply(IBattleAction action) => this._unitTurnObject.UseAp(action.GetApCost());

        public void ApCostRevert(IBattleAction action) => this._unitTurnObject.RecoverAp(action.GetApCost());

        private IEnumerator HandleMovementSelection(GridPosition target) {
            IReadOnlyList<GridPosition> path =
                this.battleMapManager.FindPath(this._unitTurnObject.GetGridPosition(), target);
            yield return this.StartCoroutine(this._unitTurnObject.MoveOnPath(path));
            this.unitActionPanelUI.Show();
        }

        private void HandleCancelAction() {
            this._unitTurnState.CancelLastAction(this);
            this.unitActionPanelUI.Show();
        }

        public void StartTurn(UnitObject unit) {
            this._unitTurnObject = unit;
            this._unitTurnObject.RestoreAp();
            this._unitTurnState = new UnitTurnState(unit);
            this.unitActionPanelUI.Show();
        }

        /**
         * Called from UI
         */
        public void DoAction(ActionType actionType) {
            this.unitActionPanelUI.Hide();
            this._unitTurnState.ExecuteAction(this._actions[actionType], this);
        }

        public bool CanDoAction(ActionType actionType) => this._unitTurnState.CanDoAction(this._actions[actionType]);
    }
}
