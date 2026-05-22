namespace Game.Passive {
    using System.Collections;
    using Battle.Data;
    using Battle.Effect.Recover;
    using Battle.Unit;
    using Battle.Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Passive/Execution")]
    public class ExecutionPassive : Passive {
        [SerializeField] private float percentageOfLife = 0.2f;

        public override IEnumerator OnDamage(UnitObject userUnit, UnitObject targetUnit, int damage) {
            if (targetUnit.Unit.IsDead() || userUnit.Team.GetBattleTeam() != BattleTeam.Player) {
                yield break;
            }

            float currentHp = targetUnit.Unit.GetCurrentStat(StatType.Hp);
            float maxHp = targetUnit.Unit.GetMaxStat(StatType.Hp);
            float currentHpPercentage = currentHp / maxHp;

            if (!(currentHpPercentage <= this.percentageOfLife)) {
                yield break;
            }

            targetUnit.Unit.AddStat(StatType.Hp, -currentHp);
            yield return new WaitForSeconds(0.15f);
            yield return targetUnit.EffectController.ApplyEffect(new ExecutionEffect(currentHp));
            yield return targetUnit.PlayDamage((int)currentHp, false);
        }
    }
}
