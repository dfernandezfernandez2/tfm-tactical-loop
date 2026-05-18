namespace Game.Map.Battle.Generation {
    using System.Collections.Generic;
    using System.Linq;
    using Battle.Data;
    using Data;
    using UnityEngine;

    public static class BattleMapValidator {
        public static bool IsValid(GeneratedTile[,] tiles, BattleMapGenerationConfig config) =>
            IsValidSize(tiles, config) && IsValidSpawnZones(tiles, config) && IsValidConnectivity(tiles) &&
            IsValidObstacles(tiles, config);

        private static bool IsValidSize(GeneratedTile[,] tiles, BattleMapGenerationConfig config) {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            return width >= config.WidthRange.Min && width <= config.WidthRange.Max
                                                  && height >= config.HeightRange.Min &&
                                                  height <= config.HeightRange.Max;
        }

        private static bool IsValidSpawnZones(GeneratedTile[,] tiles, BattleMapGenerationConfig config) {
            List<Vector2Int> playerSpawns = GenerationUtils.GetSpawns(tiles, true);
            List<Vector2Int> enemySpawns = GenerationUtils.GetSpawns(tiles, false);
            if (playerSpawns.Count < config.PlayerSpawnCount || enemySpawns.Count < config.EnemySpawnCount ||
                playerSpawns.Concat(enemySpawns).Select(spawn => tiles[spawn.x, spawn.y])
                    .Any(tile => tile.Type != TileType.Floor || !tile.IsWalkable())) {
                return false;
            }

            int minDistance = playerSpawns.Aggregate(int.MaxValue,
                (current1, first) =>
                    enemySpawns.Select(second => Mathf.Abs(first.x - second.x) + Mathf.Abs(first.y - second.y))
                        .Aggregate(current1, Mathf.Min));
            return minDistance >= 3;
        }

        private static bool IsValidConnectivity(GeneratedTile[,] tiles) {
            List<Vector2Int> walkableTiles = GenerationUtils.GetWalkableTiles(tiles);
            if (walkableTiles.Count == 0) {
                return false;
            }

            HashSet<Vector2Int> connected = GenerationUtils.GetReachableWalkableTiles(tiles, walkableTiles[0]);
            float connectedRatio = connected.Count / (float)walkableTiles.Count;
            if (connectedRatio < 0.95f) {
                return false;
            }

            List<Vector2Int> playerSpawns = GenerationUtils.GetSpawns(tiles, true);
            List<Vector2Int> enemySpawns = GenerationUtils.GetSpawns(tiles, false);
            HashSet<Vector2Int> reachableFromPlayer = GenerationUtils.GetReachableWalkableTiles(tiles, playerSpawns[0]);
            return enemySpawns.Any(reachableFromPlayer.Contains);
        }

        private static bool IsValidObstacles(GeneratedTile[,] tiles, BattleMapGenerationConfig config) {
            int wallCount = GenerationUtils.CountTilesOfType(tiles, TileType.Wall);
            int walkableCount = GenerationUtils.GetWalkableTiles(tiles).Count;
            int total = tiles.GetLength(0) * tiles.GetLength(1);
            if (wallCount < config.WallCountRange.Min || wallCount > config.WallCountRange.Max) {
                return false;
            }

            float wallRatio = wallCount / (float)total;
            if (wallRatio > 0.25f) {
                return false;
            }

            float walkableRatio = walkableCount / (float)total;
            if (walkableRatio < 0.65f) {
                return false;
            }

            return !HasBlockingWallLine(tiles) &&
                   !HasTooDenseWallCluster(tiles, 2) &&
                   IsValidWallsHeight(tiles, config);
        }

        private static bool IsValidWallsHeight(GeneratedTile[,] tiles, BattleMapGenerationConfig config) {
            for (int x = 0; x < tiles.GetLength(0); x++) {
                for (int y = 0; y < tiles.GetLength(1); y++) {
                    GeneratedTile tile = tiles[x, y];
                    if (tile.Type != TileType.Wall) {
                        continue;
                    }
                    if (tile.Height < config.MinWallHeight) {
                        return false;
                    }
                    int maxNearbyFloorHeight =
                        GenerationUtils.GetMaxNearbyFloorHeight(tiles, new Vector2Int(x, y), config.WallHeightCheckRadius);
                    int requiredHeight = maxNearbyFloorHeight + config.MinWallHeightAboveNearbyFloor;
                    if (tile.Height < requiredHeight) {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool HasBlockingWallLine(GeneratedTile[,] tiles) {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int y = 0; y < height; y++) {
                bool fullRow = true;
                for (int x = 0; x < width; x++) {
                    if (tiles[x, y].Type == TileType.Wall) {
                        continue;
                    }

                    fullRow = false;
                    break;
                }

                if (fullRow) {
                    return true;
                }
            }

            for (int x = 0; x < width; x++) {
                bool fullColumn = true;
                for (int y = 0; y < height; y++) {
                    if (tiles[x, y].Type == TileType.Wall) {
                        continue;
                    }

                    fullColumn = false;
                    break;
                }

                if (fullColumn) {
                    return true;
                }
            }

            return false;
        }

        private static bool HasTooDenseWallCluster(GeneratedTile[,] tiles, int maxNeighbours) {
            for (int x = 0; x < tiles.GetLength(0); x++) {
                for (int y = 0; y < tiles.GetLength(1); y++) {
                    if (tiles[x, y].Type != TileType.Wall) {
                        continue;
                    }

                    int neighbours = GenerationUtils.CountWallNeighbours(tiles, x, y);
                    if (neighbours > maxNeighbours) {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
