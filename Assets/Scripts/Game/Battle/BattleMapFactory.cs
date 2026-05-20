namespace Game.Battle {
    using Data;
    using Map.Generation;
    using Map.Generation.Enemies;
    using Map.Generation.Enemies.Data;
    using Run.Map;
    using UnityEngine;

    public class BattleMapFactory : MonoBehaviour {
        [SerializeField] private BattleEnemiesGenerationConfig enemiesGenerationConfig;

        public BattleMapSetupData CreateMapFromNode(RunNode node) {
            string seed = CreateSeed(node);

            BattleEnemiesGenerator generator = new(this.enemiesGenerationConfig);
            Team enemyTeam = generator.Generate(node.EncounterType, node.Level, seed);

            BattleMapGenerationConfig config =
                BattleMapGenerationConfigFactory.FromNode(node, enemyTeam.GetTeamUnits().Count);
            string map = BattleMapGenerator.Generate(config, CreateSeed(node));

            return new BattleMapSetupData(map, enemyTeam);
        }

        private static string CreateSeed(RunNode node) => $"{node.EncounterType}_L{node.Level}_{node.Id}";
    }
}
