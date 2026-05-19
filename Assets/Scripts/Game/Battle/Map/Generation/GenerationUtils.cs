namespace Game.Battle.Map.Generation {
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Map.Data;
    using UnityEngine;
    using Random = System.Random;

    public static class GenerationUtils {
        public static bool IsInside(GeneratedTile[,] tiles, int x, int y) =>
            x >= 0 &&
            y >= 0 &&
            x < tiles.GetLength(0) &&
            y < tiles.GetLength(1);

        public static Vector2Int[] Directions() =>
            new[] {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

        public static void Shuffle<T>(IList<T> list, Random random) {
            for (int i = list.Count - 1; i > 0; i--) {
                int j = random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        public static List<Vector2Int> GetWalkableTiles(GeneratedTile[,] tiles) {
            List<Vector2Int> result = new();
            for (int x = 0; x < tiles.GetLength(0); x++) {
                for (int y = 0; y < tiles.GetLength(1); y++) {
                    if (tiles[x, y].IsWalkable()) {
                        result.Add(new Vector2Int(x, y));
                    }
                }
            }

            return result;
        }

        public static List<Vector2Int> GetSpawns(GeneratedTile[,] tiles, bool player) {
            List<Vector2Int> result = new();
            for (int x = 0; x < tiles.GetLength(0); x++) {
                for (int y = 0; y < tiles.GetLength(1); y++) {
                    GeneratedTile tile = tiles[x, y];
                    if (player ? tile.IsPlayerSpawn : tile.IsEnemySpawn) {
                        result.Add(new Vector2Int(x, y));
                    }
                }
            }

            return result;
        }

        private static bool CanMoveBetween(GeneratedTile[,] tiles, Vector2Int from, Vector2Int to) {
            int fromHeight = tiles[from.x, from.y].Height;
            int toHeight = tiles[to.x, to.y].Height;
            int heightDifference = Mathf.Abs(fromHeight - toHeight);
            return heightDifference <= 1;
        }

        public static int CountTilesOfType(GeneratedTile[,] tiles, TileType type) {
            int count = 0;
            for (int x = 0; x < tiles.GetLength(0); x++) {
                for (int y = 0; y < tiles.GetLength(1); y++) {
                    if (tiles[x, y].Type == type) {
                        count++;
                    }
                }
            }

            return count;
        }

        public static int CountWallNeighbours(GeneratedTile[,] tiles, int x, int y) => Directions()
            .Select(direction => new Vector2Int(x + direction.x, y + direction.y))
            .Where(next => IsInside(tiles, next.x, next.y))
            .Count(next => tiles[next.x, next.y].Type == TileType.Wall);

        public static HashSet<Vector2Int> GetReachableWalkableTiles(GeneratedTile[,] tiles, Vector2Int start) {
            HashSet<Vector2Int> visited = new();
            Queue<Vector2Int> queue = new();
            if (!IsInside(tiles, start.x, start.y) || !tiles[start.x, start.y].IsWalkable()) {
                return visited;
            }

            visited.Add(start);
            queue.Enqueue(start);
            while (queue.Count > 0) {
                Vector2Int current = queue.Dequeue();
                foreach (Vector2Int direction in Directions()) {
                    Vector2Int next = current + direction;
                    if (!IsInside(tiles, next.x, next.y) || visited.Contains(next) ||
                        !tiles[next.x, next.y].IsWalkable() || !CanMoveBetween(tiles, current, next)) {
                        continue;
                    }

                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }

            return visited;
        }

        public static int GetMaxNearbyFloorHeight(GeneratedTile[,] tiles, Vector2Int center, int radius) {
            int maxHeight = 0;
            for (int x = center.x - radius; x <= center.x + radius; x++) {
                for (int y = center.y - radius; y <= center.y + radius; y++) {
                    if (!IsInside(tiles, x, y)) {
                        continue;
                    }

                    int distance = Mathf.Abs(x - center.x) + Mathf.Abs(y - center.y);
                    if (distance == 0 || distance > radius) {
                        continue;
                    }

                    GeneratedTile tile = tiles[x, y];
                    if (tile.Type != TileType.Floor) {
                        continue;
                    }

                    maxHeight = Mathf.Max(maxHeight, tile.Height);
                }
            }

            return maxHeight;
        }
    }
}
