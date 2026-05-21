namespace Game.Passive {
    using System.Collections;
    using Battle.Effect.Recover;
    using Battle.Unit;
    using Battle.Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Passive/Life Steal")]
    public class LifeStealPassive : Passive {
        [SerializeField] private float percentageOfDamage = 0.1f;

        public override IEnumerator OnDamage(UnitObject userUnit, UnitObject targetUnit, int damage) {
            int heal = Mathf.Min(1, Mathf.FloorToInt(damage * this.percentageOfDamage));
            float recovered = userUnit.Unit.AddStat(StatType.Hp, heal);
            yield return targetUnit.EffectController.ApplyEffect(new HealRecoverEffect(recovered));
        }
    }
}
