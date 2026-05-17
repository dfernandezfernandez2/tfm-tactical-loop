namespace Game.Map.Battle.Data {
    using System;
    using Game.Battle.Item;
    using Unit;

    public class TileSearchConfig {
        public int Range = -1;
        public bool CanEnterCheck = true;
        public Target Target = Target.None;
        public Func<UnitObject, bool> CanSelect = null;

        public bool RequiresLineOfSight = false;
        public bool ApplyHeightLineOfSight = false;
    }
}
