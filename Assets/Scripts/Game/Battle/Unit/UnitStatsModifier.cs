namespace Game.Battle.Unit {
    using System.Collections.Generic;
    using Data;

    public class UnitStatsModifier {
        private readonly Dictionary<StatType, float> _modifiers = new();

        public void AddModifier(StatType statType, float value) {
            if (this._modifiers.TryAdd(statType, value)) {
                return;
            }

            float modifier = this._modifiers[statType];
            modifier += value;
            this._modifiers[statType] = modifier;
        }

        public float GetModifier(StatType statType) => this._modifiers.GetValueOrDefault(statType, 0f);
    }
}
