namespace Game.Battle.Item.Effects {
    using System;
    using global::Unit.Data;
    using Map.Battle;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Buff Item effect")]
    public class BuffItemEffect : ItemEffect {
        public StatType statType;
        public int amount;
        public int turnsDuration;

        public override bool CanApply(UnitObject target) => !target.GetUnit().IsDead();

        public override void Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) =>
            throw new NotImplementedException();
    }
}
