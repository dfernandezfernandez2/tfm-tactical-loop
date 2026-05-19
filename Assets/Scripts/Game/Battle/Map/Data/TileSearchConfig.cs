namespace Game.Battle.Map.Data {
    using System;
    using Item;
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
