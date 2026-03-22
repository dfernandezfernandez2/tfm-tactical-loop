namespace Map.Battle.Renderer {
    using System.Collections.Generic;
    using Data;
    using UnityEngine;

    public class BattleMapRenderer : IMapRenderer {
        private readonly GameObject _parentGameObject;
        private readonly Dictionary<TileType, TileRenderElement> _tileRenderElements;
        private readonly WorldRender _worldRender;

        public BattleMapRenderer(TileRenderSet tileRenderSet, GridConfiguration gridConfiguration) {
            this._tileRenderElements = tileRenderSet.ToDict();
            this._worldRender = new WorldRender(gridConfiguration);
            this._parentGameObject = new GameObject("Map");
        }

        public void Render(BattleMapData data) => data.ForEach(this.RenderTile);

        private void RenderTile(TileData tileData) {
            if (!this._tileRenderElements.TryGetValue(tileData.Type, out TileRenderElement tileRenderElement)) {
                Debug.LogWarning(
                    $"{tileData.Type} defined in position {tileData.Position.x}, {tileData.Position.y} is missing on render elements, will be skipped");
                return;
            }

            GameObject parentGameObject = new($"tile_{tileData.Position.x}_{tileData.Position.y}");
            parentGameObject.transform.SetParent(this._parentGameObject.transform);


            if (tileData.Type.IsRenderBellow()) {
                for (int i = 0; i <= tileData.Height; i++) {
                    this.RenderTile(tileRenderElement.Prefab, parentGameObject.transform, tileData.Position.x,
                        tileData.Position.y, i);
                }
            }
            else {
                this.RenderTile(tileRenderElement.Prefab, parentGameObject.transform, tileData.Position.x,
                    tileData.Position.y, tileData.Height);
            }
        }

        private void RenderTile(GameObject gameObject, Transform parent, int x, int y, int height) {
            Vector3 tilePosition =
                this._worldRender.GridToWorld(x, y, height);
            GameObject createdObject = Object.Instantiate(gameObject, tilePosition, Quaternion.identity, parent);
            createdObject.name = $"tile_{height}";
        }
    }
}
