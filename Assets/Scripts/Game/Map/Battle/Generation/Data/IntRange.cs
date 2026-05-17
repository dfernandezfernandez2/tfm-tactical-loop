namespace Game.Map.Battle.Generation.Data {
    using System;

    public readonly struct IntRange {
        public IntRange(int min, int max) {
            this.Min = min;
            this.Max = max;
        }

        public int Min { get; }
        public int Max { get; }

        public int Pick(Random random) =>
            random.Next(this.Min, this.Max + 1);
    }
}
