namespace Game.Battle.Item.Effects {
    using global::Unit.Data;
    using Map.Battle;
    using Unit;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Revive Item effect")]
    public class ReviveItemEffect : ItemEffect {
        public int amount;

        public override bool CanApply(UnitObject target) => target.GetUnit().IsDead();

        public override void Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject targetUnit = battleMapManager.GetUnit(target);
            Unit unit = targetUnit.GetUnit();
            // todo: aqui debería activarse el efecto de revivir con text la vida
            unit.GetStat(StatType.Hp).Add(this.amount);
        }
    }
}
