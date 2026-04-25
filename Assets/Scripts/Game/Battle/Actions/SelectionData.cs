namespace Game.Battle.Actions {
    using Map.Battle;
    using Unit;

    public class SelectionData {
        public UnitObject User;
        public GridPosition Position;
        public BattleMapManager BattleMapManager;
        public IBattleContext Context;
    }
}
