namespace Map.Battle.Data {
    using System;
    using System.Collections.Generic;

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
