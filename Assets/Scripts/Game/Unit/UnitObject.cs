namespace Game.Unit {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Battle.UI;
    using global::Unit.Data;
    using Map.Battle;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class UnitObject : MonoBehaviour {

        private static readonly int _damageAnimation = Animator.StringToHash("Damage");
        private static readonly int _deathAnimation = Animator.StringToHash("Death");
        private static readonly int _healAnimation = Animator.StringToHash("Heal");

        [SerializeField] private UnitData data;
        [SerializeField] private CombatTextUI combatTextUI;
        [SerializeField] private WorldRender worldRender;

        private Animator _animator;

        private Unit _unit;
        private readonly Queue<UnitDamage> _pendingDamages = new();
        private readonly Queue<int> _pendingHeals = new();

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
            this._unit.OnMove += this.OnMove;
            this._unit.OnDamage += this.OnDamage;
            this._unit.OnHeal += this.OnHeal;
            this._unit.OnMiss += this.OnMiss;
            this._unit.OnHit += this.OnHit;
        }

        public void Init(GridPosition gridPosition) => this._unit.Move(gridPosition);

        public IEnumerator MoveOnPath(IReadOnlyList<GridPosition> path) {
            foreach (GridPosition pos in path) {
                bool finished = false;

                this.OnMoveEnd += Handler;
                this._unit.Move(pos);

                yield return new WaitUntil(() => finished);

                this.OnMoveEnd -= Handler;
                yield return null;
                continue;

                void Handler() {
                    finished = true;
                }
            }
        }

        public IEnumerator DoBasicAttack(UnitObject target) {
            this._unit.DoBasicAttack(target?._unit);
            yield return null;
        }

        private void OnMove(GridPosition gridPosition) {
            Vector3 position = this.worldRender.GridToWorld(gridPosition);
            this.StartCoroutine(this.MoveRoutine(position));
        }

        private IEnumerator MoveRoutine(Vector3 target) {
            const float speed = 4f;
            float time = 0f;
            Vector3 start = this.transform.position;
            float distance = Vector3.Distance(start, target);
            float duration = distance / speed;
            while (time < duration) {
                time += Time.deltaTime;
                this.transform.position = Vector3.Lerp(start, target, time / duration);
                yield return null;
            }

            this.transform.position = target;
            this.OnMoveEnd?.Invoke();
        }

        private event Action OnMoveEnd;

        private void OnDamage(int damage, bool isCritical) {
            this._pendingDamages.Enqueue(new UnitDamage {
                Damage = damage,
                IsCritical = isCritical
            });
            this._animator.SetTrigger(_damageAnimation);
        }

        // this will be called from animation hit event
        public void OnDamageImpact() {
            UnitDamage damage = this._pendingDamages.Dequeue();
            this.combatTextUI.Init(damage.Damage.ToString(), damage.IsCritical ? CombatTextType.Crit : CombatTextType.Hit);
            if (this._unit.IsDead()) {
                this._animator.SetTrigger(_deathAnimation);
            }
        }

        private void OnHeal(int heal) {
            this._pendingHeals.Enqueue(heal);
            this._animator.SetTrigger(_healAnimation);
        }

        // this will be called from animation heal event
        public void OnHealApplied() {
            int heal = this._pendingHeals.Dequeue();
            this.combatTextUI.Init(heal.ToString(), CombatTextType.Heal);
        }

        private void OnMiss() {
            // animacion de golpe básico y al termina el texto o al estar terminando
            this.combatTextUI.Init(CombatTextType.Miss);
        }

        private void OnHit() {
            // animacion de golpe básico
        }

        public Unit GetUnit() => this._unit;
    }
}
