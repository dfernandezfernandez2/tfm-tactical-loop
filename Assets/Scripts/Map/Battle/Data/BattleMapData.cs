namespace Map.Battle.Data;

using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapData {

    private readonly int _width;
    private readonly int _height;
    private readonly TileData[,] _tiles;
    public List<Vector2Int> PlayerSpawns { get; }
    public List<Vector2Int> EnemySpawns { get; }

    private BattleMapData(int width, int height,  TileData[,] tiles, List<Vector2Int> playerSpawns, List<Vector2Int> enemySpawns) {
        this._width = width;
        this._height = height;
        this._tiles = tiles;
        this.PlayerSpawns = playerSpawns;
        this.EnemySpawns = enemySpawns;
    }

    public void ForEach(Action<int, int, TileData> action) {
        for (int y = 0; y < this._height; y++) {
            for (int x = 0; x < this._width; x++) {
                action.Invoke(x, y, this._tiles[x, y]);
            }
        }
    }

    public class Builder {

        private readonly List<TileData> _tiles = new();
        private readonly int _width;
        private readonly int _height;

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
            List<Vector2Int> playerSpawns = new();
            List<Vector2Int> enemySpawns = new();

            foreach(TileData tile in this._tiles) {
                tileDatas[tile.Position.x, tile.Position.y] = tile;
                switch (tile.Type) {
                    case TileType.EnemySpawn:
                        enemySpawns.Add(tile.Position);
                        break;
                    case TileType.PlayerSpawn:
                        playerSpawns.Add(tile.Position);
                        break;
                }
            }
            return new BattleMapData(this._width, this._height, tileDatas, playerSpawns, enemySpawns);
        }
    }

}
