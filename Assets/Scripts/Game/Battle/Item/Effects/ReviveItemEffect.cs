namespace Game.Battle.Item.Effects {
    using System.Collections;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Revive Item effect")]
    public class ReviveItemEffect : ItemEffect {
        public float amount;

        public override bool CanApply(UnitObject target) => target.Unit.IsDead();

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject targetUnit = battleMapManager.GetUnit(target);
            Unit unit = targetUnit.Unit;
            float maxHp = unit.GetMaxStat(StatType.Hp);
            float heal = unit.AddStat(StatType.Hp, Mathf.Max(1, maxHp * this.amount));
            yield return targetUnit.PlayRevive((int)heal);
        }
    }
}
