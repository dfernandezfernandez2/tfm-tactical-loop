namespace Game.Battle.Map.Generation.Enemies.Data {
    using System;
    using System.Collections.Generic;
    using Run.Map;
    using UnityEngine;

    [Serializable]
    public class BattleEnemiesGenerationConfig {
        [SerializeField] private List<BattleEnemyEncounterTypeConfig> enemiesPool = new();

        public BattleEnemyEncounterTypeConfig GetConfigByEncounterType(EncounterType encounterType) =>
            this.enemiesPool.Find(en => en.IsFromType(encounterType));
    }
}
