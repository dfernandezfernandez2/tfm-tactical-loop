namespace Game.Unit.Skills.Effects {
    using System.Collections;
    using Map.Battle;
    using UnityEngine;

    public abstract class SkillEffect : ScriptableObject {
        public abstract bool CanApply(UnitObject target);
        public abstract IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager);
    }
}
