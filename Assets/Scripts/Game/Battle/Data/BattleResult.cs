namespace Game.Battle.Data {
    public struct BattleResult {
        public BattleResult(BattleTeam winner) => this.Winner = winner;

        public BattleTeam Winner { get; }
    }
}
