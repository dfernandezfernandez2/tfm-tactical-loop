namespace Game.Unit.Skills.Effects {
    using System.Collections;
    using Battle;
    using Data;
    using Map.Battle;
    using Map.Battle.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Damage")]
    public class DamageSkillEffect : AbstractNonDeadSkillEffect {
        [SerializeField] private int damage;
        [SerializeField] private bool canFail;
        [SerializeField] private bool applyDefense;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject unitObjectTarget = battleMapManager.GetUnit(target);
            AttackResult result = user.Unit.UnitDamageResolver.DoAttack(unitObjectTarget.Unit, this.damage,
                this.canFail, this.applyDefense);
            yield return BattleSequenceExecutor.PlayAttackResultAnimation(user, unitObjectTarget, result);
        }
    }
}
