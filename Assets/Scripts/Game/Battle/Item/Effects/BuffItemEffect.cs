namespace Game.Battle.Item.Effects {
    using System.Collections;
    using Effect.Buff;
    using global::Unit.Data;
    using Map.Battle;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Buff Item effect")]
    public class BuffItemEffect : ItemEffect {
        public StatType statType;
        public int amount;
        public int turnsDuration;
        public Color color;

        public override bool CanApply(UnitObject target) => !target.Unit.IsDead();

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            yield return user.EffectController.ApplyEffect(new BuffEffect(this.turnsDuration, this.statType,
                this.amount, this.color));
        }
    }
}
