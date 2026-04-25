namespace Unit.Data {
    using System;

    public class Stat {
        private float _current;

        public Stat(float value) {
            this.Max = value;
            this._current = value;
        }

        public float Max { get; }

        public float Add(float amount) {
            float previous = this._current;
            this._current = Math.Min(this.Max, Math.Max(0, this._current + amount));
            return this._current - previous;
        }

        public void Restore() => this._current = this.Max;
        public float GetCurrentWithModifier(float amount) => Math.Min(this.Max, this._current + amount);
    }
}
