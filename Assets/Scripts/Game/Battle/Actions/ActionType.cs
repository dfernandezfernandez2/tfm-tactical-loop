namespace Game.Battle.Actions {
    using Translation;

    public enum ActionType {
        Movement,
        Attack,
        Skill,
        Item,
        Wait
    }

    public static class ActionTypeExtensions {
        public static string GetName(this ActionType actionType) =>
            TranslatorManager.Get($"battle.action.{actionType.ToString().ToLower()}.name");

        public static string GetActionId(this ActionType actionType) =>
            actionType switch {
                ActionType.Movement => "MovementAction",
                ActionType.Attack => "AttackAction",
                ActionType.Skill => "SkillAction",
                ActionType.Item => "ItemAction",
                ActionType.Wait => "WaitAction",
                _ => null
            };
    }
}
