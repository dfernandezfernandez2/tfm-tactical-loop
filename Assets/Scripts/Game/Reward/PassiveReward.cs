namespace Game.Reward {
    using Core.Data;
    using Passive;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Passive")]
    public class PassiveReward : ScriptableObject, IReward {
        [SerializeField] private Passive passive;

        public Sprite GetIcon() => this.passive.GetIcon();

        public string GetName() => this.passive.GetName();
        public string GetDescription() => this.passive.GetDescription();

        public void ApplyReward(RunData runData) => runData.AddPassive(this.passive);

        public Passive GetPassive() => this.passive;
    }
}
