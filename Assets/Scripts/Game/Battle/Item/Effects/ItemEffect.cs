namespace Game.Battle.Item.Effects {
    using Map.Battle;
    using Unit;
    using UnityEngine;

    public abstract class ItemEffect : ScriptableObject {
        public abstract bool CanApply(UnitObject target);
        public abstract void Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager);
    }
}
