namespace Game.Battle.Actions {
    using Map.Battle;
    using Map.Battle.Data;
    using Unit;

    public class SelectionData {
        public BattleMapManager BattleMapManager;
        public IBattleContext Context;
        public GridPosition Position;
        public UnitObject User;
    }
}
