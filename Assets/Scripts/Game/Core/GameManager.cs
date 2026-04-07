namespace Game.Core {
    using System.Collections.Generic;
    using Battle;
    using Game.Core;
    using Map;
    using Unit;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        [SerializeField] private WorldRender gridConverter;
        [SerializeField] private List<UnitSpawnConfiguration> unitsPrefab;
        [SerializeField] private List<UnitObject> gameUnits = new();
        private TurnManager _turnManager;

        public void Awake() {
            this._turnManager = this.GetComponent<TurnManager>();
            foreach (UnitSpawnConfiguration unitSpawnConfiguration in this.unitsPrefab) {
                this.SpawnUnit(unitSpawnConfiguration);
            }
        }

        private void SpawnUnit(UnitSpawnConfiguration unitSpawnConfiguration) {
            GridPosition gridPosition = new(unitSpawnConfiguration.GridPosition, unitSpawnConfiguration.Height);
            UnitObject unit = Instantiate(unitSpawnConfiguration.UnitPrefab);
            unit.Init(gridPosition, this.gridConverter);
            this.gameUnits.Add(unit);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                this._turnManager.StartTurn(this.gameUnits[0]);
            }

        }
    }
}
