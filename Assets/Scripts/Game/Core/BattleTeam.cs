namespace Game.Core {
    using Map.Battle.Renderer;
    using UnityEngine;

    public enum BattleTeam {
        Player,
        Enemy
    }

    public static class BattleTeamColorExtensions {
        public static Color GetColor(this BattleTeam battleTeam) =>
            battleTeam switch {
                BattleTeam.Player => Color.softYellow,
                BattleTeam.Enemy => Color.pink,
                _ => Color.white
            };
        public static HighlightColor GetHighlightColor(this BattleTeam battleTeam) =>
            battleTeam switch {
                BattleTeam.Player => HighlightColor.Yellow,
                BattleTeam.Enemy => HighlightColor.Orange,
                _ => HighlightColor.None
            };
    }
}
