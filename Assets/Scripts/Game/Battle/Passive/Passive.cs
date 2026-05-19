namespace Game.Battle.Passive {
    using UnityEngine;

    public abstract class Passive : ScriptableObject, IPassive {
        [SerializeField] private string passiveName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;

        public string GetName() => this.passiveName;

        public string GetDescription() => this.description;

        public Sprite GetIcon() => this.icon;
    }
}
