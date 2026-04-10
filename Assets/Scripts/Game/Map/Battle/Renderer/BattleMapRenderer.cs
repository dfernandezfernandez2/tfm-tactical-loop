namespace Game.Map.Battle.Renderer {
    using System.Collections.Generic;
    using Data;
    using UnityEngine;

    public class BattleMapRenderer : IMapRenderer {
        private readonly GameObject _parentGameObject;
        private readonly Dictionary<TileType, TileRenderElement> _tileRenderElements;
        private readonly WorldRender _worldRender;

        public BattleMapRenderer(TileRenderSet tileRenderSet, WorldRender worldRender) {
            this._tileRenderElements = tileRenderSet.ToDict();
            this._worldRender = worldRender;
            this._parentGameObject = new GameObject("Map");
        }

        public void Render(BattleMapData data) => data.ForEach(this.RenderTile);

        private void RenderTile(TileData tileData) {
            if (!this._tileRenderElements.TryGetValue(tileData.Type, out TileRenderElement tileRenderElement)) {
                Debug.LogWarning(
                    $"{tileData.Type} defined in position {tileData.TileGridPosition.Position.x}, {tileData.TileGridPosition.Position.y} is missing on render elements, will be skipped");
                return;
            }

            GameObject parentGameObject =
                new($"tile_{tileData.TileGridPosition.Position.x}_{tileData.TileGridPosition.Position.y}");
            parentGameObject.transform.SetParent(this._parentGameObject.transform);


            if (tileData.Type.IsRenderBellow()) {
                for (int i = 0; i <= tileData.TileGridPosition.Height; i++) {
                    this.RenderTile(tileData, tileRenderElement.Prefab, parentGameObject.transform,
                        new GridPosition(tileData.TileGridPosition.Position, i));
                }
            }
            else {
                this.RenderTile(tileData, tileRenderElement.Prefab, parentGameObject.transform,
                    tileData.TileGridPosition);
            }
        }

        private void RenderTile(TileData tileData, GameObject gameObject, Transform parent, GridPosition gridPosition) {
            Vector3 tilePosition =
                this._worldRender.GridToWorldTiles(gridPosition);
            GameObject createdObject = Object.Instantiate(gameObject, tilePosition, Quaternion.identity, parent);
            TileView tileView = createdObject.GetComponent<TileView>();
            tileData.TileView = tileView;
            createdObject.name = $"tile_{gridPosition.Height}";
        }
    }
}
