namespace Game.Battle.Unit.Data {
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Unit Data")]
    public class UnitData : ScriptableObject {
        public string unitName;
        public Sprite unitSprite;
        [Range(1, 50)] public int hp;
        [Range(0, 50)] public int mp;
        [Range(0, 5)] public int mpRegen;
        [Range(0, 10)] public int movement;
        [Range(1, 5)] public int ap;
        [Range(1, 10)] public int atk;
        [Range(1, 10)] public int defense;
        [Range(0, 5)] public int speed;
        [Range(1, 10)] public int range;
        [Range(0, 1)] public float accuracy;
        [Range(0, 1)] public float evasion;
        [Range(0, 1)] public float critChance;

        public Stats GetStats() =>
            new Stats.Builder()
                .With(StatType.Hp, this.hp)
                .With(StatType.Mp, this.mp)
                .With(StatType.MpRegen, this.mpRegen)
                .With(StatType.Movement, this.movement)
                .With(StatType.AP, this.ap)
                .With(StatType.Atk, this.atk)
                .With(StatType.Def, this.defense)
                .With(StatType.Accuracy, this.accuracy)
                .With(StatType.Evasion, this.evasion)
                .With(StatType.CritChance, this.critChance)
                .With(StatType.Range, this.range)
                .With(StatType.Speed, this.speed)
                .Build();

        public List<KeyValuePair<StatType, float>> GetStatsInfo() {
            List<KeyValuePair<StatType, float>> stats = new() {
                new KeyValuePair<StatType, float>(StatType.Hp, this.hp),
                new KeyValuePair<StatType, float>(StatType.Mp, this.mp),
                new KeyValuePair<StatType, float>(StatType.MpRegen, this.mpRegen),
                new KeyValuePair<StatType, float>(StatType.Movement, this.movement),
                new KeyValuePair<StatType, float>(StatType.AP, this.ap),
                new KeyValuePair<StatType, float>(StatType.Atk, this.atk),
                new KeyValuePair<StatType, float>(StatType.Def, this.defense),
                new KeyValuePair<StatType, float>(StatType.Accuracy, this.accuracy),
                new KeyValuePair<StatType, float>(StatType.Evasion, this.evasion),
                new KeyValuePair<StatType, float>(StatType.CritChance, this.critChance),
                new KeyValuePair<StatType, float>(StatType.Range, this.range),
                new KeyValuePair<StatType, float>(StatType.Speed, this.speed)
            };
            return stats;
        }
    }
}
