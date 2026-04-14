namespace Game.Core {
    using System.Collections.Generic;
    using Unit;

    public class Team {
        private readonly BattleTeam _battleTeam;
        private readonly List<UnitObject> _unitObjects;

        private readonly IReadOnlyList<UnitObject> _unitObjectsPrefabs;

        public Team(List<UnitObject> unitObjectsPrefabs, BattleTeam battleTeam) {
            this._unitObjectsPrefabs = unitObjectsPrefabs.AsReadOnly();
            this._battleTeam = battleTeam;
            this._unitObjects = new List<UnitObject>();
        }

        public IReadOnlyList<UnitObject> GetUnitObjectsPrefabs() => this._unitObjectsPrefabs;

        public void AddUnit(UnitObject unitObject) {
            unitObject.SetTeam(this);
            this._unitObjects.Add(unitObject);
        }

        public void RemoveUnit(UnitObject unitObject) {
            unitObject.SetTeam(null);
            this._unitObjects.Remove(unitObject);
        }

        public IReadOnlyList<UnitObject> GetUnitObjects() => this._unitObjects.AsReadOnly();
        public BattleTeam GetBattleTeam() => this._battleTeam;
    }
}
