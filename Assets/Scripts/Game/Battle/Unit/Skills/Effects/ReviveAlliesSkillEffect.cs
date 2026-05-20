namespace Game.Battle.Unit.Skills.Effects {
    using System.Collections;
    using System.Collections.Generic;
    using Data;
    using Map;
    using Map.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Revive Allies")]
    public class ReviveAlliesSkillEffect : SkillEffect {
        [SerializeField] private int radius = 1;
        [SerializeField] private int amount = 20;
        [SerializeField] private int maxRevives = 2;

        public override bool CanApply(UnitObject target) => true;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            IReadOnlyList<GridPosition> positionsAround =
                battleMapManager.GetPositionsAround(user.Unit.GridPosition, this.radius);

            int revivedUnits = 0;

            foreach (GridPosition position in positionsAround) {
                if (revivedUnits >= this.maxRevives) {
                    yield break;
                }

                UnitObject targetUnit = battleMapManager.GetUnit(position);
                if (!IsTarget(user, targetUnit)) {
                    continue;
                }

                float recovered = targetUnit.Unit.AddStat(StatType.Hp, this.amount);
                yield return targetUnit.PlayRevive((int)recovered);
                revivedUnits++;
            }
        }

        private static bool IsTarget(UnitObject user, UnitObject targetUnit) {
            if (targetUnit == null || !targetUnit.Unit.IsDead()) {
                return false;
            }

            return targetUnit.Team.GetBattleTeam() == user.Team.GetBattleTeam();
        }
    }
}
