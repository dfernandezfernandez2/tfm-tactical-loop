namespace Game.Map.Battle.Generation {
    using Battle.Data;
    using Data;

    public class BattleMapGenerationConfig {
        private BattleMapGenerationConfig(Builder builder) {
            this.WidthRange = builder.WidthRange;
            this.HeightRange = builder.HeightRange;
            this.WallCountRange = builder.WallCountRange;
            this.HeightPatchCountRange = builder.HeightPatchCountRange;
            this.HeightPatchRadiusRange = builder.HeightPatchRadiusRange;
            this.VariantPatchCountRange = builder.VariantPatchCountRange;
            this.VariantPatchRadiusRange = builder.VariantPatchRadiusRange;
            this.MaxHeightRange = builder.MaxHeightRange;
            this.PlayerSpawnCount = builder.PlayerSpawnCount;
            this.EnemySpawnCount = builder.EnemySpawnCount;
            this.MinWallHeight = builder.MinWallHeight;
            this.MinWallHeightAboveNearbyFloor = builder.MinWallHeightAboveNearbyFloor;
            this.WallHeightCheckRadius = builder.WallHeightCheckRadius;
            this.BaseFloorVariant = builder.BaseFloorVariant;
            this.FloorVariantWeights = builder.FloorVariantWeights;
            this.WallVariantWeights = builder.WallVariantWeights;
        }

        public IntRange WidthRange { get; }
        public IntRange HeightRange { get; }
        public IntRange WallCountRange { get; }
        public IntRange HeightPatchCountRange { get; }
        public IntRange HeightPatchRadiusRange { get; }
        public IntRange VariantPatchCountRange { get; }
        public IntRange VariantPatchRadiusRange { get; }
        public IntRange MaxHeightRange { get; }

        public int PlayerSpawnCount { get; }
        public int EnemySpawnCount { get; }

        public int MinWallHeight { get; }
        public int MinWallHeightAboveNearbyFloor { get; }
        public int WallHeightCheckRadius { get; }

        public TileTypeVariant BaseFloorVariant { get; }
        public ListWeightedOption<TileTypeVariant> FloorVariantWeights { get; }
        public ListWeightedOption<TileTypeVariant> WallVariantWeights { get; }

        public static Builder Create() => new();

        public class Builder {
            internal IntRange WidthRange { get; private set; } = new(8, 10);
            internal IntRange HeightRange { get; private set; } = new(8, 10);
            internal IntRange WallCountRange { get; private set; } = new(3, 6);
            internal IntRange HeightPatchCountRange { get; private set; } = new(3, 5);
            internal IntRange HeightPatchRadiusRange { get; private set; } = new(1, 2);
            internal IntRange VariantPatchCountRange { get; private set; } = new(2, 4);
            internal IntRange VariantPatchRadiusRange { get; private set; } = new(2, 4);
            internal IntRange MaxHeightRange { get; private set; } = new(1, 2);

            internal int PlayerSpawnCount { get; private set; } = 5;
            internal int EnemySpawnCount { get; private set; } = 4;
            internal int MinWallHeight { get; private set; } = 1;
            internal int MinWallHeightAboveNearbyFloor { get; private set; } = 1;
            internal int WallHeightCheckRadius { get; private set; } = 1;

            internal TileTypeVariant BaseFloorVariant { get; private set; } = TileTypeVariant.Grass;

            internal ListWeightedOption<TileTypeVariant> FloorVariantWeights { get; private set; } =
                new(
                    new WeightedOption<TileTypeVariant>(TileTypeVariant.Grass, 60),
                    new WeightedOption<TileTypeVariant>(TileTypeVariant.Dirt, 30),
                    new WeightedOption<TileTypeVariant>(TileTypeVariant.Stone, 10)
                );

            internal ListWeightedOption<TileTypeVariant> WallVariantWeights { get; private set; } =
                new(
                    new WeightedOption<TileTypeVariant>(TileTypeVariant.Stone, 100)
                );

            public Builder WithSizeRange(IntRange widthRange, IntRange heightRange) {
                this.WidthRange = widthRange;
                this.HeightRange = heightRange;
                return this;
            }

            public Builder WithWallCountRange(IntRange wallCountRange) {
                this.WallCountRange = wallCountRange;
                return this;
            }

            public Builder WithHeightConfig(IntRange countRange, IntRange radiusRange, IntRange maxHeightRange) {
                this.HeightPatchCountRange = countRange;
                this.HeightPatchRadiusRange = radiusRange;
                this.MaxHeightRange = maxHeightRange;
                return this;
            }

            public Builder WithZoneVariantConfig(IntRange countRange, IntRange radiusRange) {
                this.VariantPatchCountRange = countRange;
                this.VariantPatchRadiusRange = radiusRange;
                return this;
            }

            public Builder WithSpawns(int playerSpawnCount, int enemySpawnCount) {
                this.PlayerSpawnCount = playerSpawnCount;
                this.EnemySpawnCount = enemySpawnCount;
                return this;
            }

            public Builder WithBaseFloorVariant(TileTypeVariant baseFloorVariant) {
                this.BaseFloorVariant = baseFloorVariant;
                return this;
            }

            public Builder WithFloorVariantWeights(ListWeightedOption<TileTypeVariant> floorVariantWeights) {
                this.FloorVariantWeights = floorVariantWeights;
                return this;
            }

            public Builder WithWallVariantWeights(ListWeightedOption<TileTypeVariant> wallVariantWeights) {
                this.WallVariantWeights = wallVariantWeights;
                return this;
            }

            public Builder WithWallHeightRules(int minWallHeight, int minWallHeightAboveNearbyFloor,
                int wallHeightCheckRadius) {
                this.MinWallHeight = minWallHeight;
                this.MinWallHeightAboveNearbyFloor = minWallHeightAboveNearbyFloor;
                this.WallHeightCheckRadius = wallHeightCheckRadius;
                return this;
            }

            public BattleMapGenerationConfig Build() => new(this);
        }
    }
}
