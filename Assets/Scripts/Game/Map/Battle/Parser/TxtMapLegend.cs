namespace Game.Map.Battle.Parser {
    using Data;

    public class TxtMapLegend {
        private TxtMapLegend() {
        }

        public static TileType GetTileType(string symbol) =>
            symbol[^1] switch {
                'F' => TileType.Floor,
                'W' => TileType.Wall,
                _ => TileType.Empty
            };

        public static int GetTileHeight(string symbol) {
            string numberPart = symbol[..^1];
            return int.TryParse(numberPart, out int height) ? height : 0;
        }
    }
}
