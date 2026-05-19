namespace Game.Battle.Item.Effects {
    using System.Collections;
    using Effect.Recover;
    using Map.Battle;
    using Map.Battle.Data;
    using UI;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Effects/Recover Item effect")]
    public class RecoverItemEffect : ItemEffect {
        public StatType statType;
        public int amount;
        public Color color;

        public override bool CanApply(UnitObject target) =>
            !target.Unit.IsDead() && !target.Unit.IsStatFull(this.statType);

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject targetUnit = battleMapManager.GetUnit(target);
            Unit unit = targetUnit.Unit;
            int amountRecovered = (int)unit.AddStat(this.statType, this.amount);
            yield return targetUnit.EffectController.ApplyEffect(new RecoverEffect(this.statType, amountRecovered,
                this.color, CombatTextType.Heal));
        }
    }
}
