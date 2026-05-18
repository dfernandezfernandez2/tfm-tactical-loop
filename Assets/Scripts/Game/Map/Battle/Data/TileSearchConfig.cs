namespace Game.Map.Battle.Data {
    using System;
    using Game.Battle.Item;
    using Unit;

    public class TileSearchConfig {
        public bool ApplyHeightLineOfSight = false;
        public bool CanEnterCheck = true;
        public Func<UnitObject, bool> CanSelect = null;
        public int Range = -1;

        public bool RequiresLineOfSight = false;
        public Target Target = Target.None;
    }
}
