namespace Unit.Data {
    using System;
    using System.Collections.Generic;

    public class Stats {
        private readonly Dictionary<StatType, Stat> _stats;

        private Stats(Dictionary<StatType, Stat> stats) => this._stats = stats;

        public Stat this[StatType type] => this._stats[type];

        public IReadOnlyDictionary<StatType, Stat> All => this._stats;

        public class Builder {
            private readonly Dictionary<StatType, float> _values;

            public Builder() {
                this._values = new Dictionary<StatType, float>();
                foreach (StatType type in Enum.GetValues(typeof(StatType))) {
                    this._values[type] = type.DefaultValue();
                }
            }

            public Builder With(StatType type, float value) {
                this._values[type] = value;
                return this;
            }

            public Stats Build() {
                Dictionary<StatType, Stat> stats = new();

                foreach ((StatType type, float value) in this._values) {
                    stats[type] = new Stat(value);
                }

                return new Stats(stats);
            }
        }
    }
}
