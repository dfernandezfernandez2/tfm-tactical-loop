namespace Map.Battle.Renderer {
    using System.Collections.Generic;
    using Data;
    using UnityEngine;

    public class BattleMapRenderer : IMapRenderer {
        private readonly Dictionary<TileType, TileRenderElement> _tileRenderElements;
        private readonly WorldRender _worldRender;

        public BattleMapRenderer(TileRenderSet tileRenderSet, GridConfiguration gridConfiguration) {
            this._tileRenderElements = tileRenderSet.ToDict();
            this._worldRender = new WorldRender(gridConfiguration);
        }

        public void Render(BattleMapData data) => data.ForEach(this.RenderTile);

        private void RenderTile(TileData tileData) {
            if (!this._tileRenderElements.TryGetValue(tileData.Type, out TileRenderElement tileRenderElement)) {
                Debug.LogWarning(
                    $"{tileData.Type} defined in position {tileData.Position.x}, {tileData.Position.y} is missing on render elements, will be skipped");
                return;
            }

            Vector3 tilePosition =
                this._worldRender.GridToWorld(tileData.Position.x, tileData.Position.y, tileData.Height);
            Object.Instantiate(tileRenderElement.Prefab, tilePosition, Quaternion.identity, tileRenderElement.Parent);
        }
    }
}
