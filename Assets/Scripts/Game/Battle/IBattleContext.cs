namespace Game.Battle {
    using System;
    using Actions;
    using Item;
    using Map.Battle;
    using Unit;

    public interface IBattleContext {
        public void EnterMovementSelection();
        public void EnterAttackTargetSelection();
        public void EndTurn();
        public void EnterObjectSelection();
        public void EnterSkillSelection();
        public void ApCostApply(IBattleAction action);
        public void ApCostRevert(IBattleAction action);

        public void EnterItemSelectionTarget(Target target,
            Action<UnitObject, GridPosition, BattleMapManager, IBattleContext> callback,
            Func<UnitObject, bool> canSelect);

        public void EndAction();
        public bool IsAvailableAction(string actionName);
    }
}
