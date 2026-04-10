namespace Game.Map.Battle.Data {
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class BattleMapData {
        private readonly int _height;

        private readonly TileData[,] _tiles;
        private readonly int _width;

        private BattleMapData(int width, int height, TileData[,] tiles) {
            this._width = width;
            this._height = height;
            this._tiles = tiles;
        }

        public void ForEach(Action<TileData> action) {
            for (int y = 0; y < this._height; y++) {
                for (int x = 0; x < this._width; x++) {
                    action.Invoke(this._tiles[x, y]);
                }
            }
        }

        public TileData GetTile(int x, int y) => !this.IsInside(x, y) ? null : this._tiles[x, y];

        public IReadOnlyCollection<GridPosition> GetNeighbours(GridPosition origin) {
            List<GridPosition> neighbours = new();
            Vector2Int originPosition = origin.Position;
            int originHeight = origin.Height;

            Vector2Int[] directions = {
                Vector2Int.right,
                Vector2Int.left,
                Vector2Int.up,
                Vector2Int.down
            };

            foreach (Vector2Int direction in directions) {
                int x = originPosition.x + direction.x;
                int y = originPosition.y + direction.y;
                if (!this.IsInside(x, y)) {
                    continue;
                }

                TileData tile = this._tiles[x, y];
                int heightDifference = Math.Abs(tile.TileGridPosition.Height - originHeight);

                if (heightDifference <= 1) {
                    neighbours.Add(tile.TileGridPosition);
                }
            }

            return neighbours;
        }

        public bool IsInside(int x, int y) => x >= 0 && x < this._width && y >= 0 && y < this._height;

        public class Builder {
            private readonly int _height;

            private readonly List<TileData> _tiles = new();
            private readonly int _width;

            public Builder(int width, int height) {
                this._width = width;
                this._height = height;
            }

            public Builder AddTile(TileData tile) {
                this._tiles.Add(tile);
                return this;
            }

            public BattleMapData Build() {
                TileData[,] tileDatas = new TileData[this._width, this._height];

                foreach (TileData tile in this._tiles) {
                    tileDatas[tile.TileGridPosition.Position.x, tile.TileGridPosition.Position.y] = tile;
                }

                return new BattleMapData(this._width, this._height, tileDatas);
            }
        }
    }
}
