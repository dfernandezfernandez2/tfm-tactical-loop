namespace Game.Passive {
    using System.Collections;
    using Battle.Unit;
    using UnityEngine;

    public abstract class Passive : ScriptableObject, IPassive {
        [SerializeField] private string passiveName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        public virtual IEnumerator OnDeadUnit(UnitObject targetUnit) {
            yield return null;
        }

        public virtual IEnumerator OnDamage(UnitObject userUnit, UnitObject targetUnit, int damage) {
            yield return null;
        }

        public virtual void OnMapStart() {
        }

        public string GetName() => this.passiveName;

        public string GetDescription() => this.description;

        public Sprite GetIcon() => this.icon;
    }
}
