namespace Game.Battle {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Actions;
    using Core;
    using Core.Data;
    using IA;
    using Map.Battle;
    using Map.Battle.Data;
    using UI;
    using Unit;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class TurnManager : MonoBehaviour, IBattleContext {
        [SerializeField] private UnitActionPanelUI unitActionPanelUI;
        [SerializeField] private UnitInfoPanelUI unitInfoPanelUI;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private UserSelectionManager userSelectionManager;
        [SerializeField] private TurnOrderUI turnOrderUI;

        private readonly List<UnitObject> _unitsTurnOrder = new();
        private EnemyTurnController _enemyTurnController;
        private int _unitsTurnOrderIndex;
        private UnitTurnState _unitTurnState;

        public void Awake() => this._enemyTurnController = new EnemyTurnController(this.battleMapManager);

        /**
         * Communicates to UI
         */
        public void EnterMovementSelection() {
            int currentUnitMovement = this._unitsTurnOrder[this._unitsTurnOrderIndex].GetUnit().GetCurrentMovement();
            GridPosition currentUnitGridPosition =
                this._unitsTurnOrder[this._unitsTurnOrderIndex].GetUnit().GetGridPosition();
            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, currentUnitMovement);
            this.userSelectionManager.OnSelect +=
                position => this.StartCoroutine(this.HandleMovementSelection(position));
            this.userSelectionManager.OnCancel += this.HandleCancelAction;
            this.userSelectionManager.StartSelection(SelectionType.Movement, reachableTiles, currentUnitGridPosition);
        }

        public void EnterAttackTargetSelection() {
            int attackRange = this._unitsTurnOrder[this._unitsTurnOrderIndex].GetUnit().GetAttackRange();
            GridPosition currentUnitGridPosition =
                this._unitsTurnOrder[this._unitsTurnOrderIndex].GetUnit().GetGridPosition();
            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, attackRange, false);
            this.userSelectionManager.OnSelect +=
                position => this.StartCoroutine(this.HandleAttackSelected(position));
            this.userSelectionManager.OnCancel += this.HandleCancelAction;
            this.userSelectionManager.StartSelection(SelectionType.Attack, reachableTiles, currentUnitGridPosition);
        }

        public void EndTurn() => this.StartCoroutine(this.StartTurn());

        public void EnterObjectSelection() {
            this.unitActionPanelUI.Init(RunData.GetInstance().Inventory.Items.ToList().AsReadOnly());
            this.unitActionPanelUI.OnBack += () => {
                this.unitActionPanelUI.Init(this._unitsTurnOrder[Math.Max(this._unitsTurnOrderIndex, 0)]
                    .GetBasicActions());
                this.unitActionPanelUI.Show();
            };
            this.unitActionPanelUI.Show();
        }

        public void EnterSkillSelection() {
            UnitObject turnUnit = this._unitsTurnOrder[Math.Max(this._unitsTurnOrderIndex, 0)];
            this.unitActionPanelUI.Init(turnUnit.GetSkillActions());
            this.unitActionPanelUI.OnBack += () => {
                this.unitActionPanelUI.Init(turnUnit.GetBasicActions());
                this.unitActionPanelUI.Show();
            };
            this.unitActionPanelUI.Show();
        }

        public void ApCostApply(IBattleAction action) =>
            this._unitsTurnOrder[this._unitsTurnOrderIndex].GetUnit().UseAp(action.GetApCost());

        public void ApCostRevert(IBattleAction action) =>
            this._unitsTurnOrder[this._unitsTurnOrderIndex].GetUnit().RecoverAp(action.GetApCost());

        public void StartMap(Team playerTeam, Team enemyTeam) {
            this.BuildTurnOrder(playerTeam, enemyTeam);
            this.StartCoroutine(this.StartTurn());
        }

        public void EndMap() {
            this.unitActionPanelUI.Hide();
            this.turnOrderUI.Hide();
            this.turnOrderUI.Reset();
            this._unitsTurnOrder.Clear();
            this._unitsTurnOrderIndex = -1;
        }

        private void BuildTurnOrder(Team playerTeam, Team enemyTeam) {
            this._unitsTurnOrder.Clear();
            this._unitsTurnOrderIndex = -1;
            List<UnitObject> units = playerTeam.GetUnitObjects().Concat(enemyTeam.GetUnitObjects()).ToList();
            this._unitsTurnOrder.AddRange(
                units.OrderByDescending(unit => unit.GetUnit().GetSpeed())
                    .ThenBy(_ => Random.value)
                    .ToList());
            this.turnOrderUI.Show(this._unitsTurnOrder, 5);
        }

        private IEnumerator HandleMovementSelection(GridPosition target) {
            GridPosition currentUnitGridPosition =
                this._unitsTurnOrder[this._unitsTurnOrderIndex].GetUnit().GetGridPosition();
            IReadOnlyList<GridPosition> path =
                this.battleMapManager.FindPath(currentUnitGridPosition, target);
            yield return this.StartCoroutine(BattleSequenceExecutor.ExecuteMovement(
                this._unitsTurnOrder[this._unitsTurnOrderIndex], path,
                (position, gridPosition) => this.battleMapManager.UnitMove(position, gridPosition, true)));

            this.unitActionPanelUI.Show();
        }

        private IEnumerator HandleAttackSelected(GridPosition target) {
            UnitObject targetUnit = this.battleMapManager.GetUnit(target);
            yield return this.StartCoroutine(
                BattleSequenceExecutor.ExecuteBasicAttack(this._unitsTurnOrder[this._unitsTurnOrderIndex], targetUnit,
                    target, this.battleMapManager)
            );
            this.unitActionPanelUI.Show();
        }

        private void HandleCancelAction() {
            this._unitTurnState.CancelLastAction(this);
            this.unitActionPanelUI.Show();
        }

        private IEnumerator StartTurn() {
            this.unitActionPanelUI.Hide();
            this.battleMapManager.UnSelect(this._unitsTurnOrder[Math.Max(this._unitsTurnOrderIndex, 0)].GetUnit()
                .GetGridPosition());
            this._unitsTurnOrderIndex = this.GetNextUnitTurnOrderIndex(this._unitsTurnOrderIndex);
            UnitObject currentTurnUnit = this._unitsTurnOrder[this._unitsTurnOrderIndex];
            currentTurnUnit.GetUnit().RestoreAp();
            this._unitTurnState = new UnitTurnState(currentTurnUnit);
            this.turnOrderUI.UpdateCurrentTurn(this._unitsTurnOrderIndex);
            this.unitInfoPanelUI.SetUnitInfo(currentTurnUnit);
            this.battleMapManager.Select(currentTurnUnit.GetUnit().GetGridPosition());
            if (currentTurnUnit.GetTeam().GetBattleTeam().Equals(BattleTeam.Enemy)) {
                IReadOnlyList<DecisionResult> decisionResults = this._enemyTurnController.CalculateTurn(currentTurnUnit,
                    this._unitsTurnOrder, currentTurnUnit.GetAllAvailableActions());
                foreach (DecisionResult decisionResult in decisionResults) {
                    yield return decisionResult.Action.DoEnemyAction(this, currentTurnUnit, decisionResult,
                        this.battleMapManager);
                }
            }
            else {
                this.unitActionPanelUI.Init(currentTurnUnit.GetBasicActions());
                this.unitActionPanelUI.Show();
            }

            yield return null;
        }

        private int GetNextUnitTurnOrderIndex(int currentIndex) {
            bool allDead = this._unitsTurnOrder.All(unit => unit.GetUnit().IsDead());
            if (allDead) {
                return -1; // should never happen
            }

            int nextIndex = (currentIndex + 1) % this._unitsTurnOrder.Count;
            return this._unitsTurnOrder[nextIndex].GetUnit().IsDead()
                ? this.GetNextUnitTurnOrderIndex(nextIndex)
                : nextIndex;
        }

        /**
         * Called from UI
         */
        public void DoAction(IBattleAction battleAction) {
            this.unitActionPanelUI.Hide();
            this._unitTurnState.ExecuteAction(battleAction, this);
        }

        public bool CanDoAction(IBattleAction battleAction) => this._unitTurnState.CanDoAction(battleAction);
    }
}
