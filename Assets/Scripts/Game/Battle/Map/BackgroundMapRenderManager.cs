namespace Game.Battle.Map {
    using System;
    using System.Linq;
    using Run.Map;
    using UnityEngine;

    public class BackgroundMapRenderManager : MonoBehaviour {
        private const string _borderSymbol = "0:Floor:";
        private const string _emptySymbol = "0:Empty:Default";
        [SerializeField] private BattleMapLoader battleMapLoader;

        public void StartMap(EncounterType encounterType, int level) {
            int realMapSize = encounterType.GetSizeByLevel(level);
            string mapText = ReadMapText(encounterType.ToString());
            string[] mapLines = mapText.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);
            string[][] symbols = mapLines.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToArray();
            int backgroundSizeY = symbols.Length;
            int backgroundSizeX = symbols[0].Length;
            int marginX = (backgroundSizeX - realMapSize) / 2;
            int marginY = (backgroundSizeY - realMapSize) / 2;
            Vector2Int offset = new(-marginX, -marginY);
            mapText = ReplaceCenteredAreaAndBorder(symbols, realMapSize, offset,
                _borderSymbol + encounterType.GetBgTileTypeDelimiter());
            this.battleMapLoader.Load(mapText, offset);
        }

        private static string ReadMapText(string name) {
            TextAsset map = Resources.Load<TextAsset>("Map/BG/" + name);
            return map.text;
        }

        private static string ReplaceCenteredAreaAndBorder(string[][] symbols, int areaSize, Vector2Int offset,
            string borderSymbol) {
            int backgroundHeight = symbols.Length;
            int backgroundWidth = symbols[0].Length;
            int maxArea = areaSize - 1;
            for (int x = 0; x < backgroundWidth; x++) {
                for (int y = 0; y < backgroundHeight; y++) {
                    int xReal = x + offset.x;
                    int yReal = backgroundHeight - 1 - y + offset.y;
                    bool isInsideClearArea = xReal >= 0 && xReal <= maxArea && yReal >= 0 && yReal <= maxArea;
                    if (isInsideClearArea) {
                        symbols[y][x] = _emptySymbol;
                    }
                    else if (IsBorder(xReal, yReal, maxArea)) {
                        symbols[y][x] = borderSymbol;
                    }
                }
            }

            return string.Join("\n", symbols.Select(row => string.Join(" ", row)));
        }

        private static bool IsBorder(int x, int y, int areaSize) {
            bool isLeftBorder = x == -1 && y >= 0 && y <= areaSize;
            bool isRightBorder = x == areaSize + 1 && y >= 0 && y <= areaSize;
            bool isBottomBorder = y == -1 && x >= 0 && x <= areaSize;
            bool isTopBorder = y == areaSize + 1 && x >= 0 && x <= areaSize;
            bool isCorner = (y == -1 && x == -1) || (x == -1 && y == areaSize + 1) || (y == -1 && x == areaSize + 1) ||
                            (y == areaSize + 1 && x == areaSize + 1);
            return isLeftBorder || isRightBorder || isBottomBorder || isTopBorder || isCorner;
        }
    }
}
