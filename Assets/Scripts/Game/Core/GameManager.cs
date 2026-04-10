namespace Game.Core {
    using System.Collections.Generic;
    using Battle;
    using Game.Core;
    using Map.Battle;
    using Map.Battle.Data;
    using Unit;
    using UnityEngine;

    public class GameManager : MonoBehaviour {
        [SerializeField] private BattleMapLoader battleMapLoader;
        [SerializeField] private WorldRender gridConverter;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private List<UnitSpawnConfiguration> unitsPrefab;
        [SerializeField] private List<UnitObject> gameUnits = new();
        private TurnManager _turnManager;

        public void Awake() {
            this._turnManager = this.GetComponent<TurnManager>();
            foreach (UnitSpawnConfiguration unitSpawnConfiguration in this.unitsPrefab) {
                this.SpawnUnit(unitSpawnConfiguration);
            }
        }

        public void Start() {
            TextAsset map = Resources.Load<TextAsset>("Map/Battle/map_plain");
            BattleMapData battleMapData = this.battleMapLoader.Load(map.text);
            this.battleMapManager.Initialize(battleMapData);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.P)) {
                this._turnManager.StartTurn(this.gameUnits[0]);
            }
        }

        private void SpawnUnit(UnitSpawnConfiguration unitSpawnConfiguration) {
            GridPosition gridPosition = new(unitSpawnConfiguration.GridPosition, unitSpawnConfiguration.Height);
            UnitObject unit = Instantiate(unitSpawnConfiguration.UnitPrefab);
            unit.Init(gridPosition, this.gridConverter);
            this.gameUnits.Add(unit);
        }
    }
}
