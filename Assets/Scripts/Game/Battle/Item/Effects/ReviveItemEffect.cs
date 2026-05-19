namespace Game.Battle.Item.Effects {
    using System.Collections;
    using Map.Battle;
    using Map.Battle.Data;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Revive Item effect")]
    public class ReviveItemEffect : ItemEffect {
        public int amount;

        public override bool CanApply(UnitObject target) => target.Unit.IsDead();

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject targetUnit = battleMapManager.GetUnit(target);
            Unit unit = targetUnit.Unit;
            // todo: aqui debería activarse el efecto de revivir con text la vida
            unit.AddStat(StatType.Hp, this.amount);
            yield return null;
        }
    }
}
