namespace Game.Core {
    using System.Collections.Generic;
    using Battle;
    using Data;
    using Map.Battle;
    using Map.Battle.Data;
    using Unit;
    using UnityEngine;

    [RequireComponent(typeof(TurnManager))]
    public class GameManager : MonoBehaviour {
        [SerializeField] private BattleMapLoader battleMapLoader;
        [SerializeField] private WorldRender gridConverter;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private UnitPlacementController unitPlacementController;
        [SerializeField] private Camera mainCamera;
        private Team _enemyTeam;
        private Team _playerTeam;

        private TurnManager _turnManager;


        public void Awake() => this._turnManager = this.GetComponent<TurnManager>();

        public void Start() {
            TextAsset map = Resources.Load<TextAsset>("Map/Battle/map_plain");
            UnitObject unitObject = Resources.Load<UnitObject>("Knight");
            Team playerTeam = RunData.GetInstance().Team;
            Team enemyTeam = new(new List<UnitObject> { unitObject }, BattleTeam.Enemy);
            this.StartMap(playerTeam, enemyTeam, map.text);
        }

        public void StartMap(Team playerTeam, Team enemyTeam, string map) {
            this._playerTeam = playerTeam;
            this._enemyTeam = enemyTeam;

            this.InitMap(map);
            this.SpawnEnemies();
            this.unitPlacementController.Init(playerTeam, this.OnPlacementFinished, this.SpawnPlayerUnit,
                this.DespawnUnit);
        }

        private void OnPlacementFinished() {
            this.battleMapManager.HighlightUnits();
            this._turnManager.StartMap(this._playerTeam, this._enemyTeam);
        }

        private void InitMap(string map) {
            BattleMapData battleMapData = this.battleMapLoader.Load(map);
            this.battleMapManager.Initialize(battleMapData);
            this.CenterCameraOnMap();
        }

        private void CenterCameraOnMap() {
            GridPosition centerMapPosition = this.battleMapManager.GetMapCenterPosition();
            Vector3 centerMap = this.gridConverter.GridToWorld(centerMapPosition);
            this.mainCamera.transform.position = new Vector3(
                centerMap.x,
                centerMap.y,
                this.mainCamera.transform.position.z
            );
        }

        private void SpawnPlayerUnit(UnitObject unitPrefab, GridPosition position) =>
            this.SpawnUnit(unitPrefab, position, this._playerTeam);

        private void SpawnEnemies() {
            IReadOnlyList<TileData> enemyAvailableSpawnsPositions =
                this.battleMapManager.GetTeamTileSpawns(this._enemyTeam.GetBattleTeam());
            for (int i = 0;
                 i < this._enemyTeam.GetUnitObjectsPrefabs().Count && i < enemyAvailableSpawnsPositions.Count;
                 i++) {
                this.SpawnUnit(
                    this._enemyTeam.GetUnitObjectsPrefabs()[i],
                    enemyAvailableSpawnsPositions[i].TileGridPosition,
                    this._enemyTeam
                );
            }
        }

        private void SpawnUnit(UnitObject unitPrefab, GridPosition position, Team team) {
            UnitObject unit = Instantiate(unitPrefab);
            unit.Init(position);
            team.AddUnit(unit);
            this.battleMapManager.InitUnit(unit);
        }

        private void DespawnUnit(GridPosition gridPosition) {
            UnitObject unitObject = this.battleMapManager.GetUnit(gridPosition);
            this.battleMapManager.DespawnUnit(unitObject);
            Destroy(unitObject.gameObject);
        }
    }
}
