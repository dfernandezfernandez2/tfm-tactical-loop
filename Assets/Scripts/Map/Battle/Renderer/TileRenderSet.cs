namespace Map.Battle.Renderer {
    using System.Collections.Generic;
    using Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Map/Tiles Prefab")]
    public class TileRenderSet : ScriptableObject {
        [SerializeField] private List<TileRenderElement> entries = new();

        public Dictionary<TileType, TileRenderElement> ToDict() {
            Dictionary<TileType, TileRenderElement> dict = new();
            foreach (TileRenderElement entry in this.entries) {
                if (!dict.TryAdd(entry.Type, entry)) {
                    Debug.LogWarning($"Skip duplicated enty type {entry.Type}");
                }
            }

            return dict;
        }
    }
}
