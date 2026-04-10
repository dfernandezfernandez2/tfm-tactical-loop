namespace Game.Battle.Actions {
    public class SkillAction : IBattleAction {
        public ActionType GetActionType() => ActionType.Skill;
        public int GetApCost() => 0;

        public void Start(IBattleContext battleContext) =>
            battleContext.EnterSkillSelection();
    }
}
