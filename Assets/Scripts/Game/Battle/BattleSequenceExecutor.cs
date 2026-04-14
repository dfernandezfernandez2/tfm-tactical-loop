namespace Game.Battle {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Map.Battle;
    using Unit;

    public static class BattleSequenceExecutor {
        public static IEnumerator ExecuteMovement(UnitObject unit, IReadOnlyList<GridPosition> path,
            Action<GridPosition, GridPosition> onMove) {
            yield return unit.StartCoroutine(unit.MoveOnPath(path, onMove));
        }

        public static IEnumerator ExecuteBasicAttack(UnitObject attacker, UnitObject target, GridPosition targetPosition) {
            yield return attacker.PlayBasicAttack(targetPosition);
            AttackResult result = attacker.GetUnit().DoBasicAttack(target?.GetUnit());
            if (!result.GetHit()) {
                if (target != null && !result.IsTargetDead()) {
                    yield return target.PlayDodge(() => attacker.StartCoroutine(attacker.PlayMiss()));
                }
                else {
                    yield return attacker.PlayMiss();
                }

                yield break;
            }

            // could not be null, if its null could never be a hit
            yield return target.PlayDamage(result);
        }
    }
}
