namespace Game.Unit {
    using System.Collections.Generic;
    using System.Linq;
    using global::Unit.Data;
    using Map.Battle.Data;
    using UnityEngine;

    public class Unit {
        private readonly Stats _stats;

        public Unit(Stats stats) {
            this._stats = stats;
            this.UnitStatsModifier = new UnitStatsModifier();
            this.UnitDamageResolver = new UnitDamageResolver(this.GetCurrentStat);
        }

        public UnitDamageResolver UnitDamageResolver { get; private set; }
        public UnitStatsModifier UnitStatsModifier { get; }
        public Vector2Int Direction { get; set; }
        public GridPosition GridPosition { get; private set; }

        public void Move(GridPosition gridPosition, Vector2Int direction) {
            this.GridPosition = gridPosition;
            this.Direction = direction;
        }

        public List<KeyValuePair<StatType, float>> GetCurrentStats(params StatType[] filter) =>
            filter.Select(t => new KeyValuePair<StatType, float>(t, this.GetCurrentIntStat(t))).ToList();

        public float GetCurrentStat(StatType statType) =>
            this._stats[statType].GetCurrentWithModifier(this.UnitStatsModifier.GetModifier(statType));

        public int GetCurrentIntStat(StatType statType) => (int)this._stats[statType]
            .GetCurrentWithModifier(this.UnitStatsModifier.GetModifier(statType));

        public bool IsStatFull(StatType statType) => this._stats[statType].Max <= this.GetCurrentStat(statType);

        public bool IsDead() => this.GetCurrentIntStat(StatType.Hp) <= 0;

        public float AddStat(StatType statType, float amount) => this._stats[statType].Add(amount);

        public void AddMaxStat(StatType statType, float amount) => this._stats[statType].AddMaxStat(amount);

        public void RestoreStat(StatType statType) => this._stats[statType].Restore();

        public float GetMaxStat(StatType statType) => this._stats[statType].Max;
    }
}
