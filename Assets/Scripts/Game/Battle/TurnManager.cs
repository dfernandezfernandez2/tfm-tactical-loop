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
            int currentUnitMovement = this._unitTurnObject.GetUnit().GetCurrentMovement();
            GridPosition currentUnitGridPosition = this._unitTurnObject.GetUnit().GetGridPosition();
            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, currentUnitMovement);
            this.userSelectionManager.OnSelect +=
                position => this.StartCoroutine(this.HandleMovementSelection(position));
            this.userSelectionManager.OnCancel += this.HandleCancelAction;
            this.userSelectionManager.StartSelection(SelectionType.Movement, reachableTiles, currentUnitGridPosition);
        }

        public void EnterAttackTargetSelection() {
            int attackRange = this._unitTurnObject.GetUnit().GetAttackRange();
            GridPosition currentUnitGridPosition = this._unitTurnObject.GetUnit().GetGridPosition();
            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, attackRange, false);
            this.userSelectionManager.OnSelect +=
                position => this.StartCoroutine(this.HandleAttackSelected(position));
            this.userSelectionManager.OnCancel += this.HandleCancelAction;
            this.userSelectionManager.StartSelection(SelectionType.Attack, reachableTiles, currentUnitGridPosition);
        }

        public void EndTurn() => this.unitActionPanelUI.Hide();

        public void EnterObjectSelection() => throw new NotImplementedException();

        public void EnterSkillSelection() => throw new NotImplementedException();

        public void ApCostApply(IBattleAction action) => this._unitTurnObject.GetUnit().UseAp(action.GetApCost());

        public void ApCostRevert(IBattleAction action) => this._unitTurnObject.GetUnit().RecoverAp(action.GetApCost());

        private IEnumerator HandleMovementSelection(GridPosition target) {
            GridPosition currentUnitGridPosition = this._unitTurnObject.GetUnit().GetGridPosition();
            IReadOnlyList<GridPosition> path =
                this.battleMapManager.FindPath(currentUnitGridPosition, target);
            yield return this.StartCoroutine(BattleSequenceExecutor.ExecuteMovement(this._unitTurnObject, path));
            this.battleMapManager.UnitMove(currentUnitGridPosition, target);
            this.unitActionPanelUI.Show();
        }

        private IEnumerator HandleAttackSelected(GridPosition target) {
            UnitObject targetUnit = this.battleMapManager.GetUnit(target);
            yield return this.StartCoroutine(
                BattleSequenceExecutor.ExecuteBasicAttack(this._unitTurnObject, targetUnit)
            );
            this.unitActionPanelUI.Show();
        }

        private void HandleCancelAction() {
            this._unitTurnState.CancelLastAction(this);
            this.unitActionPanelUI.Show();
        }

        public void StartTurn(UnitObject unit) {
            this._unitTurnObject = unit;
            this._unitTurnObject.GetUnit().RestoreAp();
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
