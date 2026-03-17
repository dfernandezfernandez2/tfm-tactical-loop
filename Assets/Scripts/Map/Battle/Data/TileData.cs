namespace Map.Battle.Data;

using UnityEngine;

public class TileData {

    public Vector2Int Position { get; }
    public TileType Type { get; }

    public TileData(Vector2Int position, TileType type) {
        this.Position = position;
        this.Type = type;
    }

}
