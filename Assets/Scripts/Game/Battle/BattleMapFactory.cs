namespace Game.Battle {
    using System;
    using System.Linq;
    using Core;
    using Data;
    using Map.Run;
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
            string mapText = ReadMapText(node.EncounterType.ToString());
            return new BattleMapSetupData(mapText, enemyTeam);
        }

        private static string ReadMapText(string name) {
            TextAsset map = Resources.Load<TextAsset>("Map/Battle/" + name);
            return map.text;
        }

        private static Team CreateTeam(params UnitObject[] unitObjectsPrefabs) =>
            new(unitObjectsPrefabs.ToList(), BattleTeam.Enemy);
    }
}
