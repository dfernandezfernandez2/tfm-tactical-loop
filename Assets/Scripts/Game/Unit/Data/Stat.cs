namespace Unit.Data {
    using System;

    public class Stat {
        public Stat(float value) {
            this.Max = value;
            this.Current = value;
        }

        public float Current { get; private set; }
        public float Max { get; }

        public float Add(float amount) {
            this.Current = Math.Min(this.Max, this.Current + amount);
            return this.Max - amount;
        }

        public void Reduce(float amount) => this.Current = Math.Max(0, this.Current - amount);
        public void Restore() => this.Current = this.Max;
        public bool IsEmpty() => this.Current == 0;
        public bool IsFull() => this.Current >= this.Max;
    }
}
