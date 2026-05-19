namespace Game.Battle.Map.Data {
    public enum TileType {
        Empty,
        Floor,
        Wall
    }

    public static class TileTypeExtensions {
        public static bool IsWalkable(this TileType type) => type == TileType.Floor;
        public static bool BlockLineOfSight(this TileType type) => type == TileType.Wall;
    }
}
