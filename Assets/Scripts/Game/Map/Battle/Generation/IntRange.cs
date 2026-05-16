namespace Game.Map.Battle.Generation {
    using System;

    public readonly struct IntRange {
        private readonly int _min;
        private readonly int _max;

        public IntRange(int min, int max) {
            this._min = min;
            this._max = max;
        }

        public int Pick(Random random) =>
            random.Next(this._min, this._max + 1);
    }
}
