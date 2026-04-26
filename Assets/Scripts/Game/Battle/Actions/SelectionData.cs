namespace Game.Battle.Actions {
    using Map.Battle;
    using Unit;

    public class SelectionData {
        public BattleMapManager BattleMapManager;
        public IBattleContext Context;
        public GridPosition Position;
        public UnitObject User;
    }
}
