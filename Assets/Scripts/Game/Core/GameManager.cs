namespace Game.Core {
    using System;
    using System.Collections.Generic;
    using Battle;
    using Battle.Data;
    using Data;
    using Map.Battle;
    using Map.Battle.Data;
    using Map.Run;
    using Unit;
    using UnityEngine;

    [RequireComponent(typeof(TurnManager))]
    [RequireComponent(typeof(BattleMapFactory))]
    public class GameManager : MonoBehaviour {
        [SerializeField] private BattleMapLoader battleMapLoader;
        [SerializeField] private WorldRender gridConverter;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private UnitPlacementController unitPlacementController;
        [SerializeField] private Camera mainCamera;
        private BattleMapFactory _battleMapFactory;

        private Team _enemyTeam;
        private Team _playerTeam;

        private TurnManager _turnManager;


        public void Awake() {
            this._turnManager = this.GetComponent<TurnManager>();
            this._battleMapFactory = this.GetComponent<BattleMapFactory>();
            this._turnManager.OnBattleEnd += battleResult => this.OnBattleEnd?.Invoke(battleResult);
        }

        public event Action<BattleResult> OnBattleEnd;

        public void StartMap(RunNode node) {
            BattleMapSetupData battleMapSetupData = this._battleMapFactory.CreateMapFromNode(node);
            this.StartMap(RunData.GetInstance().Team, battleMapSetupData.EnemyTeam, battleMapSetupData.MapTextContent);
        }

        private void StartMap(Team playerTeam, Team enemyTeam, string map) {
            this._playerTeam = playerTeam;
            this._enemyTeam = enemyTeam;

            this.InitMap(map);
            this.SpawnEnemies();
            this.unitPlacementController.Init(playerTeam, this.OnPlacementFinished, this.SpawnPlayerUnit,
                this.DespawnUnit, this.battleMapManager.Highlight, this.battleMapManager.UnHighlight);
        }

        private void OnPlacementFinished() {
            this.battleMapManager.UnHighlightUnits();
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
            unit.Init(position, Vector2Int.down);
            team.AddUnit(unit);
            this.battleMapManager.InitUnit(unit);
        }

        private void DespawnUnit(GridPosition gridPosition) {
            UnitObject unitObject = this.battleMapManager.GetUnit(gridPosition);
            this._playerTeam.RemoveUnit(unitObject);
            this.battleMapManager.DespawnUnit(unitObject);
            Destroy(unitObject.gameObject);
        }
    }
}
