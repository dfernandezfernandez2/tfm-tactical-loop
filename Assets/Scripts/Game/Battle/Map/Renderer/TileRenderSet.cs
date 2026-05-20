namespace Game.Battle.Map.Renderer {
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Map/Tiles Prefab")]
    public class TileRenderSet : ScriptableObject {
        [SerializeField] private List<TileRenderElement> entries = new();

        public Dictionary<Tile, TileRenderElement> ToDict() {
            Dictionary<Tile, TileRenderElement> dict = new();
            foreach (TileRenderElement entry in this.entries.Where(entry => !dict.TryAdd(entry.Tile, entry))) {
                Debug.LogWarning($"Skip duplicated enty type {entry.Tile}");
            }

            return dict;
        }
    }
}
