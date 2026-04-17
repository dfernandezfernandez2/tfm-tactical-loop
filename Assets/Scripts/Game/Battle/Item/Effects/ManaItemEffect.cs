namespace Game.Battle.Item.Effects {
    using System;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Mana Item")]
    public class ManaItemEffect : ItemEffect {
        public int amount;

        public override bool CanApply(UnitObject user, UnitObject target) => throw new NotImplementedException();

        public override void Apply(UnitObject user, UnitObject target) => throw new NotImplementedException();
    }
}
