namespace Game.Battle.Unit.Skills.Effects {
    using System.Collections;
    using System.Collections.Generic;
    using Data;
    using Map;
    using Map.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Area Damage")]
    public class AreaDamageSkillEffect : SkillEffect {
        [SerializeField] private int damage;
        [SerializeField] private int radius = 1;
        [SerializeField] private bool canFail;
        [SerializeField] private bool applyDefense;
        [SerializeField] private bool affectAllies;
        [SerializeField] private bool affectSelf;

        public override bool CanApply(UnitObject target) => true;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            IReadOnlyList<GridPosition> positionsAround =
                battleMapManager.GetPositionsAround(user.Unit.GridPosition, this.radius, this.affectSelf);
            foreach (GridPosition position in positionsAround) {
                UnitObject targetUnit = battleMapManager.GetUnit(position);
                if (!this.IsTarget(user, targetUnit)) {
                    continue;
                }

                AttackResult result = user.Unit.UnitDamageResolver.DoAttack(targetUnit.Unit, this.damage, this.canFail,
                    this.applyDefense);
                yield return BattleSequenceExecutor.PlayAttackResultAnimation(user, targetUnit, result);
            }
        }

        private bool IsTarget(UnitObject user, UnitObject targetUnit) {
            if (targetUnit == null || targetUnit.Unit.IsDead()) {
                return false;
            }

            bool isSelf = targetUnit == user;
            if (isSelf) {
                return this.affectSelf;
            }

            bool isAlly = targetUnit.Team.GetBattleTeam() == user.Team.GetBattleTeam();
            if (isAlly) {
                return this.affectAllies;
            }

            return true;
        }
    }
}
