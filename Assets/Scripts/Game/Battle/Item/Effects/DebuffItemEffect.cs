namespace Game.Battle.Item.Effects {
    using System;
    using Effect.Debuff;
    using global::Unit.Data;
    using Map.Battle;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Debuff Item effect")]
    public class DebuffItemEffect : ItemEffect {
        public StatType statType;
        public int amount;
        public int turnsDuration;

        public override bool CanApply(UnitObject target) => !target.GetUnit().IsDead();

        public override void Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) =>
            user.GetUnit().ApplyEffect(new DebuffEffect(this.turnsDuration, this.statType, this.amount));
    }
}
