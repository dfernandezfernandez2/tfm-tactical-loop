namespace Game.Passive {
    using System.Collections;
    using Battle.Unit;

    public interface IPassive {
        public IEnumerator OnDeadUnit(UnitObject targetUnit);
        public IEnumerator OnDamage(UnitObject userUnit, UnitObject targetUnit, int damage);
        public void OnMapStart();
    }
}
