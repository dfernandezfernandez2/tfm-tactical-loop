namespace Game.Map.Battle.Parser {
    using System;
    using System.Text;
    using Data;
    using Generation.Data;

    public static class TxtMapLegend {
        public static Tile GetTile(string symbol) {
            string[] splitSymbol = symbol.Split('#');
            string cleanSymbol = splitSymbol[0];
            string[] splitTileData = cleanSymbol.Split(':');
            string tileType = splitTileData[1];
            string variantType = splitTileData[2];
            TileType type = Enum.TryParse(tileType, true, out TileType parsedType) ? parsedType : TileType.Empty;
            TileTypeVariant tileTypeVariant = Enum.TryParse(variantType, true, out TileTypeVariant parsedTileVariant)
                ? parsedTileVariant
                : TileTypeVariant.Default;
            return new Tile(type, tileTypeVariant);
        }

        public static SpawnType GetSpawnType(string symbol) {
            string[] splitSymbol = symbol.Split('#');
            string spawnTypeSymbol = splitSymbol.Length > 1 ? splitSymbol[1] : string.Empty;
            return Enum.TryParse(spawnTypeSymbol, true, out SpawnType spawnType) ? spawnType : SpawnType.None;
        }

        public static int GetTileHeight(string symbol) {
            string[] splitSymbol = symbol.Split('#');
            string cleanSymbol = splitSymbol[0];
            string[] splitTileData = cleanSymbol.Split(':');
            string numberPart = splitTileData[0];
            return int.TryParse(numberPart, out int height) ? height : 0;
        }

        public static string SerializeMap(GeneratedTile[,] generatedTiles) {
            int width = generatedTiles.GetLength(0);
            int height = generatedTiles.GetLength(1);
            StringBuilder builder = new();
            for (int y = height - 1; y >= 0; y--) {
                for (int x = 0; x < width; x++) {
                    GeneratedTile tile = generatedTiles[x, y];
                    StringBuilder tileBuilder = new();
                    tileBuilder.Append(tile.Height).Append(":").Append(tile.Tile.Type).Append(":")
                        .Append(tile.Tile.Variant);
                    if (tile.IsPlayerSpawn) {
                        tileBuilder.Append("#").Append(SpawnType.Player);
                    }
                    else if (tile.IsEnemySpawn) {
                        tileBuilder.Append("#").Append(SpawnType.Enemy);
                    }

                    builder.Append(tileBuilder);
                    if (x < width - 1) {
                        builder.Append(' ');
                    }
                }

                if (y > 0) {
                    builder.AppendLine();
                }
            }

            return builder.ToString();
        }
    }
}
