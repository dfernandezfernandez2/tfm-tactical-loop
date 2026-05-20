namespace Game.Battle.Unit.Skills.Effects {
    using System.Collections;
    using Data;
    using Effect.Recover;
    using Map;
    using Map.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Heal")]
    public class HealSkillEffect : SkillEffect {
        [SerializeField] private int amount;

        public override bool CanApply(UnitObject target) =>
            target != null && !target.Unit.IsDead() && !target.Unit.IsStatFull(StatType.Hp);

        public override IEnumerator Apply(UnitObject user, GridPosition targetPosition,
            BattleMapManager battleMapManager) {
            UnitObject target = battleMapManager.GetUnit(targetPosition);
            if (target == null) {
                yield break;
            }

            int recovered = (int)target.Unit.AddStat(StatType.Hp, this.amount);
            yield return target.EffectController.ApplyEffect(new HealRecoverEffect(recovered));
        }
    }
}
