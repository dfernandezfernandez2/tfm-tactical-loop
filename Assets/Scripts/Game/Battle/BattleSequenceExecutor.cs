namespace Game.Battle {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Map;
    using Map.Data;
    using Passive;
    using Run.Data;
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
            AttackResult result = attacker.Unit.UnitDamageResolver.DoAttack(target?.Unit);
            yield return PlayAttackResultAnimation(attacker, target, result);
            if (!attackTeam) {
                yield break;
            }

            if (target == null) {
                yield break;
            }

            if (attacker.Team.GetBattleTeam() == target.Team.GetBattleTeam()) {
                yield break;
            }

            IReadOnlyList<UnitObject> unitObjects = battleMapManager.GetUnitsAround(targetPosition);
            foreach (UnitObject unit in unitObjects) {
                if (unit.Team.GetBattleTeam() != target.Team.GetBattleTeam() && unit != attacker &&
                    !target.Unit.IsDead() && !unit.Unit.IsDead()) {
                    yield return ExecuteBasicAttack(unit, target, targetPosition, battleMapManager, false);
                }
            }
        }

        public static IEnumerator
            PlayAttackResultAnimation(UnitObject attacker, UnitObject target, AttackResult result) {
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
            foreach (IPassive passive in RunData.GetInstance().Passives) {
                yield return passive.OnDamage(attacker, target, result.GetDamage());
            }
        }
    }
}
