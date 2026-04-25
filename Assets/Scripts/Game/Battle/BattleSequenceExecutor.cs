namespace Game.Battle {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Map.Battle;
    using Unit;
    using Unit.Data;

    public static class BattleSequenceExecutor {
        public static IEnumerator ExecuteMovement(UnitObject unit, IReadOnlyList<GridPosition> path,
            Action<GridPosition, GridPosition> onMove) {
            yield return unit.MoveOnPath(path, onMove);
        }

        public static IEnumerator ExecuteBasicAttack(UnitObject attacker, UnitObject target,
            GridPosition targetPosition, BattleMapManager battleMapManager, bool attackTeam = true) {
            yield return attacker.PlayBasicAttack(targetPosition);
            AttackResult result = attacker.Unit.UnitDamageResolver.DoBasicAttack(target?.Unit);
            if (!result.GetHit()) {
                if (target != null && !result.IsTargetDead()) {
                    yield return target.PlayDodge(attacker);
                }
                else {
                    yield return attacker.PlayMiss();
                }

                yield break;
            }

            // could not be null, if its null could never be a hit
            yield return target.PlayDamage(result);
            if (!attackTeam) {
                yield break;
            }

            if (attacker.Team.GetBattleTeam() == target.Team.GetBattleTeam()) {
                yield break;
            }

            IReadOnlyList<UnitObject> unitObjects = battleMapManager.GetUnitsAround(targetPosition);
            foreach (UnitObject unit in unitObjects) {
                if (unit.Team.GetBattleTeam() != target.Team.GetBattleTeam() && unit != attacker &&
                    !target.Unit.IsDead()) {
                    yield return ExecuteBasicAttack(unit, target, targetPosition, battleMapManager, false);
                }
            }
        }
    }
}
