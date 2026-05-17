namespace Game.Map.Battle.Generation.Data {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public readonly struct WeightedOption<T> {
        public WeightedOption(T value, int weight) {
            this.Value = value;
            this.Weight = weight;
        }

        public T Value { get; }
        public int Weight { get; }
    }

    public readonly struct ListWeightedOption<T> {
        private readonly IReadOnlyList<WeightedOption<T>> _options;

        public ListWeightedOption(params WeightedOption<T>[] options) => this._options = options;

        public T Pick(Random random) {
            int total = this._options.Sum(option => option.Weight);
            int roll = random.Next(total);
            int current = 0;
            foreach (WeightedOption<T> option in this._options) {
                current += option.Weight;
                if (roll < current) {
                    return option.Value;
                }
            }

            return this._options[^1].Value;
        }
    }
}
