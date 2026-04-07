namespace Game.Battle {
    using System.Collections.Generic;
    using Actions;
    using Unit;

    public class UnitTurnState {
        private readonly HashSet<ActionType> _actionsDone = new();

        private readonly UnitObject _unitObject;

        public UnitTurnState(UnitObject unitObject) => this._unitObject = unitObject;

        public bool CanDoAction(IBattleAction action) => !this._actionsDone.Contains(action.GetActionType()) &&
                                                         this._unitObject.CanUseAp(action.GetApCost());
    }
}
