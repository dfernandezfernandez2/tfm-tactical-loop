namespace Game.Battle.Data {
    using System.Collections.Generic;
    using Unit;

    public struct TeamUnit {
        public TeamUnit(Unit unitData, UnitObject prefab) {
            this.UnitData = unitData;
            this.Prefab = prefab;
        }

        public readonly Unit UnitData;
        public readonly UnitObject Prefab;
    }

    public class Team {
        private readonly BattleTeam _battleTeam;
        private readonly List<TeamUnit> _teamUnits = new();
        private readonly List<UnitObject> _unitObjects;

        public Team(List<UnitObject> unitObjectsPrefabs, BattleTeam battleTeam) {
            foreach (UnitObject unitObjectsPrefab in unitObjectsPrefabs) {
                Unit unit = new(unitObjectsPrefab.data.GetStats());
                this._teamUnits.Add(new TeamUnit(unit, unitObjectsPrefab));
            }

            this._battleTeam = battleTeam;
            this._unitObjects = new List<UnitObject>();
        }

        public IReadOnlyList<TeamUnit> GetTeamUnits() => this._teamUnits;

        public void AddUnit(UnitObject unitObject, TeamUnit teamUnit) {
            unitObject.Init(teamUnit.UnitData);
            unitObject.Team = this;
            this._unitObjects.Add(unitObject);
        }

        public void RemoveUnit(UnitObject unitObject) {
            unitObject.Team = null;
            this._unitObjects.Remove(unitObject);
        }

        public void ClearUnitObjects() => this._unitObjects.Clear();

        public IReadOnlyList<UnitObject> GetUnitObjects() => this._unitObjects.AsReadOnly();
        public BattleTeam GetBattleTeam() => this._battleTeam;
    }
}
