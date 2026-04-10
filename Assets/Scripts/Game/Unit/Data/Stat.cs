namespace Unit.Data {
    using System;

    public class Stat {
        private readonly float _max;

        public Stat(float value) {
            this._max = value;
            this.Current = value;
        }

        public float Current { get; private set; }

        public void Add(float amount) => this.Current = Math.Min(this._max, this.Current + amount);
        public void Reduce(float amount) => this.Current = Math.Max(0, this.Current - amount);
        public void Restore() => this.Current = this._max;
        public bool IsEmpty() => this.Current == 0;
        public bool IsFull() => this.Current >= this._max;
    }
}
