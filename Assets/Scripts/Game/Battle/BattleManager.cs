namespace Game.Battle {
    using System;
    using System.Collections.Generic;
    using Data;
    using Map;
    using Map.Data;
    using Passive;
    using Placement;
    using Run.Data;
    using Run.Map;
    using Unit;
    using UnityEngine;

    [RequireComponent(typeof(TurnManager))]
    [RequireComponent(typeof(BattleMapFactory))]
    public class BattleManager : MonoBehaviour {
        [SerializeField] private UnitPlacementController unitPlacementController;
        [SerializeField] private BattleMapLoader battleMapLoader;
        [SerializeField] private BattleMapManager battleMapManager;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject battlePanel;
        private BattleMapFactory _battleMapFactory;

        private Team _enemyTeam;
        private Team _playerTeam;

        private TurnManager _turnManager;


        public void Awake() {
            this._turnManager = this.GetComponent<TurnManager>();
            this._battleMapFactory = this.GetComponent<BattleMapFactory>();
            this._turnManager.OnBattleEnd += battleResult => {
                this.battleMapLoader.DestroyCurrentMap();
                this.battleMapManager.End();
                this.gamePanel.SetActive(false);
                this.battlePanel.SetActive(false);
                this.OnBattleEnd?.Invoke(battleResult);
            };
        }

        public event Action<BattleResult> OnBattleEnd;

        public void StartMap(RunNode node) {
            this.gamePanel.SetActive(true);
            BattleMapSetupData battleMapSetupData = this._battleMapFactory.CreateMapFromNode(node);
            this.StartMap(RunData.GetInstance().Team, battleMapSetupData.EnemyTeam, battleMapSetupData.MapTextContent);
        }

        private void StartMap(Team playerTeam, Team enemyTeam, string map) {
            foreach (IPassive passive in RunData.GetInstance().Passives) {
                passive.OnMapStart();
            }

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
            this.battlePanel.SetActive(true);
            this._turnManager.StartMap(this._playerTeam, this._enemyTeam);
        }

        private void InitMap(string map) {
            BattleMapData battleMapData = this.battleMapLoader.Load(map);
            this.battleMapManager.Initialize(battleMapData);
            this.battleMapManager.CenterCameraOnMap();
        }

        private void SpawnPlayerUnit(TeamUnit teamUnit, GridPosition position) =>
            this.SpawnUnit(teamUnit, position, this._playerTeam);

        private void SpawnEnemies() {
            IReadOnlyList<TileData> enemyAvailableSpawnsPositions =
                this.battleMapManager.GetTeamTileSpawns(this._enemyTeam.GetBattleTeam());
            for (int i = 0;
                 i < this._enemyTeam.GetTeamUnits().Count && i < enemyAvailableSpawnsPositions.Count;
                 i++) {
                this.SpawnUnit(
                    this._enemyTeam.GetTeamUnits()[i],
                    enemyAvailableSpawnsPositions[i].TileGridPosition,
                    this._enemyTeam
                );
            }
        }

        private void SpawnUnit(TeamUnit teamUnit, GridPosition position, Team team) {
            UnitObject unit = Instantiate(teamUnit.Prefab);
            team.AddUnit(unit, teamUnit);
            unit.InitPosition(position, Vector2Int.down);
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
