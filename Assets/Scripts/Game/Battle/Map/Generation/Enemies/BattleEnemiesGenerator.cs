namespace Game.Battle.Map.Generation.Enemies {
    using System.Collections.Generic;
    using Battle.Data;
    using Data;
    using Run.Map;
    using Unit;
    using Random = System.Random;

    public class BattleEnemiesGenerator {
        private readonly BattleEnemiesGenerationConfig _config;

        public BattleEnemiesGenerator(BattleEnemiesGenerationConfig config) => this._config = config;

        public Team Generate(EncounterType encounterType, int level, string seed) {
            Random random = new(seed.GetHashCode());
            BattleEnemyEncounterTypeConfig config = this._config.GetConfigByEncounterType(encounterType);
            List<UnitObject> unitObjects = config.GetEnemyUnits(random, level);
            return new Team(unitObjects, BattleTeam.Enemy);
        }
    }
}
