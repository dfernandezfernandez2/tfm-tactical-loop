namespace Game.Battle.Data {
    public class BattleMapSetupData {
        public readonly Team EnemyTeam;
        public readonly string MapTextContent;

        public BattleMapSetupData(string mapTextContent, Team enemyTeam) {
            this.MapTextContent = mapTextContent;
            this.EnemyTeam = enemyTeam;
        }
    }
}
