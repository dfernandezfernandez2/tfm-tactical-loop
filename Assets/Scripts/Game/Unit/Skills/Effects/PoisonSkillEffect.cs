namespace Game.Unit.Skills.Effects {
    using System.Collections;
    using Effect.Status;
    using Map.Battle;
    using Map.Battle.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Poison")]
    public class PoisonSkillEffect : AbstractNonDeadSkillEffect {
        [SerializeField] private int duration;
        [SerializeField] private int initialDamage;
        [SerializeField] private int damagePerTurn;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject unitObjectTarget = battleMapManager.GetUnit(target);
            yield return unitObjectTarget.EffectController.ApplyEffect(new PoisonEffect(this.duration,
                this.initialDamage, this.damagePerTurn));
        }
    }
}
