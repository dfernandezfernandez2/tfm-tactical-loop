namespace Map.Battle.Parser;

using System;
using Data;
using UnityEngine;

public class TxtMapParser : IMapParser {

    public BattleMapData Parse(string mapTextContent) {
        if (string.IsNullOrWhiteSpace(mapTextContent)) {
            throw new ArgumentNullException(nameof(mapTextContent));
        }

        string[] linesMap = CleanText(mapTextContent).Split("\n",  StringSplitOptions.RemoveEmptyEntries);
        ValidateText(linesMap);
        int height = linesMap.Length;
        int width = linesMap[0].Length;

        BattleMapData.Builder battleMapDataBuilder = new(width, height);

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                char symbol = linesMap[y][x];
                TileType type = TxtMapLegend.GetTileTypeFromChar(symbol);
                Vector2Int position = new(x, height - 1 - y);
                TileData tileData = new(position, type);
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
        int height = linesMap.Length;
        int width = linesMap[0].Length;

        for (int y = 1; y < height; y++) {
            if (linesMap[y].Length != width) {
                throw new FormatException($"All rows must be with the same length. Row 0: {width}, Row {y}: {linesMap[y]}");
            }
        }
    }
}
