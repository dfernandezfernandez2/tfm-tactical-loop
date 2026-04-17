namespace Game.Battle {
    using System.Collections.Generic;
    using Actions;
    using Unit;

    public class UnitTurnState {
        private readonly HashSet<string> _actionsDone = new();

        private readonly UnitObject _unitObject;
        private IBattleAction _lastAction;

        public UnitTurnState(UnitObject unitObject) => this._unitObject = unitObject;

        public bool CanDoAction(IBattleAction action) => !this._actionsDone.Contains(action.GetId()) &&
                                                         action.CanDoAction(this._unitObject);

        public void ExecuteAction(IBattleAction action, IBattleContext battleContext) {
            this._actionsDone.Add(action.GetId());
            battleContext.ApCostApply(action);
            action.Start(battleContext);
            this._lastAction = action;
        }

        public void CancelLastAction(IBattleContext battleContext) {
            battleContext.ApCostRevert(this._lastAction);
            this._actionsDone.Remove(this._lastAction.GetId());
            this._lastAction = null;
        }
    }
}
