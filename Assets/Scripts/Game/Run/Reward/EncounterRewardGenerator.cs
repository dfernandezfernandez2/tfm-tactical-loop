namespace Game.Run.Reward {
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Map;
    using Passive;
    using UnityEngine;

    public class EncounterRewardGenerator : MonoBehaviour {
        [SerializeField] private List<EncounterRewardPoolEntry> rewardPool = new();

        public List<IReward> GenerateRewards(RunNode completedNode) =>
            this.GenerateRewardsByType(completedNode.EncounterType, completedNode.Level, 3);

        private List<IReward> GenerateRewardsByType(EncounterType encounterType, int level, int amount) {
            RunData runData = RunData.GetInstance();
            List<EncounterRewardPoolEntry> candidates = this.rewardPool
                .Where(entry => entry.IsValidFor(encounterType, level))
                .Where(entry => !IsAlreadyUnlockedPassive(entry.Reward, runData))
                .ToList();
            List<IReward> result = new();
            while (result.Count < amount && candidates.Count > 0) {
                EncounterRewardPoolEntry selectedEntry = GetWeightedRandom(candidates);
                IReward reward = selectedEntry.Reward;
                result.Add(reward);
                candidates.RemoveAll(entry => entry.Reward == reward);
            }

            return result;
        }

        private static EncounterRewardPoolEntry GetWeightedRandom(List<EncounterRewardPoolEntry> entries) {
            int totalWeight = entries.Sum(entry => entry.Weight);
            int roll = Random.Range(0, totalWeight);
            int current = 0;
            foreach (EncounterRewardPoolEntry entry in entries) {
                current += entry.Weight;
                if (roll < current) {
                    return entry;
                }
            }

            return entries[^1];
        }

        private static bool IsAlreadyUnlockedPassive(IReward reward, RunData runData) {
            if (reward is not PassiveReward passiveReward) {
                return false;
            }

            Passive passive = passiveReward.GetPassive();
            return runData.HasPassive(passive);
        }
    }
}
