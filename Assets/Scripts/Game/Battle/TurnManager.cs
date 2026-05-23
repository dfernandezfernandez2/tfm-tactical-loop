namespace Game.Battle {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Actions;
    using Data;
    using IA;
    using Item;
    using Map;
    using Map.Data;
    using Run.Data;
    using Selection;
    using UI;
    using Unit;
    using Unit.Data;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class TurnManager : MonoBehaviour, IBattleContext {
        [SerializeField] private UnitActionPanelUI unitActionPanelUI;
        [SerializeField] private UnitInfoPanelUI unitInfoPanelUI;
        [SerializeField] private UserSelectionManager userSelectionManager;
        [SerializeField] private TurnOrderUI turnOrderUI;
        [SerializeField] private BattleMapManager battleMapManager;

        private readonly List<UnitObject> _unitsTurnOrder = new();
        private Team _enemyTeam;
        private EnemyTurnController _enemyTurnController;
        private Team _playerTeam;
        private int _unitsTurnOrderIndex;
        private UnitTurnState _unitTurnState;

        public void Awake() => this._enemyTurnController = new EnemyTurnController(this.battleMapManager);

        /**
         * Communicates to UI
         */
        public void EnterMovementSelection() {
            int currentUnitMovement = this._unitsTurnOrder[this._unitsTurnOrderIndex].Unit
                .GetCurrentIntStat(StatType.Movement);
            GridPosition currentUnitGridPosition =
                this._unitsTurnOrder[this._unitsTurnOrderIndex].Unit.GridPosition;
            TileSearchConfig config = new() {
                Range = currentUnitMovement
            };
            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, config);
            this.userSelectionManager.OnSelect +=
                position => this.StartCoroutine(this.HandleMovementSelection(position));
            this.userSelectionManager.OnCancel += this.HandleCancelAction;
            this.userSelectionManager.StartSelection(SelectionType.Movement, reachableTiles, currentUnitGridPosition);
        }

        public void EnterAttackTargetSelection() {
            int attackRange = this._unitsTurnOrder[this._unitsTurnOrderIndex].Unit
                .GetCurrentIntStat(StatType.Range);
            GridPosition currentUnitGridPosition =
                this._unitsTurnOrder[this._unitsTurnOrderIndex].Unit.GridPosition;
            TileSearchConfig config = new() {
                Range = attackRange,
                CanEnterCheck = false,
                Target = Target.Enemy,
                CanSelect = unit => !unit.Unit.IsDead()
            };
            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, config);
            this.userSelectionManager.OnSelect +=
                position => this.StartCoroutine(this.HandleAttackSelected(position));
            this.userSelectionManager.OnCancel += this.HandleCancelAction;
            this.userSelectionManager.StartSelection(SelectionType.Attack, reachableTiles, currentUnitGridPosition);
        }

        public void EndTurn() => this.StartCoroutine(this.StartTurn());

        public void EnterObjectSelection() {
            this.unitActionPanelUI.Init(RunData.GetInstance().Inventory.Items.ToList().AsReadOnly());
            this.unitActionPanelUI.OnBack += () => {
                this._unitTurnState.CancelAction(this, new ItemSelectionAction());
                this.unitActionPanelUI.Init(this._unitsTurnOrder[Math.Max(this._unitsTurnOrderIndex, 0)]
                    .Actions.GetBasicActions());
                this.unitActionPanelUI.Show();
            };
            this.unitActionPanelUI.Show();
        }

        public void EnterSkillSelection() {
            UnitObject turnUnit = this._unitsTurnOrder[Math.Max(this._unitsTurnOrderIndex, 0)];
            this.unitActionPanelUI.Init(turnUnit.Actions.GetSkillActions());
            this.unitActionPanelUI.OnBack += () => {
                this._unitTurnState.CancelAction(this, new SkillSelectionAction());
                this.unitActionPanelUI.Init(turnUnit.Actions.GetBasicActions());
                this.unitActionPanelUI.Show();
            };
            this.unitActionPanelUI.Show();
        }

        public void ApCostApply(IBattleAction action) =>
            this._unitsTurnOrder[this._unitsTurnOrderIndex].Unit.AddStat(StatType.AP, -action.GetApCost());

        public void ApCostRevert(IBattleAction action) =>
            this._unitsTurnOrder[this._unitsTurnOrderIndex].Unit.AddStat(StatType.AP, action.GetApCost());

        public IEnumerator EnterSelectionTarget(TileSearchConfig config, Func<SelectionData, IEnumerator> callback,
            SelectionType selectionType = SelectionType.Default) {
            UnitObject currentUser = this._unitsTurnOrder[this._unitsTurnOrderIndex];
            GridPosition currentUnitGridPosition = currentUser.Unit.GridPosition;
            SelectionData itemSelectionData = new() {
                User = currentUser,
                Position = currentUnitGridPosition,
                BattleMapManager = this.battleMapManager,
                Context = this
            };
            if (config.Target == Target.Self) {
                yield return callback(itemSelectionData);
                yield break;
            }

            IReadOnlyList<TileData> reachableTiles =
                this.battleMapManager.GetReachableTiles(currentUnitGridPosition, config);

            bool selected = false;
            bool cancelled = false;
            GridPosition selectedPosition = currentUnitGridPosition;

            this.userSelectionManager.OnSelect += OnSelect;
            this.userSelectionManager.OnCancel += OnCancel;
            this.userSelectionManager.StartSelection(selectionType, reachableTiles, currentUnitGridPosition);
            yield return new WaitUntil(() => selected || cancelled);
            if (cancelled) {
                this.HandleCancelAction();
                yield break;
            }

            itemSelectionData.Position = selectedPosition;

            yield return callback(itemSelectionData);
            yield break;

            void OnSelect(GridPosition position) {
                selectedPosition = position;
                selected = true;
            }

            void OnCancel() {
                cancelled = true;
            }
        }

        public void EndAction() {
            UnitObject turnUnit = this._unitsTurnOrder[Math.Max(this._unitsTurnOrderIndex, 0)];
            this.unitActionPanelUI.Init(turnUnit.Actions.GetBasicActions());
            this.unitActionPanelUI.Show();
        }

        public bool IsAvailableAction(string actionName) => this._unitTurnState.CanDoAction(actionName);

        public event Action<BattleResult> OnBattleEnd;

        public void StartMap(Team playerTeam, Team enemyTeam) {
            this._playerTeam = playerTeam;
            this._enemyTeam = enemyTeam;
            this.BuildTurnOrder(playerTeam, enemyTeam);
            this.StartCoroutine(this.StartTurn());
        }

        private void EndMap() {
            this.unitActionPanelUI.Hide();
            this.turnOrderUI.Hide();
            this.turnOrderUI.Reset();
            this._unitsTurnOrder.Clear();
            this._unitsTurnOrderIndex = -1;
            foreach (UnitObject unitObject in this._playerTeam.GetUnitObjects()) {
                Destroy(unitObject.gameObject);
            }

            this._playerTeam.ClearUnitObjects();
            foreach (UnitObject unitObject in this._enemyTeam.GetUnitObjects()) {
                Destroy(unitObject.gameObject);
            }

            this._enemyTeam.ClearUnitObjects();
        }

        private void BuildTurnOrder(Team playerTeam, Team enemyTeam) {
            this._unitsTurnOrder.Clear();
            this._unitsTurnOrderIndex = -1;
            List<UnitObject> units = playerTeam.GetUnitObjects().Concat(enemyTeam.GetUnitObjects()).ToList();
            this._unitsTurnOrder.AddRange(
                units.OrderByDescending(unit => unit.Unit.GetCurrentIntStat(StatType.Speed))
                    .ThenBy(_ => Random.value)
                    .ToList());
            this.turnOrderUI.Show(this._unitsTurnOrder, 5);
        }

        private IEnumerator HandleMovementSelection(GridPosition target) {
            GridPosition currentUnitGridPosition =
                this._unitsTurnOrder[this._unitsTurnOrderIndex].Unit.GridPosition;
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
            if (!this.CheckBattleEnd()) {
                this.unitActionPanelUI.Show();
            }
        }

        private void HandleCancelAction() {
            this._unitTurnState.CancelLastAction(this);
            this.unitActionPanelUI.Show();
        }

        private IEnumerator StartTurn() {
            this.unitActionPanelUI.Hide();

            UnitObject previousUnitTurn = this._unitsTurnOrder[Math.Max(this._unitsTurnOrderIndex, 0)];
            yield return previousUnitTurn.OnTurnEnd();
            if (previousUnitTurn.Unit.IsDead()) {
                // todo: visual en el orden
            }

            if (this.CheckBattleEnd()) {
                yield break;
            }

            this.battleMapManager.UnSelect(previousUnitTurn.Unit.GridPosition);

            this._unitsTurnOrderIndex = this.GetNextUnitTurnOrderIndex(this._unitsTurnOrderIndex);
            UnitObject currentTurnUnit = this._unitsTurnOrder[this._unitsTurnOrderIndex];
            yield return currentTurnUnit.OnTurnStart();
            if (currentTurnUnit.Unit.IsDead()) {
                if (this.CheckBattleEnd()) {
                    yield break;
                }

                yield return this.StartCoroutine(this.StartTurn());
                yield break;
            }

            this._unitTurnState = new UnitTurnState(currentTurnUnit);
            this.turnOrderUI.UpdateCurrentTurn(this._unitsTurnOrderIndex);
            this.unitInfoPanelUI.SetUnitInfo(currentTurnUnit);
            this.battleMapManager.Select(currentTurnUnit.Unit.GridPosition);
            if (currentTurnUnit.Team.GetBattleTeam().Equals(BattleTeam.Enemy)) {
                IReadOnlyList<DecisionResult> decisionResults = this._enemyTurnController.CalculateTurn(currentTurnUnit,
                    this._unitsTurnOrder, currentTurnUnit.Actions.GetAllAvailableActions());
                foreach (DecisionResult decisionResult in decisionResults) {
                    yield return decisionResult.Action.DoEnemyAction(this, currentTurnUnit, decisionResult,
                        this.battleMapManager);
                }
            }
            else {
                this.unitActionPanelUI.Init(currentTurnUnit.Actions.GetBasicActions());
                this.unitActionPanelUI.Show();
            }

            yield return null;
        }

        private int GetNextUnitTurnOrderIndex(int currentIndex) {
            bool allDead = this._unitsTurnOrder.All(unit => unit.Unit.IsDead());
            if (allDead) {
                return -1; // should never happen
            }

            int nextIndex = (currentIndex + 1) % this._unitsTurnOrder.Count;
            return this._unitsTurnOrder[nextIndex].Unit.IsDead()
                ? this.GetNextUnitTurnOrderIndex(nextIndex)
                : nextIndex;
        }

        /**
         * Called from UI
         */
        public void DoAction(IBattleAction battleAction) {
            this.unitActionPanelUI.Hide();
            this.StartCoroutine(Action());
            return;

            IEnumerator Action() {
                yield return this._unitTurnState.ExecuteAction(battleAction, this);
                this.CheckBattleEnd();
            }
        }

        public bool CanDoAction(IBattleAction battleAction) => this._unitTurnState.CanDoAction(battleAction, this);

        private bool CheckBattleEnd() {
            bool playerAlive = this._unitsTurnOrder.Any(unit =>
                unit.Team.GetBattleTeam() == BattleTeam.Player &&
                !unit.Unit.IsDead()
            );
            if (!playerAlive) {
                this.EndMap();
                this.OnBattleEnd?.Invoke(new BattleResult(BattleTeam.Enemy));
                return true;
            }

            bool enemyAlive = this._unitsTurnOrder.Any(unit =>
                unit.Team.GetBattleTeam() == BattleTeam.Enemy &&
                !unit.Unit.IsDead()
            );
            if (!enemyAlive) {
                this.EndMap();
                this.OnBattleEnd?.Invoke(new BattleResult(BattleTeam.Player));
                return true;
            }

            return false;
        }
    }
}
