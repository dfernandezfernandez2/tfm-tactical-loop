namespace Game.Map.Battle.Renderer {
    using System.Collections.Generic;
    using Data;
    using UnityEngine;

    public class BattleMapRenderer : IMapRenderer {
        private readonly GameObject _parentGameObject;
        private readonly Dictionary<Tile, TileRenderElement> _tileRenderElements;
        private readonly WorldRender _worldRender;

        public BattleMapRenderer(TileRenderSet tileRenderSet, WorldRender worldRender, GameObject parentGameObject) {
            this._tileRenderElements = tileRenderSet.ToDict();
            this._worldRender = worldRender;
            this._parentGameObject = parentGameObject;
        }

        public void Render(BattleMapData data) => data.ForEach(this.RenderTile);

        private void RenderTile(TileData tileData) {
            if (!this._tileRenderElements.TryGetValue(tileData.Tile, out TileRenderElement tileRenderElement)) {
                Debug.LogWarning(
                    $"{tileData.Tile} defined in position {tileData.TileGridPosition.Position.x}, {tileData.TileGridPosition.Position.y} is missing on render elements, will be skipped");
                return;
            }

            GameObject parentGameObject =
                new($"tile_{tileData.TileGridPosition.Position.x}_{tileData.TileGridPosition.Position.y}");
            parentGameObject.transform.SetParent(this._parentGameObject.transform);


            if (tileData.Tile.IsRenderBellow()) {
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
