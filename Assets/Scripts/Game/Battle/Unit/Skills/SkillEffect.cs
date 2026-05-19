namespace Game.Battle.Unit.Skills {
    using System.Collections;
    using Map;
    using Map.Data;
    using UnityEngine;

    public abstract class SkillEffect : ScriptableObject {
        public abstract bool CanApply(UnitObject target);
        public abstract IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager);
    }
}
