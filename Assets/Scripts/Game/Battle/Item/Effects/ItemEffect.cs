namespace Game.Battle.Item.Effects {
    using System.Collections;
    using Map.Battle;
    using Map.Battle.Data;
    using Unit;
    using UnityEngine;

    public abstract class ItemEffect : ScriptableObject {
        public abstract bool CanApply(UnitObject target);
        public abstract IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager);
    }
}
