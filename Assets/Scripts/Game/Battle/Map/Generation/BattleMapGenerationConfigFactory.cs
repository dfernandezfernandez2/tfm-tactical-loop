namespace Game.Battle.Map.Generation {
    using Data;
    using Map.Data;
    using Run.Map;

    public static class BattleMapGenerationConfigFactory {
        public static BattleMapGenerationConfig FromNode(RunNode node, int numEnemies) =>
            node.EncounterType switch {
                EncounterType.Elite => CreateElite(node.Level, numEnemies),
                EncounterType.Boss => CreateBoss(node.Level, numEnemies),
                _ => CreateBasic(node.Level, numEnemies)
            };

        private static BattleMapGenerationConfig CreateBasic(int level, int numEnemies) {
            int size = EncounterType.Basic.GetSizeByLevel(level);
            return BattleMapGenerationConfig
                .Create()
                .WithSizeRange(
                    new IntRange(size, size),
                    new IntRange(size, size)
                )
                .WithWallCountRange(
                    new IntRange(3 + level, 6 + level)
                )
                .WithHeightConfig(
                    new IntRange(2, 3 + level),
                    new IntRange(1, 3),
                    new IntRange(1, 2)
                )
                .WithZoneVariantConfig(
                    new IntRange(2, 4),
                    new IntRange(2, 4)
                )
                .WithSpawns(
                    6,
                    numEnemies + 2
                )
                .WithBaseFloorVariant(TileTypeVariant.Grass)
                .WithFloorVariantWeights(
                    new ListWeightedOption<TileTypeVariant>(
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Grass, 65),
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Dirt, 35)
                    )
                )
                .WithWallVariantWeights(
                    new ListWeightedOption<TileTypeVariant>(
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Stone, 100)
                    )
                )
                .WithWallHeightRules(1, 1, 1)
                .Build();
        }


        private static BattleMapGenerationConfig CreateElite(int level, int numEnemies) {
            int size = EncounterType.Elite.GetSizeByLevel(level);
            return BattleMapGenerationConfig
                .Create()
                .WithSizeRange(
                    new IntRange(size, size),
                    new IntRange(size, size)
                )
                .WithWallCountRange(
                    new IntRange(6 + level, 10 + (level * 2))
                )
                .WithHeightConfig(
                    new IntRange(5, 8 + level),
                    new IntRange(1, 3),
                    new IntRange(1, 1 + level)
                )
                .WithZoneVariantConfig(
                    new IntRange(3, 6),
                    new IntRange(2, 5)
                )
                .WithSpawns(
                    5,
                    numEnemies + 2
                )
                .WithBaseFloorVariant(TileTypeVariant.Stone)
                .WithFloorVariantWeights(
                    new ListWeightedOption<TileTypeVariant>(
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Stone, 50),
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Dirt, 40),
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Grass, 10)
                    )
                )
                .WithWallVariantWeights(
                    new ListWeightedOption<TileTypeVariant>(
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Stone, 100)
                    )
                )
                .WithWallHeightRules(2, 1, 1)
                .Build();
        }

        private static BattleMapGenerationConfig CreateBoss(int level, int numEnemies) {
            int size = EncounterType.Boss.GetSizeByLevel(level);
            return BattleMapGenerationConfig
                .Create()
                .WithSizeRange(
                    new IntRange(size, size),
                    new IntRange(size, size)
                )
                .WithWallCountRange(
                    new IntRange(10, 18)
                )
                .WithHeightConfig(
                    new IntRange(8, 14),
                    new IntRange(2, 4),
                    new IntRange(2, 3)
                )
                .WithZoneVariantConfig(
                    new IntRange(4, 7),
                    new IntRange(3, 6)
                )
                .WithSpawns(
                    4,
                    numEnemies + 2
                )
                .WithBaseFloorVariant(TileTypeVariant.Obsidian)
                .WithFloorVariantWeights(
                    new ListWeightedOption<TileTypeVariant>(
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Obsidian, 30),
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Stone, 50),
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Dirt, 15),
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Grass, 5)
                    )
                )
                .WithWallVariantWeights(
                    new ListWeightedOption<TileTypeVariant>(
                        new WeightedOption<TileTypeVariant>(TileTypeVariant.Stone, 100)
                    )
                )
                .WithWallHeightRules(2, 2, 1)
                .Build();
        }
    }
}
