namespace Game.Battle.Item.Effects {
    using Unit;
    using UnityEngine;

    public abstract class ItemEffect : ScriptableObject {
        public abstract bool CanApply(UnitObject user, UnitObject target);
        public abstract void Apply(UnitObject user, UnitObject target);
    }
}
