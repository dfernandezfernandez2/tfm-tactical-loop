namespace Game.Unit {
    using System.Collections.Generic;
    using System.Linq;
    using Effect;
    using global::Unit.Data;
    using Map.Battle;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class Unit {
        private readonly Stats _stats;
        private readonly UnitEffectController _unitEffectController;

        private Vector2Int _direction;
        private GridPosition _gridPosition;

        public Unit(Stats stats) {
            this._stats = stats;
            this._unitEffectController = new UnitEffectController();
        }

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
            objective.AddStat(StatType.Hp, -damage);
            return AttackResult.Hit(damage, isCrit, objective.IsDead());
        }

        private bool RollHit(Unit target) {
            float accuracy = this.GetCurrentStat(StatType.Accuracy);
            float evasion = target.GetCurrentStat(StatType.Evasion);

            float hitChance = accuracy / (accuracy + evasion);
            hitChance = Mathf.Clamp(hitChance, 0.1f, 0.95f);

            return Random.value <= hitChance;
        }

        private bool RollCrit() {
            float critChance = this.GetCurrentStat(StatType.CritChance);
            return Random.value <= critChance;
        }

        private int CalculateDamage(Unit target, bool isCrit) {
            float atk = this.GetCurrentStat(StatType.Atk);
            float def = target.GetCurrentStat(StatType.Def);

            float dmg = isCrit ? atk * 1.5f : atk;
            float final = Mathf.Max(0, dmg - def);

            return final <= 0 ? 1 : Mathf.RoundToInt(final);
        }

        public GridPosition GetGridPosition() => this._gridPosition;

        public void UpdateDirection(Vector2Int direction) => this._direction = direction;

        public Vector2Int GetDirection() => this._direction;

        public List<KeyValuePair<StatType, float>> GetCurrentStats(params StatType[] filter) =>
            filter.Select(t => new KeyValuePair<StatType, float>(t, this.GetCurrentIntStat(t))).ToList();

        public float GetCurrentStat(StatType statType) =>
            this._stats[statType].GetCurrentWithModifier(this._unitEffectController.GetModifier(statType));

        public int GetCurrentIntStat(StatType statType) => (int)this._stats[statType]
            .GetCurrentWithModifier(this._unitEffectController.GetModifier(statType));

        public bool IsStatFull(StatType statType) => this._stats[statType].Max < this.GetCurrentStat(statType);

        public bool IsDead() => this.GetCurrentIntStat(StatType.Hp) <= 0;

        public float AddStat(StatType statType, float amount) => this._stats[statType].Add(amount);

        public void RestoreStat(StatType statType) => this._stats[statType].Restore();

        public float GetMaxStat(StatType statType) => this._stats[statType].Max;

        public void ApplyEffect(BattleEffect effect) => this._unitEffectController.ApplyEffect(effect);
    }
}
