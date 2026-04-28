namespace Game.Battle.Item.Effects {
    using System.Collections;
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
        public Color color;

        public override bool CanApply(UnitObject target) => !target.Unit.IsDead();

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject targetUnitObject = battleMapManager.GetUnit(target);
            yield return targetUnitObject.EffectController.ApplyEffect(new DebuffEffect(this.turnsDuration,
                this.statType,
                this.amount, this.color));
        }
    }
}
