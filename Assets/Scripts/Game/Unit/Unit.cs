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

        public void TakeDamage(int amount) {
            this._stats[StatType.Hp].Reduce(amount);
            if (this.IsDead()) {
                this.OnDeath?.Invoke();
            }
            else {
                this.OnDamage?.Invoke(amount);
            }
        }

        public void Heal(int amount) {
            this._stats[StatType.Hp].Add(amount);
            this.OnHeal?.Invoke(amount);
        }

        public void DoBasicAttack(Unit objective) {
            if (!this.RollHit(objective)) {
                this.OnMiss?.Invoke();
                return;
            }

            bool isCrit = this.RollCrit();
            int damage = this.CalculateDamage(objective, isCrit);

            objective.TakeDamage(damage);
            this.OnHit?.Invoke(damage);
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

        private bool IsDead() => this._stats[StatType.Hp].IsEmpty();

        public event Action OnDeath;

        public event Action<int> OnDamage;

        public event Action<int> OnHeal;

        public event Action OnMiss;

        public event Action<int> OnHit;

        public event Action<GridPosition> OnMove;

        public void UseAp(int ap) => this._stats[StatType.AP].Reduce(ap);

        public void RecoverAp(int ap) => this._stats[StatType.AP].Add(ap);

        public void RestoreAp() => this._stats[StatType.AP].Restore();

        public bool CanUseAp(int ap) => this._stats[StatType.AP].Current >= ap;

        public int GetCurrentMovement() => (int)this._stats[StatType.Movement].Current;

        public GridPosition GetGridPosition() => this._gridPosition;
    }
}
