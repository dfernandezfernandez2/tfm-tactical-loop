namespace Game.Passive {
    using System.Collections;
    using Battle.Data;
    using Battle.Effect.Recover;
    using Battle.Unit;
    using Battle.Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Passive/Life Steal")]
    public class LifeStealPassive : Passive {
        [SerializeField] private float percentageOfDamage = 0.1f;

        public override IEnumerator OnDamage(UnitObject userUnit, UnitObject targetUnit, int damage) {
            if (userUnit.Team.GetBattleTeam() != BattleTeam.Player) {
                yield break;
            }

            int heal = Mathf.Min(1, Mathf.FloorToInt(damage * this.percentageOfDamage));
            float recovered = userUnit.Unit.AddStat(StatType.Hp, heal);
            if (recovered <= 0) {
                yield break;
            }

            yield return userUnit.EffectController.ApplyEffect(new LifeSteelEffect(recovered));
        }
    }
}
