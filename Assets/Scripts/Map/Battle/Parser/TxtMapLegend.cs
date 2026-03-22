namespace Map.Battle.Parser {
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

            if (int.TryParse(numberPart, out int height)) {
                return height;
            }

            return 0;
        }
    }
}
