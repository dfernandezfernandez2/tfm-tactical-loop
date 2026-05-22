namespace Game.Run.Map {
    public enum EncounterType {
        Start,
        Basic,
        Elite,
        Boss,
        End
    }

    public static class EncounterTypeExtensions {
        public static int GetSizeByLevel(this EncounterType encounterType, int level) =>
            encounterType switch {
                EncounterType.Basic => 8 + level,
                EncounterType.Elite => 10 + level,
                EncounterType.Boss => 16,
                _ => 10
            };

        public static string GetBgTileTypeDelimiter(this EncounterType encounterType) =>
            encounterType switch {
                EncounterType.Boss => "Magma",
                _ => "Water"
            };
    }
}
