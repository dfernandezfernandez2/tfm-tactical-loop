namespace Game.Battle.Actions {
    public enum ActionType {
        Movement,
        Attack,
        Skill,
        Item,
        Wait
    }

    public static class ActionTypeExtensions {
        public static IBattleAction GetBattleAction(this ActionType actionType) =>
            actionType switch {
                ActionType.Movement => new MovementSelectionAction(),
                ActionType.Attack => new AttackSelectionAction(),
                ActionType.Skill => new SkillSelectionAction(),
                ActionType.Item => new ItemSelectionAction(),
                ActionType.Wait => new WaitAction(),
                _ => null
            };

        public static string GetName(this ActionType actionType) =>
            actionType switch {
                ActionType.Movement => "Movement",
                ActionType.Attack => "Attack",
                ActionType.Skill => "Skill",
                ActionType.Item => "Item",
                ActionType.Wait => "Wait",
                _ => null
            };
    }
}
