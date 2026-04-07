namespace Game.Battle {
    using System;
    using System.Collections.Generic;
    using Actions;
    using Map;
    using Map.Battle;
    using UI;
    using Unit;
    using UnityEngine;

    public class TurnManager : MonoBehaviour, IBattleContext {
        [SerializeField] private UnitActionPanelUI unitActionPanelUI;
        [SerializeField] private StaticMapLoader mapLoader;

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
        }

        public void EnterAttackTargetSelection() => throw new NotImplementedException();

        public void EndTurn() => this.unitActionPanelUI.Hide();

        public void EnterObjectSelection() => throw new NotImplementedException();

        public void EnterSkillSelection() => throw new NotImplementedException();

        public void StartTurn(UnitObject unit) {
            this._unitTurnObject = unit;
            this._unitTurnState = new UnitTurnState(unit);
            this.unitActionPanelUI.Show();
        }

        /**
         * Called from UI
         */
        public void DoAction(ActionType actionType) => this._actions[actionType].Start(this._unitTurnState, this);

        public bool CanDoAction(ActionType actionType) => this._actions[actionType].CanBeExecuted(this._unitTurnState);
    }
}
