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
        private GameObject _parentGameObject;

        public void Awake() {
            this._parentGameObject = new GameObject("Map");
            this._mapRenderer = new BattleMapRenderer(this.tileRenderSet, this.worldRender, this._parentGameObject);
            this._mapParser = new TxtMapParser();
        }

        public BattleMapData Load(string mapTextContent) {
            BattleMapData data = this._mapParser.Parse(mapTextContent);
            this._mapRenderer.Render(data);
            return data;
        }

        public void DestroyCurrentMap() {
            foreach (Transform child in this._parentGameObject.transform) {
                Destroy(child.gameObject);
            }
        }
    }
}
