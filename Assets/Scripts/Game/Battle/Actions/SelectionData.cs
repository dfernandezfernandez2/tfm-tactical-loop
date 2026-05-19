namespace Game.Battle.Actions {
    using Map;
    using Map.Data;
    using Unit;

    public class SelectionData {
        public BattleMapManager BattleMapManager;
        public IBattleContext Context;
        public GridPosition Position;
        public UnitObject User;
    }
}
