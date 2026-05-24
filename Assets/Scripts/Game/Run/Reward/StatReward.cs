namespace Game.Run.Reward {
    using Battle.Data;
    using Battle.Unit.Data;
    using Data;
    using Translation;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Stat")]
    public class StatReward : ScriptableObject, IReward {
        [SerializeField] private StatType stat;
        [SerializeField] private float amount;
        [SerializeField] private Sprite sprite;
        [SerializeField] private string id;

        public Sprite GetIcon() => this.sprite;
        public string GetName() => TranslatorManager.Get($"stat_reward.{this.id}.name");
        public string GetDescription() => TranslatorManager.Get($"stat_reward.{this.id}.description");

        public void ApplyReward(RunData runData) {
            foreach (TeamUnit teamUnit in runData.Team.GetTeamUnits()) {
                teamUnit.UnitData.AddMaxStat(this.stat, this.amount);
            }
        }
    }
}
