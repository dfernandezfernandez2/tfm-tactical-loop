namespace Game.Unit.Skills.Effects {
    using System.Collections;
    using System.Collections.Generic;
    using Map.Battle;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Push")]
    public class PushSkillEffect : AbstractNonDeadSkillEffect {
        [SerializeField] private int distance;
        [SerializeField] private float pushSpeed = 4f;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            List<GridPosition> path = this.GetPushPath(user, target, battleMapManager);
            if (path.Count == 0) {
                yield break;
            }

            UnitObject targetUnit = battleMapManager.GetUnit(target);
            yield return targetUnit.MoveOnPath(path,
                (position, gridPosition) => battleMapManager.UnitMove(position, gridPosition, false), false,
                this.pushSpeed);
        }

        private List<GridPosition> GetPushPath(UnitObject user, GridPosition target,
            BattleMapManager battleMapManager) {
            Vector2Int direction = user.Unit.GridPosition.GetDirectionTo(target);
            GridPosition current = target;
            List<GridPosition> path = new();
            for (int i = 0; i < this.distance; i++) {
                GridPosition next = new(current.Position + direction, current.Height);
                if (!battleMapManager.IsAvailablePosition(next)) {
                    break;
                }

                path.Add(next);
                current = next;
            }

            return path;
        }
    }
}
