namespace Map.Battle.Data {
    public enum TileType {
        Empty,
        Floor,
        Wall
    }

    public static class TileTypeExtensions {
        public static bool IsRenderBellow(this TileType type) => type == TileType.Wall;
    }
}
