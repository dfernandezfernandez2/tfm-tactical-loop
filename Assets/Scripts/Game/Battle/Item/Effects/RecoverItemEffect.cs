namespace Game.Battle.Item.Effects {
    using System.Collections;
    using global::Unit.Data;
    using Map.Battle;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Recover Item effect")]
    public class RecoverItemEffect : ItemEffect {
        public StatType statType;
        public int amount;

        public override bool CanApply(UnitObject target) =>
            !target.Unit.IsDead() && !target.Unit.IsStatFull(this.statType);

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject targetUnit = battleMapManager.GetUnit(target);
            Unit unit = targetUnit.Unit;
            int amountRecovered = (int)unit.AddStat(this.statType, this.amount);
            // todo: aqui debería activarse el efecto de recuperación con text la cantidad
            yield return null;
        }
    }
}
