namespace Game.Unit {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Battle.UI;
    using Core;
    using Data;
    using global::Unit.Data;
    using Map.Battle;
    using UnityEngine;

    public enum AnimationType {
        Attack,
        Damage,
        Death,
        Dodge
    }

    internal readonly struct UnitLayer {
        private readonly Dictionary<Vector2Int, int> _directionsLayers;

        public UnitLayer(params KeyValuePair<int, Vector2Int>[] directionsLayers) {
            this._directionsLayers = new Dictionary<Vector2Int, int>();
            foreach (KeyValuePair<int, Vector2Int> directionsLayer in directionsLayers) {
                this._directionsLayers.Add(directionsLayer.Value, directionsLayer.Key);
            }
        }

        public List<KeyValuePair<int, float>> GetChangeLayer(Vector2Int direction) => this._directionsLayers
            .Select(keyValuePair =>
                new KeyValuePair<int, float>(keyValuePair.Value, direction == keyValuePair.Key ? 1f : 0f)).ToList();
    }

    public static class AnimationTypeExtensions {
        public static string GetAnimationEndName(this AnimationType animationType) =>
            "signal.end." + animationType.ToString().ToLower();

        public static string GetAnimationText(this AnimationType animationType) =>
            "signal.text." + animationType.ToString().ToLower();
    }

    [RequireComponent(typeof(Animator))]
    public class UnitObject : MonoBehaviour {
        private static readonly int _isMoving = Animator.StringToHash("isMoving");

        [SerializeField] private UnitData data;
        [SerializeField] private CombatTextUI combatTextUI;
        [SerializeField] private WorldRender worldRender;

        private readonly Dictionary<string, int> _signalCounters = new();

        private Animator _animator;
        private Team _team;
        private Unit _unit;
        private UnitLayer _unitLayer;

        public void Awake() {
            this._animator = this.GetComponent<Animator>();
            Stats stats = new Stats.Builder()
                .With(StatType.Hp, this.data.hp)
                .With(StatType.Mp, this.data.mp)
                .With(StatType.MpRegen, this.data.mpRegen)
                .With(StatType.Movement, this.data.movement)
                .With(StatType.AP, this.data.ap)
                .With(StatType.Atk, this.data.atk)
                .With(StatType.Def, this.data.defense)
                .With(StatType.Accuracy, this.data.accuracy)
                .With(StatType.Evasion, this.data.evasion)
                .With(StatType.CritChance, this.data.critChance)
                .With(StatType.Range, this.data.range)
                .With(StatType.Speed, this.data.speed)
                .Build();
            this._unit = new Unit(stats);
            this._unitLayer = new UnitLayer(
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Down"), Vector2Int.down),
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Up"), Vector2Int.up),
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Right"), Vector2Int.right),
                new KeyValuePair<int, Vector2Int>(this._animator.GetLayerIndex("Left"), Vector2Int.left)
            );
        }

        public void Init(GridPosition gridPosition, Vector2Int direction) {
            this._unit.Move(gridPosition, direction);
            this.UpdateDirection(direction);
            this.transform.position = this.worldRender.GridToWorld(gridPosition);
        }

        public Unit GetUnit() => this._unit;

        public IEnumerator MoveOnPath(IReadOnlyList<GridPosition> path, Action<GridPosition, GridPosition> onMove) {
            GridPosition currentPosition = this._unit.GetGridPosition();
            foreach (GridPosition pos in path) {
                Vector2Int direction = currentPosition.GetDirectionTo(pos);
                this.UpdateDirection(direction);
                this._unit.Move(pos, direction);
                Vector3 target = this.worldRender.GridToWorld(pos);
                GridPosition position = currentPosition;
                yield return this.StartCoroutine(this.MoveRoutine(target, () => onMove(position, pos)));
                currentPosition = pos;
            }
        }

        private IEnumerator MoveRoutine(Vector3 target, Action onHalfMovement) {
            const float speed = 2f;
            float time = 0f;
            Vector3 start = this.transform.position;
            float distance = Vector3.Distance(start, target);
            float duration = distance / speed;
            this._animator.SetBool(_isMoving, true);

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
            this._animator.SetBool(_isMoving, false);
        }

        public void Signal(string signalId) =>
            this._signalCounters[signalId] = this._signalCounters.GetValueOrDefault(signalId, 0) + 1;

        private IEnumerator WaitForSignal(string signalId, int version) {
            yield return new WaitUntil(() => this._signalCounters.GetValueOrDefault(signalId, 0) > version);
        }

        private int GetSignalVersion(string signalId) => this._signalCounters.GetValueOrDefault(signalId, 0);

        public IEnumerator PlayBasicAttack(GridPosition targetPosition) {
            this.UpdateDirection(this._unit.GetGridPosition().GetDirectionTo(targetPosition));
            const AnimationType animationType = AnimationType.Attack;
            int endVersion = this.GetSignalVersion(animationType.GetAnimationEndName());
            this._animator.ResetTrigger(animationType.ToString());
            this._animator.SetTrigger(animationType.ToString());
            yield return this.WaitForSignal(animationType.GetAnimationEndName(), endVersion);
        }

        public IEnumerator PlayDamage(AttackResult attackResult) {
            const AnimationType animationType = AnimationType.Damage;
            int signalTextVersion = this.GetSignalVersion(animationType.GetAnimationText());
            int signalEndVersion = this.GetSignalVersion(animationType.GetAnimationEndName());
            this._animator.ResetTrigger(animationType.ToString());
            this._animator.SetTrigger(animationType.ToString());
            yield return this.WaitForSignal(animationType.GetAnimationText(), signalTextVersion);
            this.combatTextUI.Init(attackResult.GetDamage().ToString(),
                attackResult.IsCritical() ? CombatTextType.Crit : CombatTextType.Hit);
            yield return this.WaitForSignal(animationType.GetAnimationEndName(), signalEndVersion);
            if (this._unit.IsDead()) {
                yield return this.PlayDeath();
            }
        }

        public IEnumerator PlayMiss() {
            this.combatTextUI.Init(CombatTextType.Miss);
            yield return null;
        }

        public IEnumerator PlayDeath() {
            const AnimationType animationType = AnimationType.Death;
            int signalEndVersion = this.GetSignalVersion(animationType.GetAnimationEndName());
            this._animator.ResetTrigger(animationType.ToString());
            this._animator.SetTrigger(animationType.ToString());
            yield return this.WaitForSignal(animationType.GetAnimationEndName(), signalEndVersion);
        }

        public IEnumerator PlayDodge(Action onDodge) {
            const AnimationType animationType = AnimationType.Dodge;
            int signalTextVersion = this.GetSignalVersion(animationType.GetAnimationText());
            int signalEndVersion = this.GetSignalVersion(animationType.GetAnimationEndName());
            this._animator.ResetTrigger(animationType.ToString());
            this._animator.SetTrigger(animationType.ToString());
            yield return this.WaitForSignal(animationType.GetAnimationText(), signalTextVersion);
            onDodge();
            yield return this.WaitForSignal(animationType.GetAnimationEndName(), signalEndVersion);
        }

        public Team GetTeam() => this._team;
        public void SetTeam(Team team) => this._team = team;
        public string GetName() => this.data.unitName;
        public Sprite GetSprite() => this.data.unitSprite;

        public List<KeyValuePair<StatType, float>> GetStats() {
            List<KeyValuePair<StatType, float>> stats = new() {
                new KeyValuePair<StatType, float>(StatType.Hp, this.data.hp),
                new KeyValuePair<StatType, float>(StatType.Mp, this.data.mp),
                new KeyValuePair<StatType, float>(StatType.MpRegen, this.data.mpRegen),
                new KeyValuePair<StatType, float>(StatType.Movement, this.data.movement),
                new KeyValuePair<StatType, float>(StatType.AP, this.data.ap),
                new KeyValuePair<StatType, float>(StatType.Atk, this.data.atk),
                new KeyValuePair<StatType, float>(StatType.Def, this.data.defense),
                new KeyValuePair<StatType, float>(StatType.Accuracy, this.data.accuracy),
                new KeyValuePair<StatType, float>(StatType.Evasion, this.data.evasion),
                new KeyValuePair<StatType, float>(StatType.CritChance, this.data.critChance),
                new KeyValuePair<StatType, float>(StatType.Range, this.data.range),
                new KeyValuePair<StatType, float>(StatType.Speed, this.data.speed)
            };
            return stats;
        }

        private void UpdateDirection(Vector2Int direction) {
            foreach (KeyValuePair<int, float> keyValuePair in this._unitLayer.GetChangeLayer(direction)) {
                this._animator.SetLayerWeight(keyValuePair.Key, keyValuePair.Value);
            }

            this._unit.UpdateDirection(direction);
        }
    }
}
