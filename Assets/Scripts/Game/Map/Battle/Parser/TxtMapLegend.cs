namespace Game.Map.Battle.Parser {
    using Data;

    public static class TxtMapLegend {

        public static TileType GetTileType(string symbol) {
            string cleanSymbol = symbol.Split('#')[0];
            return cleanSymbol[^1] switch {
                'F' => TileType.Floor,
                'W' => TileType.Wall,
                _ => TileType.Empty
            };
        }

        public static SpawnType GetSpawnType(string symbol) {
            int index = symbol.IndexOf('#');
            if (index == -1) {
                return SpawnType.None;
            }
            char spawnChar = symbol[index + 1];
            return spawnChar switch {
                'P' => SpawnType.Player,
                'E' => SpawnType.Enemy,
                _ => SpawnType.None
            };
        }

        public static int GetTileHeight(string symbol) {
            string cleanSymbol = symbol.Split('#')[0];
            string numberPart = cleanSymbol[..^1];
            return int.TryParse(numberPart, out int height) ? height : 0;
        }
    }
}
