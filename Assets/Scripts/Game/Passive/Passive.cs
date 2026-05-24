namespace Game.Passive {
    using System.Collections;
    using Battle.Unit;
    using Translation;
    using UnityEngine;

    public abstract class Passive : ScriptableObject, IPassive {
        [SerializeField] private string id;
        [SerializeField] private Sprite icon;
        [SerializeField] private GameObject uiObject;

        public virtual IEnumerator OnDeadUnit(UnitObject targetUnit) {
            yield return null;
        }

        public virtual IEnumerator OnDamage(UnitObject userUnit, UnitObject targetUnit, int damage) {
            yield return null;
        }

        public virtual void OnMapStart() {
        }

        public string GetName() => TranslatorManager.Get($"passive.{this.id}.name");

        public string GetDescription() => TranslatorManager.Get($"passive.{this.id}.description");

        public Sprite GetIcon() => this.icon;

        public GameObject GetUIObject() => this.uiObject;
    }
}
