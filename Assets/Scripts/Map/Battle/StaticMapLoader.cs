namespace Map.Battle {
    using Data;
    using Parser;
    using Renderer;
    using UnityEngine;

    public class StaticMapLoader : MonoBehaviour, IMapLoader {
        [SerializeField] private TextAsset mapFile;
        [SerializeField] private TileRenderSet tileRenderSet;
        [SerializeField] private GridConfiguration gridConfiguration;
        private IMapParser _mapParser;

        private IMapRenderer _mapRenderer;

        public void Awake() {
            this._mapRenderer = new BattleMapRenderer(this.tileRenderSet, this.gridConfiguration);
            this._mapParser = new TxtMapParser();
        }

        public void Start() => this.Load();

        public void Load() {
            BattleMapData data = this._mapParser.Parse(this.mapFile.text);
            this._mapRenderer.Render(data);
        }
    }
}
