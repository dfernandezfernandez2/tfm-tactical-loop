namespace Game.Battle.Map.Generation.Enemies.Data {
    using System;
    using Unit;
    using UnityEngine;

    [Serializable]
    public class BattleEnemyOption {
        [SerializeField] private UnitObject enemyPrefab;
        [SerializeField] private int weight = 100;
        [SerializeField] private int guaranteedCount;
        [SerializeField] private int maxAppearances;

        public UnitObject EnemyPrefab => this.enemyPrefab;
        public int Weight => this.weight;
        public int GuaranteedCount => this.guaranteedCount;
        public int MaxAppearances => this.maxAppearances;
    }
}
