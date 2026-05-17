namespace Game.Map.Battle {
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using UnityEngine;

    public class BattleLineOfSight {
        private readonly BattleMapData _mapData;

        public BattleLineOfSight(BattleMapData mapData) => this._mapData = mapData;

        public bool HasLineOfSight(TileData origin, TileData target, bool applyHeightLineOfSight) {
            Vector2Int originPosition = origin.TileGridPosition.Position;
            Vector2Int targetPosition = target.TileGridPosition.Position;
            return (from position in GetLinePositions(originPosition, targetPosition)
                where position != originPosition && position != targetPosition
                select this._mapData.GetTile(position.x, position.y)).All(currentTile =>
                currentTile != null && !currentTile.Tile.Type.BlockLineOfSight() && (!applyHeightLineOfSight ||
                    !BlocksLineOfSightByHeight(origin, target, currentTile)));
        }

        private static bool BlocksLineOfSightByHeight(TileData originTile, TileData targetTile,
            TileData intermediateTile) {
            int originHeight = originTile.TileGridPosition.Height;
            int targetHeight = targetTile.TileGridPosition.Height;
            int intermediateHeight = intermediateTile.TileGridPosition.Height;
            return intermediateHeight > originHeight && intermediateHeight > targetHeight;
        }

        // Bresenham algorithm
        private static IEnumerable<Vector2Int> GetLinePositions(Vector2Int origin, Vector2Int target) {
            int originX = origin.x;
            int originY = origin.y;
            int targetX = target.x;
            int targetY = target.y;

            int deltaX = Mathf.Abs(targetX - originX);
            int deltaY = Mathf.Abs(targetY - originY);

            int stepX = originX < targetX ? 1 : -1;
            int stepY = originY < targetY ? 1 : -1;

            int error = deltaX - deltaY;

            while (true) {
                yield return new Vector2Int(originX, originY);
                if (originX == targetX && originY == targetY) {
                    break;
                }

                int doubleError = error * 2;
                if (doubleError > -deltaY) {
                    error -= deltaY;
                    originX += stepX;
                }

                if (doubleError < deltaX) {
                    error += deltaX;
                    originY += stepY;
                }
            }
        }
    }
}
