namespace Game.Battle.Map {
    using Data;
    using Parser;
    using Renderer;
    using UnityEngine;

    public class BattleMapLoader : MonoBehaviour {
        [SerializeField] private TileRenderSet tileRenderSet;
        [SerializeField] private WorldRender worldRender;
        [SerializeField] private GameObject parentGameObject;

        private IMapRenderer _mapRenderer;
        private GameObject _parentGameObject;


        public void Awake() {
            this._parentGameObject = this.parentGameObject != null ? this.parentGameObject : new GameObject("Map");
            this._mapRenderer = new BattleMapRenderer(this.tileRenderSet, this.worldRender, this._parentGameObject);
        }

        public BattleMapData Load(string mapTextContent) => this.Load(mapTextContent, Vector2Int.zero);

        public BattleMapData Load(string mapTextContent, Vector2Int offset) {
            TxtMapParser parser = new();
            BattleMapData data = parser.Parse(mapTextContent, offset);
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
