namespace Game.Core {
    namespace Game.Core {
        using System;
        using Unit;
        using UnityEngine;

        [Serializable]
        public class UnitSpawnConfiguration {
            [SerializeField] private UnitObject unitPrefab;
            [SerializeField] private Vector2Int gridPosition;
            [SerializeField] private int height;

            public UnitObject UnitPrefab => this.unitPrefab;
            public Vector2Int GridPosition => this.gridPosition;
            public int Height => this.height;
        }
    }
}
