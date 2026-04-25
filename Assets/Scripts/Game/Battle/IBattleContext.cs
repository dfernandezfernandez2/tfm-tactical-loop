namespace Game.Battle {
    using System;
    using System.Collections;
    using Actions;
    using Item;
    using Unit;

    public interface IBattleContext {
        public void EnterMovementSelection();
        public void EnterAttackTargetSelection();
        public void EndTurn();
        public void EnterObjectSelection();
        public void EnterSkillSelection();
        public void ApCostApply(IBattleAction action);
        public void ApCostRevert(IBattleAction action);

        public IEnumerator EnterItemSelectionTarget(Target target,
            Func<InventorySelectionData, IEnumerator> callback,
            Func<UnitObject, bool> canSelect);

        public void EndAction();
        public bool IsAvailableAction(string actionName);
    }
}
