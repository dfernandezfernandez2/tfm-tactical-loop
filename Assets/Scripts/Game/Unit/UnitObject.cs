namespace Game.Unit {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Battle.UI;
    using global::Unit.Data;
    using Map.Battle;
    using UnityEngine;

    public class UnitObject : MonoBehaviour {
        [SerializeField] private UnitData data;
        [SerializeField] private CombatTextUI combatTextUI;

        private Unit _unit;
        private WorldRender _worldRender;

        public void Awake() {
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

        public void Init(GridPosition gridPosition, WorldRender worldRender) {
            this._worldRender = worldRender;
            this._unit.Move(gridPosition);
        }

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
            Vector3 position = this._worldRender.GridToWorld(gridPosition);
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
            // animacion de recibir daño y por el final mostrar texto
            this.combatTextUI.Init(damage.ToString(), isCritical ? CombatTextType.Crit : CombatTextType.Hit);
            // falta calcular si está muerto tbn para al final tener la animacion de muerte
        }

        private void OnHeal(int heal) {
            // animacion de cura
        }

        private void OnMiss() {
            // animacion de golpe básico y al termina el texto o al estar terminando
            this.combatTextUI.Init(CombatTextType.Miss);
        }

        private void OnHit() {
            // animacion de golpe básico
        }

        public void UseAp(int ap) => this._unit.UseAp(ap);

        public void RecoverAp(int ap) => this._unit.RecoverAp(ap);

        public bool CanUseAp(int ap) => this._unit.CanUseAp(ap);

        public void RestoreAp() => this._unit.RestoreAp();

        public int GetCurrentMovement() => this._unit.GetCurrentMovement();

        public GridPosition GetGridPosition() => this._unit.GetGridPosition();

        public int GetAttackRange() => this._unit.GetAttackRange();
    }
}
