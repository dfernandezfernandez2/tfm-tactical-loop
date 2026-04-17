namespace Game.Battle.Item.Effects {
    using System;
    using global::Unit.Data;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Buff Item")]
    public class BuffItemEffect : ItemEffect {
        public StatType statType;
        public int amount;
        public int turnsDuration;

        public override bool CanApply(UnitObject user, UnitObject target) => throw new NotImplementedException();

        public override void Apply(UnitObject user, UnitObject target) => throw new NotImplementedException();
    }
}
