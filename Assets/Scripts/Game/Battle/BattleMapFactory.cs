namespace Game.Battle {
    using System;
    using System.Linq;
    using Data;
    using Map.Generation;
    using Run.Map;
    using Unit;
    using UnityEngine;

    public class BattleMapFactory : MonoBehaviour {
        [SerializeField] private UnitObject archerUnitObject;
        [SerializeField] private UnitObject knightUnitObject;
        [SerializeField] private UnitObject mageUnitObject;

        public BattleMapSetupData CreateMapFromNode(RunNode node) {
            Team enemyTeam = node.EncounterType switch {
                EncounterType.Basic => CreateTeam(this.archerUnitObject),
                EncounterType.Elite => CreateTeam(this.knightUnitObject, this.knightUnitObject, this.archerUnitObject,
                    this.mageUnitObject),
                EncounterType.Boss => CreateTeam(this.knightUnitObject, this.knightUnitObject, this.archerUnitObject,
                    this.archerUnitObject, this.mageUnitObject),
                _ => throw new ArgumentOutOfRangeException()
            };
            BattleMapGenerationConfig config =
                BattleMapGenerationConfigFactory.FromNode(node, enemyTeam.GetTeamUnits().Count);
            string map = BattleMapGenerator.Generate(config, CreateSeed(node));
            return new BattleMapSetupData(map, enemyTeam);
        }

        private static Team CreateTeam(params UnitObject[] unitObjectsPrefabs) =>
            new(unitObjectsPrefabs.ToList(), BattleTeam.Enemy);

        private static string CreateSeed(RunNode node) => $"{node.EncounterType}_L{node.Level}_{node.Id}";
    }
}
