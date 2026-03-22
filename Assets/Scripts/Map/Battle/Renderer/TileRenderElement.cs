namespace Map.Battle.Renderer {
    using System;
    using Data;
    using UnityEngine;

    [Serializable]
    public class TileRenderElement {
        [SerializeField] private TileType type;
        [SerializeField] private GameObject prefab;

        public TileType Type => this.type;
        public GameObject Prefab => this.prefab;
    }
}
