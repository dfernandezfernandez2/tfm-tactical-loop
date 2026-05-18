namespace Game.Unit {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Battle.UI;
    using Core;
    using Data;
    using global::Unit.Data;
    using Map.Battle;
    using Map.Battle.Data;
    using Skills;
    using UnityEngine;

    [RequireComponent(typeof(UnitAnimationController))]
    [RequireComponent(typeof(UnitEffectController))]
    [RequireComponent(typeof(UnitActions))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class UnitObject : MonoBehaviour {
        public UnitData data;
        [SerializeField] private CombatTextUI combatTextUI;
        [SerializeField] private WorldRender worldRender;
        private UnitAnimationController _animator;
        private SpriteRenderer _renderer;

        public Team Team { get; set; }
        public Unit Unit { get; private set; }
        public UnitEffectController EffectController { get; private set; }
        public UnitActions Actions { get; private set; }

        public void Awake() {
            this._animator = this.GetComponent<UnitAnimationController>();
            this.EffectController = this.GetComponent<UnitEffectController>();
            this.EffectController.Init(this);
            this.Actions = this.GetComponent<UnitActions>();
            this._renderer = this.GetComponent<SpriteRenderer>();
        }

        public void Init(Unit unit) => this.Unit = unit;

        public string GetName() => this.data.unitName;
        public Sprite GetSprite() => this.data.unitSprite;

        public List<KeyValuePair<StatType, float>> GetStatsInfo() => this.data.GetStatsInfo();

        public void InitPosition(GridPosition gridPosition, Vector2Int direction) {
            this.Unit.Move(gridPosition, direction);
            this.UpdateDirection(direction);
            this.transform.position = this.worldRender.GridToWorld(gridPosition);
            this._renderer.sortingOrder = WorldRender.GetSortingOrder(gridPosition);
        }

        public IEnumerator OnTurnStart() {
            this.Unit.RestoreStat(StatType.AP);
            this.Unit.AddStat(StatType.Mp, this.Unit.GetCurrentStat(StatType.MpRegen));
            yield return this.EffectController.OnTurnStart();
        }

        public IEnumerator OnTurnEnd() {
            yield return this.EffectController.OnTurnEnd();
        }

        public IEnumerator MoveOnPath(IReadOnlyList<GridPosition> path, Action<GridPosition, GridPosition> onMove,
            bool playMoveAnimation = true, float speed = 2f) {
            GridPosition currentPosition = this.Unit.GridPosition;
            foreach (GridPosition pos in path) {
                Vector2Int direction = currentPosition.GetDirectionTo(pos);
                this.UpdateDirection(direction);
                this.Unit.Move(pos, direction);
                Vector3 target = this.worldRender.GridToWorld(pos);
                GridPosition position = currentPosition;
                yield return this.MoveRoutine(target, () => onMove(position, pos), playMoveAnimation, speed);
                currentPosition = pos;
                this._renderer.sortingOrder = WorldRender.GetSortingOrder(pos);
            }
        }

        private IEnumerator MoveRoutine(Vector3 target, Action onHalfMovement, bool playMoveAnimation = true,
            float speed = 2f) {
            float time = 0f;
            Vector3 start = this.transform.position;
            float distance = Vector3.Distance(start, target);
            float duration = distance / speed;
            if (playMoveAnimation) {
                this._animator.SetMoving(true);
            }

            bool halfTriggered = false;
            while (time < duration) {
                time += Time.deltaTime;
                this.transform.position = Vector3.Lerp(start, target, time / duration);
                if (!halfTriggered && time >= duration / 2f) {
                    halfTriggered = true;
                    onHalfMovement.Invoke();
                }

                yield return null;
            }

            this.transform.position = target;
            if (playMoveAnimation) {
                this._animator.SetMoving(false);
            }
        }

        public IEnumerator PlayBasicAttack(GridPosition targetPosition) {
            this.UpdateDirection(this.Unit.GridPosition.GetDirectionTo(targetPosition));
            yield return this._animator.PlayAnimation(AnimationType.Attack);
        }

        public IEnumerator PlayDamage(int damage) {
            yield return this.PlayDamage(AttackResult.Hit(damage, false, false));
        }

        public IEnumerator PlayDamage(AttackResult attackResult) {
            yield return this._animator.PlayAnimation(AnimationType.Damage, OnText);
            if (this.Unit.IsDead()) {
                yield return this.PlayDeath();
            }

            yield break;

            IEnumerator OnText() {
                this.combatTextUI.Init(attackResult.GetDamage().ToString(),
                    attackResult.IsCritical() ? CombatTextType.Crit : CombatTextType.Hit);
                yield return null;
            }
        }

        public IEnumerator PlayMiss() {
            this.combatTextUI.Init(CombatTextType.Miss);
            yield return null;
        }

        public IEnumerator PlayDeath() {
            yield return this._animator.PlayAnimation(AnimationType.Death);
        }

        public IEnumerator PlayDodge(UnitObject attacker) {
            Vector2Int initialDirection = this.Unit.Direction;
            Vector2Int attackerDirection = attacker.Unit.Direction;
            Vector2Int dodgeDirection = -attackerDirection;
            this.UpdateDirection(dodgeDirection);
            yield return this._animator.PlayAnimation(AnimationType.Dodge, attacker.PlayMiss);
            this.UpdateDirection(initialDirection);
        }

        public void PlayText(string message, CombatTextType type) => this.combatTextUI.Init(message, type);

        public IEnumerator PlaySkill(Skill skill, GridPosition targetPosition) {
            if (!targetPosition.Equals(this.Unit.GridPosition)) {
                this.UpdateDirection(this.Unit.GridPosition.GetDirectionTo(targetPosition));
            }

            if (string.IsNullOrEmpty(skill.animationName)) {
                yield break;
            }

            yield return this._animator.PlayAnimation(skill.animationName);
        }

        private void UpdateDirection(Vector2Int direction) {
            this._animator.UpdateDirection(direction);
            this.Unit.Direction = direction;
        }
    }
}
