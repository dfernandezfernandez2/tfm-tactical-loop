namespace Game.Map.Battle {
    using Data;
    using Parser;
    using Renderer;
    using UnityEngine;

    public class BattleMapLoader : MonoBehaviour {
        [SerializeField] private TileRenderSet tileRenderSet;
        [SerializeField] private WorldRender worldRender;

        private IMapParser _mapParser;
        private IMapRenderer _mapRenderer;

        public void Awake() {
            this._mapRenderer = new BattleMapRenderer(this.tileRenderSet, this.worldRender);
            this._mapParser = new TxtMapParser();
        }

        public BattleMapData Load(string mapTextContent) {
            BattleMapData data = this._mapParser.Parse(mapTextContent);
            this._mapRenderer.Render(data);
            return data;
        }
    }
}
