namespace Game.Battle.Actions {
    public class AttackAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Attack;
        public int GetApCost() => 1;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterAttackTargetSelection();
    }
}
