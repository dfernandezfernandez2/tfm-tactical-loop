namespace Game.Reward {
    using Core;
    using Core.Data;
    using global::Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Stat")]
    public class StatReward : ScriptableObject, IReward {
        [SerializeField] private StatType stat;
        [SerializeField] private float amount;
        [SerializeField] private Sprite sprite;
        [SerializeField] private string statRewardName;
        [SerializeField] private string description;

        public Sprite GetIcon() => this.sprite;
        public string GetName() => this.statRewardName;
        public string GetDescription() => this.description;

        public void ApplyReward(RunData runData) {
            foreach (TeamUnit teamUnit in runData.Team.GetTeamUnits()) {
                teamUnit.UnitData.AddMaxStat(this.stat, this.amount);
            }
        }
    }
}
