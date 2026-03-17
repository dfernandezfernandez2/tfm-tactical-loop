namespace Map.Battle.Parser;

using Data;

public class TxtMapLegend {

    private TxtMapLegend() {}

    public static TileType GetTileTypeFromChar(char symbol) =>
        symbol switch {
            '_' => TileType.Floor,
            '-' => TileType.Wall,
            'P' => TileType.PlayerSpawn,
            'E' => TileType.EnemySpawn,
            _ => TileType.Empty
        };
}
