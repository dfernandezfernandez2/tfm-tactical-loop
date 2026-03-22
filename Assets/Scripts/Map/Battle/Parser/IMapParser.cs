namespace Map.Battle.Parser {
    using Data;

    public interface IMapParser {
        public BattleMapData Parse(string mapTextContent);
    }
}
