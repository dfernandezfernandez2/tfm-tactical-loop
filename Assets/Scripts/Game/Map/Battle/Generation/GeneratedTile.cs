namespace Game.Map.Battle.Generation {
    using Data;

    public class GeneratedTile {
        public int Height;
        public bool IsEnemySpawn;
        public bool IsPlayerSpawn;
        public Tile Tile;

        public GeneratedTile(TileType type, TileTypeVariant variant, int height = 0) {
            this.Tile = new Tile(type, variant);
            this.Height = height;
        }

        public TileType Type => this.Tile.Type;
        public TileTypeVariant Variant => this.Tile.Variant;

        public void SetTile(TileType type, TileTypeVariant variant) => this.Tile = new Tile(type, variant);
        public bool IsWalkable() => this.Tile.IsWalkable();
    }
}
