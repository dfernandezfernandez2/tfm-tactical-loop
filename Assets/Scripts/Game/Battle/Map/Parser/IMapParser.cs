namespace Game.Battle.Map.Parser {
    using Data;
    using UnityEngine;

    public interface IMapParser {
        public BattleMapData Parse(string mapTextContent);
        public BattleMapData Parse(string mapTextContent, Vector2Int offset);
    }
}
