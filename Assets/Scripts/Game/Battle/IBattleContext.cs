namespace Game.Battle {
    using System;
    using System.Collections;
    using Actions;
    using Map.Data;
    using Selection;

    public interface IBattleContext {
        public void EnterMovementSelection();
        public void EnterAttackTargetSelection();
        public void EndTurn();
        public void EnterObjectSelection();
        public void EnterSkillSelection();
        public void ApCostApply(IBattleAction action);
        public void ApCostRevert(IBattleAction action);

        public IEnumerator EnterSelectionTarget(TileSearchConfig config, Func<SelectionData, IEnumerator> callback,
            SelectionType selectionType = SelectionType.Default);

        public void EndAction();
        public bool IsAvailableAction(string actionName);
    }
}
