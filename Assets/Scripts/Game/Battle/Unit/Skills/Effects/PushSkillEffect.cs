namespace Game.Battle.Unit.Skills.Effects {
    using System.Collections;
    using System.Collections.Generic;
    using Map;
    using Map.Data;
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
                Vector2Int nextPosition = current.Position + direction;
                TileData nextTile = battleMapManager.GetTile(nextPosition);
                if (nextTile == null) {
                    break;
                }

                GridPosition next = new(current.Position + direction, current.Height);
                if (!battleMapManager.IsAvailablePosition(next) || current.Height < next.Height) {
                    break;
                }

                path.Add(next);
                // if the unit fall in above height falls but not continue the push movement
                if (current.Height > next.Height) {
                    break;
                }

                current = next;
            }

            return path;
        }
    }
}
