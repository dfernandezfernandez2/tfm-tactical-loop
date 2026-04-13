namespace Game.Core {
    using UnityEngine;

    public enum BattleTeam {
        Player,
        Enemy
    }

    public static class BattleTeamColorExtensions {
        public static Color GetColor(this BattleTeam battleTeam) =>
            battleTeam switch {
                BattleTeam.Player => Color.softYellow,
                BattleTeam.Enemy => Color.darkOrange,
                _ => Color.white
            };
    }
}
