namespace Game.Map.Battle {
    using Data;

    public class BattleLineOfSight {
        private readonly BattleMapData _mapData;

        public BattleLineOfSight(BattleMapData mapData) => this._mapData = mapData;

        public bool HasLineOfSight(GridPosition origin, GridPosition target) {
            return false;
        }
    }
}
