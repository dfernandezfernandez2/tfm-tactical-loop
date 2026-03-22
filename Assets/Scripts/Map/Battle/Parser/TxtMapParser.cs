namespace Map.Battle.Parser {
    using System;
    using Data;
    using UnityEngine;

    public class TxtMapParser : IMapParser {
        public BattleMapData Parse(string mapTextContent) {
            if (string.IsNullOrWhiteSpace(mapTextContent)) {
                throw new ArgumentNullException(nameof(mapTextContent));
            }

            string[] linesMap = CleanText(mapTextContent).Split("\n", StringSplitOptions.RemoveEmptyEntries);
            ValidateText(linesMap);
            int height = linesMap.Length;
            int width = linesMap[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            BattleMapData.Builder battleMapDataBuilder = new(width, height);

            for (int y = 0; y < height; y++) {
                string[] symbols = linesMap[y].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < width; x++) {
                    string symbol = symbols[x];
                    TileType type = TxtMapLegend.GetTileType(symbol);
                    Vector2Int position = new(x, height - 1 - y);
                    int tileHeight = TxtMapLegend.GetTileHeight(symbol);
                    TileData tileData = new(position, type, tileHeight);
                    battleMapDataBuilder.AddTile(tileData);
                }
            }

            return battleMapDataBuilder.Build();
        }

        private static string CleanText(string text) => text.Replace("\r", "");

        private static void ValidateText(string[] linesMap) {
            if (linesMap.Length == 0) {
                throw new ArgumentException("Map text is empty");
            }

            int width = linesMap[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

            for (int y = 1; y < linesMap.Length; y++) {
                int currentWidth = linesMap[y].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                if (currentWidth != width) {
                    throw new FormatException(
                        $"All rows must be with the same length. Row 0: {width}, Row {y}: {currentWidth}");
                }
            }
        }
    }
}
