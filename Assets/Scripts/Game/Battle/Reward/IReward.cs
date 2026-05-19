namespace Game.Battle.Reward {
    using Run.Data;
    using UnityEngine;

    public interface IReward {
        public Sprite GetIcon();
        public string GetName();
        public string GetDescription();
        public void ApplyReward(RunData runData);
    }
}
