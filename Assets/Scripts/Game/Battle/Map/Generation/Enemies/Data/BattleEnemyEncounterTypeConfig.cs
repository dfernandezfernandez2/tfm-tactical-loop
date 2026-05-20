namespace Game.Battle.Map.Generation.Enemies.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Generation.Data;
    using Run.Map;
    using Unit;
    using UnityEngine;
    using Random = System.Random;

    [Serializable]
    public class BattleEnemyEncounterTypeConfig {
        [SerializeField] private EncounterType encounterType;
        [SerializeField] private List<BattleEnemyOption> enemyOptions = new();
        [SerializeField] private int minNumUnits;
        [SerializeField] private int maxNumUnits;
        [SerializeField] private int unitsPerScale;
        [SerializeField] private int scalePerLevel;

        public bool IsFromType(EncounterType type) => type == this.encounterType;

        public List<UnitObject> GetEnemyUnits(Random random, int level) {
            int numberUnits = this.GetNumberUnits(random, level);
            Dictionary<BattleEnemyOption, int> appearances = new();
            List<UnitObject> result = new();
            // this is for bosses os special cases only so do not need to validate don't exceed the number of units
            foreach (BattleEnemyOption option in this.enemyOptions) {
                for (int i = 0; i < option.GuaranteedCount; i++) {
                    AddEnemy(result, appearances, option);
                }
            }

            while (result.Count < numberUnits) {
                List<BattleEnemyOption> availableOptions = this.enemyOptions
                    .Where(option => GetAppearances(appearances, option) < option.MaxAppearances).ToList();
                if (availableOptions.Count == 0) {
                    break;
                }

                WeightedOption<BattleEnemyOption>[] weightedOptions = availableOptions
                    .Select(option => new WeightedOption<BattleEnemyOption>(option, option.Weight))
                    .ToArray();
                BattleEnemyOption selectedOption =
                    new ListWeightedOption<BattleEnemyOption>(weightedOptions).Pick(random);
                AddEnemy(result, appearances, selectedOption);
            }

            return result;
        }

        private int GetNumberUnits(Random random, int level) {
            int baseUnits = random.Next(this.minNumUnits, this.maxNumUnits + 1);
            int scaleSteps = Mathf.Max(0, level - 1) / this.scalePerLevel;
            int scaledUnits = baseUnits + (scaleSteps * Mathf.Max(0, this.unitsPerScale));
            return Mathf.Min(scaledUnits, this.maxNumUnits);
        }

        private static void AddEnemy(ICollection<UnitObject> result, IDictionary<BattleEnemyOption, int> appearances,
            BattleEnemyOption option) {
            result.Add(option.EnemyPrefab);
            appearances[option] = GetAppearances(appearances, option) + 1;
        }

        private static int GetAppearances(IDictionary<BattleEnemyOption, int> appearances, BattleEnemyOption option) =>
            appearances.TryGetValue(option, out int count) ? count : 0;
    }
}
