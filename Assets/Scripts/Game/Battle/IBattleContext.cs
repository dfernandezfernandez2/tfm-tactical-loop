namespace Game.Battle {
    using Actions;

    public interface IBattleContext {
        public void EnterMovementSelection();
        public void EnterAttackTargetSelection();
        public void EndTurn();
        public void EnterObjectSelection();
        public void EnterSkillSelection();
        public void ApCostApply(IBattleAction action);
        public void ApCostRevert(IBattleAction action);
    }
}
