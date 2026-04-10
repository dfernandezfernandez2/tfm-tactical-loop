namespace Game.Unit {
    using System;
    using global::Unit.Data;
    using Map.Battle;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class Unit {
        private readonly Stats _stats;
        private GridPosition _gridPosition;

        public Unit(Stats stats) => this._stats = stats;

        public void Move(GridPosition gridPosition) {
            this._gridPosition = gridPosition;
            this.OnMove?.Invoke(gridPosition);
        }

        private void TakeDamage(int amount, bool isCritical) {
            this._stats[StatType.Hp].Reduce(amount);
            this.OnDamage?.Invoke(amount, isCritical);
        }

        public void Heal(int amount) {
            this._stats[StatType.Hp].Add(amount);
            this.OnHeal?.Invoke(amount);
        }

        public void DoBasicAttack(Unit objective) {
            if (objective == null || !this.RollHit(objective)) {
                this.OnMiss?.Invoke();
                return;
            }

            bool isCrit = this.RollCrit();
            int damage = this.CalculateDamage(objective, isCrit);

            objective.TakeDamage(damage, isCrit);
            this.OnHit?.Invoke();
        }

        private bool RollHit(Unit target) {
            float accuracy = this._stats[StatType.Accuracy].Current;
            float evasion = target._stats[StatType.Evasion].Current;

            float hitChance = accuracy / (accuracy + evasion);
            hitChance = Mathf.Clamp(hitChance, 0.1f, 0.95f);

            return Random.value <= hitChance;
        }

        private bool RollCrit() {
            float critChance = this._stats[StatType.CritChance].Current;
            return Random.value <= critChance;
        }

        private int CalculateDamage(Unit target, bool isCrit) {
            float atk = this._stats[StatType.Atk].Current;
            float def = target._stats[StatType.Def].Current;

            float dmg = isCrit ? atk * 1.5f : atk;
            float final = Mathf.Max(0, dmg - def);

            return final <= 0 ? 1 : Mathf.RoundToInt(final);
        }

        public bool IsDead() => this._stats[StatType.Hp].IsEmpty();

        public event Action<int, bool> OnDamage;

        public event Action<int> OnHeal;

        public event Action OnMiss;

        public event Action OnHit;

        public event Action<GridPosition> OnMove;

        public void UseAp(int ap) => this._stats[StatType.AP].Reduce(ap);

        public void RecoverAp(int ap) => this._stats[StatType.AP].Add(ap);

        public void RestoreAp() => this._stats[StatType.AP].Restore();

        public bool CanUseAp(int ap) => this._stats[StatType.AP].Current >= ap;

        public int GetCurrentMovement() => (int) this._stats[StatType.Movement].Current;

        public GridPosition GetGridPosition() => this._gridPosition;

        public int GetAttackRange() => (int) this._stats[StatType.Range].Current;
    }
}
