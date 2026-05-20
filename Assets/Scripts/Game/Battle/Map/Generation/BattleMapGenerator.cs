namespace Game.Battle.Map.Generation {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Map.Data;
    using Parser;
    using UnityEngine;
    using Random = System.Random;

    public static class BattleMapGenerator {
        private const int _maxAttempts = 40;

        public static string Generate(BattleMapGenerationConfig config, string seed) {
            int baseSeed = seed.GetHashCode();
            for (int attempt = 0; attempt < _maxAttempts; attempt++) {
                Random random = new(baseSeed + attempt);
                int width = config.WidthRange.Pick(random);
                int height = config.HeightRange.Pick(random);
                GeneratedTile[,] tiles = CreateBaseMap(width, height, config.BaseFloorVariant);
                ApplyFloorVariantPatches(tiles, config, random);
                SmoothIsolatedVariants(tiles);
                ApplyHeights(tiles, config, random);
                ApplyWalls(tiles, config, random);
                ApplySpawns(tiles, config, random);
                if (BattleMapValidator.IsValid(tiles, config)) {
                    return TxtMapLegend.SerializeMap(tiles);
                }
            }

            throw new Exception($"Could not generate valid battle map with seed {seed}");
        }

        private static GeneratedTile[,] CreateBaseMap(int width, int height, TileTypeVariant baseFloorVariant) {
            GeneratedTile[,] tiles = new GeneratedTile[width, height];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    tiles[x, y] = new GeneratedTile(
                        TileType.Floor,
                        baseFloorVariant
                    );
                }
            }

            return tiles;
        }

        private static void ApplyFloorVariantPatches(GeneratedTile[,] tiles, BattleMapGenerationConfig config,
            Random random) {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            int patchCount = config.VariantPatchCountRange.Pick(random);
            for (int i = 0; i < patchCount; i++) {
                TileTypeVariant variant = config.FloorVariantWeights.Pick(random);
                int centerX = random.Next(0, width);
                int centerY = random.Next(0, height);
                int radius = config.VariantPatchRadiusRange.Pick(random);
                ApplyVariantPatch(tiles, centerX, centerY, radius, variant, random);
            }
        }

        private static void ApplyVariantPatch(GeneratedTile[,] tiles, int centerX, int centerY, int radius,
            TileTypeVariant variant, Random random) {
            for (int x = centerX - radius; x <= centerX + radius; x++) {
                for (int y = centerY - radius; y <= centerY + radius; y++) {
                    if (!GenerationUtils.IsInside(tiles, x, y) || tiles[x, y].Type != TileType.Floor) {
                        continue;
                    }

                    int distance = Mathf.Abs(x - centerX) + Mathf.Abs(y - centerY);
                    if (distance > radius) {
                        continue;
                    }

                    float chance = 1f - ((float)distance / (radius + 1));
                    chance = Mathf.Clamp(chance + 0.25f, 0.25f, 0.95f);
                    if (random.NextDouble() > chance) {
                        continue;
                    }

                    tiles[x, y].SetTile(TileType.Floor, variant);
                }
            }
        }

        private static void SmoothIsolatedVariants(GeneratedTile[,] tiles) {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            List<(int x, int y, TileTypeVariant variant)> changes = new();
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    GeneratedTile tile = tiles[x, y];
                    if (tile.Type != TileType.Floor) {
                        continue;
                    }

                    TileTypeVariant neighbourMajorityVariant = GetNeighbourMajorityVariant(tiles, x, y);
                    int sameNeighboursVariants = CountSameVariantNeighbours(tiles, x, y, tile.Variant);
                    if (sameNeighboursVariants == 0 && neighbourMajorityVariant != tile.Variant) {
                        changes.Add((x, y, neighbourMajorityVariant));
                    }
                }
            }

            foreach ((int x, int y, TileTypeVariant variant) change in changes) {
                tiles[change.x, change.y].SetTile(TileType.Floor, change.variant);
            }
        }

        private static TileTypeVariant GetNeighbourMajorityVariant(GeneratedTile[,] tiles, int x, int y) {
            Dictionary<TileTypeVariant, int> counts = new();
            foreach (Vector2Int direction in GenerationUtils.Directions()) {
                int neighbourX = x + direction.x;
                int neighbourY = y + direction.y;
                if (!GenerationUtils.IsInside(tiles, neighbourX, neighbourY)) {
                    continue;
                }

                GeneratedTile neighbour = tiles[neighbourX, neighbourY];
                if (neighbour.Type != TileType.Floor) {
                    continue;
                }

                counts.TryAdd(neighbour.Variant, 0);
                counts[neighbour.Variant]++;
            }

            TileTypeVariant bestVariant = tiles[x, y].Variant;
            int bestCount = -1;
            foreach (KeyValuePair<TileTypeVariant, int> pair in counts.Where(pair => pair.Value > bestCount)) {
                bestVariant = pair.Key;
                bestCount = pair.Value;
            }

            return bestVariant;
        }

        private static int CountSameVariantNeighbours(GeneratedTile[,] tiles, int x, int y, TileTypeVariant variant) =>
            (from direction in GenerationUtils.Directions()
                let nx = x + direction.x
                let ny = y + direction.y
                where GenerationUtils.IsInside(tiles, nx, ny)
                select tiles[nx, ny])
            .Count(neighbour => neighbour.Type == TileType.Floor && neighbour.Variant == variant);

        private static void ApplyHeights(GeneratedTile[,] tiles, BattleMapGenerationConfig config, Random random) {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            int patchCount = config.HeightPatchCountRange.Pick(random);
            int maxHeight = config.MaxHeightRange.Pick(random);
            for (int i = 0; i < patchCount; i++) {
                int centerX = random.Next(1, width - 1);
                int centerY = random.Next(1, height - 1);
                int radius = config.HeightPatchRadiusRange.Pick(random);
                int tileHeight = random.Next(1, maxHeight + 1);
                for (int x = centerX - radius; x <= centerX + radius; x++) {
                    for (int y = centerY - radius; y <= centerY + radius; y++) {
                        if (!GenerationUtils.IsInside(tiles, x, y)) {
                            continue;
                        }

                        int distance = Mathf.Abs(x - centerX) + Mathf.Abs(y - centerY);
                        if (distance > radius) {
                            continue;
                        }

                        if (random.NextDouble() > 0.8f) {
                            continue;
                        }

                        tiles[x, y].Height = Mathf.Max(tiles[x, y].Height, tileHeight);
                    }
                }
            }

            SmoothHeightJumps(tiles);
            SmoothHeightJumps(tiles); // second time
        }

        private static void SmoothHeightJumps(GeneratedTile[,] tiles) {
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    foreach (Vector2Int direction in GenerationUtils.Directions()) {
                        int neighbourX = x + direction.x;
                        int neighbourY = y + direction.y;
                        if (!GenerationUtils.IsInside(tiles, neighbourX, neighbourY)) {
                            continue;
                        }

                        int diff = tiles[x, y].Height - tiles[neighbourX, neighbourY].Height;
                        if (diff > 1) {
                            tiles[x, y].Height = tiles[neighbourX, neighbourY].Height + 1;
                        }
                    }
                }
            }
        }

        private static void ApplyWalls(GeneratedTile[,] tiles, BattleMapGenerationConfig config, Random random) {
            List<Vector2Int> candidates = GetWallCandidates(tiles, config);
            GenerationUtils.Shuffle(candidates, random);
            int targetWallCount = config.WallCountRange.Pick(random);
            int placed = 0;
            foreach (Vector2Int candidate in candidates) {
                if (placed >= targetWallCount) {
                    break;
                }

                if (WouldCreateTooDenseWallCluster(tiles, candidate)) {
                    continue;
                }

                GeneratedTile floorTile = tiles[candidate.x, candidate.y];
                TileTypeVariant wallVariant = random.NextDouble() < 0.65f
                    ? config.WallVariantWeights.Pick(random)
                    : TileTypeVariant.Stone;
                floorTile.SetTile(TileType.Wall, wallVariant);
                ApplyWallHeight(tiles, candidate, config);
                placed++;
            }
        }

        private static List<Vector2Int> GetWallCandidates(GeneratedTile[,] tiles, BattleMapGenerationConfig config) {
            List<Vector2Int> candidates = new();
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 1; x < width - 1; x++) {
                for (int y = 2; y < height - 2; y++) {
                    GeneratedTile tile = tiles[x, y];
                    if (tile.Type != TileType.Floor || tile.Height < config.MinWallHeight) {
                        continue;
                    }

                    candidates.Add(new Vector2Int(x, y));
                }
            }

            return candidates;
        }

        private static bool WouldCreateTooDenseWallCluster(GeneratedTile[,] tiles, Vector2Int position) {
            int wallNeighbours = (from direction in GenerationUtils.Directions()
                let neighbourX = position.x + direction.x
                let neighbourY = position.y + direction.y
                where GenerationUtils.IsInside(tiles, neighbourX, neighbourY)
                where tiles[neighbourX, neighbourY].Type == TileType.Wall
                select neighbourX).Count();
            return wallNeighbours >= 2;
        }

        private static void ApplySpawns(GeneratedTile[,] tiles, BattleMapGenerationConfig config, Random random) {
            List<Vector2Int> playerCandidates = new();
            List<Vector2Int> enemyCandidates = new();
            int width = tiles.GetLength(0);
            int height = tiles.GetLength(1);
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    GeneratedTile tile = tiles[x, y];
                    if (tile.Type != TileType.Floor) {
                        continue;
                    }

                    if (y <= 1) {
                        playerCandidates.Add(new Vector2Int(x, y));
                    }

                    if (y >= height - 2) {
                        enemyCandidates.Add(new Vector2Int(x, y));
                    }
                }
            }

            GenerationUtils.Shuffle(playerCandidates, random);
            GenerationUtils.Shuffle(enemyCandidates, random);
            for (int i = 0; i < config.PlayerSpawnCount && i < playerCandidates.Count; i++) {
                Vector2Int position = playerCandidates[i];
                tiles[position.x, position.y].IsPlayerSpawn = true;
            }

            for (int i = 0; i < config.EnemySpawnCount && i < enemyCandidates.Count; i++) {
                Vector2Int position = enemyCandidates[i];
                tiles[position.x, position.y].IsEnemySpawn = true;
            }
        }

        private static void ApplyWallHeight(GeneratedTile[,] tiles, Vector2Int wallPosition,
            BattleMapGenerationConfig config) {
            GeneratedTile wallTile = tiles[wallPosition.x, wallPosition.y];
            int maxNearbyFloorHeight =
                GenerationUtils.GetMaxNearbyFloorHeight(tiles, wallPosition, config.WallHeightCheckRadius);
            int requiredWallHeight = maxNearbyFloorHeight + config.MinWallHeightAboveNearbyFloor;
            wallTile.Height = Mathf.Max(wallTile.Height, config.MinWallHeight, requiredWallHeight);
        }
    }
}
