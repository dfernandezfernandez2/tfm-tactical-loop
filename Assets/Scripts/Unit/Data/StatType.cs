namespace Unit.Data {
    public enum StatType {
        Hp,
        Mp,
        MpRegen,
        Movement,
        AP,
        Atk,
        Def,
        Accuracy,
        Evasion,
        CritChance,
        Range,
        Speed
    }

    public static class StatTypeExtensions {
        public static float DefaultValue(this StatType statType) =>
            statType switch {
                StatType.Hp => 100f,
                StatType.Mp => 0f,
                StatType.MpRegen => 0f,
                StatType.Movement => 3f,
                StatType.AP => 2f,
                StatType.Atk => 10f,
                StatType.Def => 5f,
                StatType.Accuracy => 0.9f,
                StatType.Evasion => 0.1f,
                StatType.CritChance => 0.1f,
                StatType.Range => 1f,
                StatType.Speed => 1f,
                _ => 0f
            };
    }
}
