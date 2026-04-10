namespace Game.Unit {
    public class AttackResult {
        private readonly int _damage;

        private readonly bool _hit;
        private readonly bool _isCritical;
        private readonly bool _isTargetDead;

        private AttackResult() : this(false) {}

        private AttackResult(bool isTargetDead) {
            this._hit = false;
            this._damage = 0;
            this._isCritical = false;
            this._isTargetDead = isTargetDead;
        }

        private AttackResult(int damage, bool isCritical, bool isTargetDead) {
            this._hit = true;
            this._damage = damage;
            this._isCritical = isCritical;
            this._isTargetDead = isTargetDead;
        }

        public static AttackResult Miss() => new();

        public static AttackResult Miss(bool isTargetDead) => new(isTargetDead);

        public static AttackResult Hit(int damage, bool isCritical, bool isTargetDead) =>
            new(damage, isCritical, isTargetDead);

        public bool GetHit() => this._hit;
        public int GetDamage() => this._damage;
        public bool IsCritical() => this._isCritical;
        public bool IsTargetDead() => this._isTargetDead;

        public override string ToString() => $"{this._damage} damage, {this._isCritical} critical, {this._isTargetDead}, is hit {this._hit}";
    }
}
