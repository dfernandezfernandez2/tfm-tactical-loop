namespace Game.Unit {
    using System.Collections.Generic;
    using System.Linq;
    using global::Unit.Data;
    using Map.Battle;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class Unit {
        private readonly Stats _stats;
        private Vector2Int _direction;
        private GridPosition _gridPosition;

        public Unit(Stats stats) => this._stats = stats;

        public void Move(GridPosition gridPosition, Vector2Int direction) {
            this._gridPosition = gridPosition;
            this._direction = direction;
        }

        public AttackResult DoBasicAttack(Unit objective) {
            if (objective == null) {
                return AttackResult.Miss();
            }

            if (objective.IsDead()) {
                return AttackResult.Miss(true);
            }

            if (!this.RollHit(objective)) {
                return AttackResult.Miss();
            }

            bool isCrit = this.RollCrit();
            int damage = this.CalculateDamage(objective, isCrit);
            objective.GetStat(StatType.Hp).Reduce(damage);
            return AttackResult.Hit(damage, isCrit, objective.IsDead());
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

        public Stat GetStat(StatType statType) => this._stats[statType];

        public bool IsDead() => this._stats[StatType.Hp].IsEmpty();

        public int GetCurrentMovement() => (int)this._stats[StatType.Movement].Current;

        public GridPosition GetGridPosition() => this._gridPosition;

        public int GetAttackRange() => (int)this._stats[StatType.Range].Current;

        public bool IsStatFull(StatType statType) => this._stats[statType].IsFull();

        public List<KeyValuePair<StatType, float>> GetCurrentStats(params StatType[] filter) =>
            filter.Select(t => new KeyValuePair<StatType, float>(t, this._stats[t].Current)).ToList();

        public void UpdateDirection(Vector2Int direction) => this._direction = direction;

        public int GetCurrentAp() => (int)this._stats[StatType.AP].Current;

        public int GetCurrentHp() => (int)this._stats[StatType.Hp].Current;

        public Vector2Int GetDirection() => this._direction;
    }
}
