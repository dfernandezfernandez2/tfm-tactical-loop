namespace Game.Reward {
    using System;
    using Map.Run;
    using UnityEngine;

    [Serializable]
    public class EncounterRewardPoolEntry {
        [SerializeField] private ScriptableObject reward;
        [SerializeField] private EncounterType encounterType;
        [SerializeField] private int minLevel = 1;
        [SerializeField] private int maxLevel = 99;
        [SerializeField] private int weight = 1;

        public IReward Reward => this.reward as IReward;
        public EncounterType EncounterType => this.encounterType;
        public int MinLevel => this.minLevel;
        public int MaxLevel => this.maxLevel;
        public int Weight => this.weight;

        public bool IsValidFor(EncounterType type, int level) =>
            this.Reward != null &&
            this.encounterType == type &&
            level >= this.minLevel &&
            level <= this.maxLevel &&
            this.weight > 0;
    }
}
