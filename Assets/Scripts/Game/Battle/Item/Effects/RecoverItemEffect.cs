namespace Game.Battle.Item.Effects {
    using global::Unit.Data;
    using Map.Battle;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Recover Item effect")]
    public class RecoverItemEffect : ItemEffect {
        public StatType statType;
        public int amount;

        public override bool CanApply(UnitObject target) =>
            !target.GetUnit().IsDead() && !target.GetUnit().IsStatFull(this.statType);

        public override void Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject targetUnit = battleMapManager.GetUnit(target);
            Unit unit = targetUnit.GetUnit();
            int amountRecovered = (int)unit.GetStat(this.statType).Add(this.amount);
            // todo: aqui debería activarse el efecto de recuperación con text la cantidad
        }
    }
}
