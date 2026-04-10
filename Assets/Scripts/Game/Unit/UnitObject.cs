namespace Game.Unit {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Battle.UI;
    using global::Unit.Data;
    using Map.Battle;
    using UnityEngine;

    public enum AnimationType {
        Attack, Damage, Death, Dodge
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

        private Animator _animator;
        private Unit _unit;

        private readonly Dictionary<string, int> _signalCounters = new();

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
        }

        public void Init(GridPosition gridPosition) {
            this._unit.Move(gridPosition);
            this.transform.position = this.worldRender.GridToWorld(gridPosition);
        }

        public Unit GetUnit() => this._unit;

        public IEnumerator MoveOnPath(IReadOnlyList<GridPosition> path) {
            foreach (GridPosition pos in path) {
                this._unit.Move(pos);
                Vector3 target = this.worldRender.GridToWorld(pos);
                yield return this.StartCoroutine(this.MoveRoutine(target));
            }
        }

        private IEnumerator MoveRoutine(Vector3 target) {
            const float speed = 4f;
            float time = 0f;
            Vector3 start = this.transform.position;
            float distance = Vector3.Distance(start, target);
            float duration = distance / speed;
            this._animator.SetBool(_isMoving, true);
            while (time < duration) {
                time += Time.deltaTime;
                this.transform.position = Vector3.Lerp(start, target, time / duration);
                yield return null;
            }

            this.transform.position = target;
            this._animator.SetBool(_isMoving, false);
        }

        public void Signal(string signalId) => this._signalCounters[signalId] = this._signalCounters.GetValueOrDefault(signalId, 0) + 1;

        private IEnumerator WaitForSignal(string signalId, int version) {
            yield return new WaitUntil(() => this._signalCounters.GetValueOrDefault(signalId, 0) > version);
        }

        private int GetSignalVersion(string signalId) => this._signalCounters.GetValueOrDefault(signalId, 0);

        public IEnumerator PlayBasicAttack() {
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
            this.combatTextUI.Init(attackResult.GetDamage().ToString(), attackResult.IsCritical() ? CombatTextType.Crit : CombatTextType.Hit);
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

    }
}
