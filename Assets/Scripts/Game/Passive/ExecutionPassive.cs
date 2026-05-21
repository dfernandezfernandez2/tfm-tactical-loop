namespace Game.Passive {
    using System.Collections;
    using Battle.Unit;
    using Battle.Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Passive/Execution")]
    public class ExecutionPassive : Passive {
        [SerializeField] private float percentageOfLife = 0.2f;

        public override IEnumerator OnDamage(UnitObject userUnit, UnitObject targetUnit, int damage) {
            if (targetUnit.Unit.IsDead()) {
                yield break;
            }

            int currentHp = targetUnit.Unit.GetCurrentIntStat(StatType.Hp);
            int maxHp = (int)targetUnit.Unit.GetMaxStat(StatType.Hp);
            float currentHpPercentage = (float)currentHp / maxHp;

            if (!(currentHpPercentage <= this.percentageOfLife)) {
                yield break;
            }

            targetUnit.Unit.AddStat(StatType.Hp, -currentHp);
            yield return targetUnit.PlayDamage(currentHp);
        }
    }
}
