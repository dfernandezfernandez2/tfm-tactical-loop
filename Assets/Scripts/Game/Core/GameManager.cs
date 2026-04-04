namespace Game.Core {
    using System.Collections.Generic;
    using Game.Core;
    using Map;
    using Unit;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        [SerializeField] private WorldRender gridConverter;
        [SerializeField] private List<UnitSpawnConfiguration> unitsPrefab;

        public void Awake() {
            foreach (UnitSpawnConfiguration unitSpawnConfiguration in this.unitsPrefab) {
                this.SpawnUnit(unitSpawnConfiguration);
            }
        }

        private void SpawnUnit(UnitSpawnConfiguration unitSpawnConfiguration) {
            GridPosition gridPosition = new(unitSpawnConfiguration.GridPosition, unitSpawnConfiguration.Height);
            UnitObject unit = Instantiate(unitSpawnConfiguration.UnitPrefab);
            unit.Init(gridPosition, this.gridConverter);
        }
    }
}
