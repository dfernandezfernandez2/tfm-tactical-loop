namespace Game.Battle {
    using System.Collections.Generic;
    using Actions;
    using Unit;

    public class UnitTurnState {
        private readonly HashSet<string> _actionsDone = new();

        private readonly UnitObject _unitObject;
        private IBattleAction _lastAction;

        public UnitTurnState(UnitObject unitObject) => this._unitObject = unitObject;

        public bool CanDoAction(IBattleAction action, IBattleContext battleContext) =>
            !this._actionsDone.Contains(action.GetActionName()) &&
            action.CanDoAction(battleContext, this._unitObject);

        public bool CanDoAction(string actionName) => !this._actionsDone.Contains(actionName);

        public void ExecuteAction(IBattleAction action, IBattleContext battleContext) {
            this._actionsDone.Add(action.GetActionName());
            battleContext.ApCostApply(action);
            action.Start(battleContext);
            this._lastAction = action;
        }

        public void CancelLastAction(IBattleContext battleContext) {
            battleContext.ApCostRevert(this._lastAction);
            this._actionsDone.Remove(this._lastAction.GetActionName());
            this._lastAction = null;
        }

        public void CancelAction(IBattleContext battleContext, IBattleAction action) {
            battleContext.ApCostRevert(action);
            this._actionsDone.Remove(action.GetActionName());
            if (action == this._lastAction) {
                this._lastAction = null;
            }
        }
    }
}
