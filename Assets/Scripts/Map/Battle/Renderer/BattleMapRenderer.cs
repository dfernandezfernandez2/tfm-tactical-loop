namespace Map.Battle.Renderer;

using System.Collections.Generic;
using Data;
using UnityEngine;

public class BattleMapRenderer : IMapRenderer {

    private readonly Dictionary<TileType, TileRenderElement> _tileRenderElements;
    private readonly float _size;

    public BattleMapRenderer(TileRenderSet tileRenderSet, float size) {
        this._tileRenderElements = tileRenderSet.ToDict();
        this._size = size;
    }

    public void Render(BattleMapData data) => data.ForEach(this.RenderTile);

    private void RenderTile(int x, int y, TileData tileData) {
        if (!this._tileRenderElements.TryGetValue(tileData.Type, out TileRenderElement tileRenderElement)) {
            Debug.LogWarning($"{tileData.Type} defined in position {x}, {y} is missing on render elements, will be skipped");
            return;
        }

        Vector3 tilePosition = new(tileData.Position.x * this._size, 0f, tileData.Position.y * this._size);
        Object.Instantiate(tileRenderElement.Prefab, tilePosition, Quaternion.identity, tileRenderElement.Parent);
    }
}
