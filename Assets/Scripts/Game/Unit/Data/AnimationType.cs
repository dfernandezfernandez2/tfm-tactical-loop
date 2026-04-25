namespace Game.Unit.Data {
    public enum AnimationType {
        Attack,
        Damage,
        Death,
        Dodge
    }

    public static class AnimationTypeExtensions {
        public static string GetAnimationEndName(this AnimationType animationType) =>
            "signal.end." + animationType.ToString().ToLower();

        public static string GetAnimationText(this AnimationType animationType) =>
            "signal.text." + animationType.ToString().ToLower();
    }
}
