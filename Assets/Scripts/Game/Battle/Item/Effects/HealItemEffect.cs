namespace Game.Battle.Item.Effects {
    using System;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Heal Item")]
    public class HealItemEffect : ItemEffect {
        public int amount;

        public override bool CanApply(UnitObject user, UnitObject target) => throw new NotImplementedException();

        public override void Apply(UnitObject user, UnitObject target) => throw new NotImplementedException();
    }
}
