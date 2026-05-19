namespace Game.Battle.Map.Parser {
    using Data;

    public interface IMapParser {
        public BattleMapData Parse(string mapTextContent);
    }
}
