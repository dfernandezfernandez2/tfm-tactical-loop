namespace Game.Map.Battle.Renderer {
    using System;
    using Data;
    using UnityEngine;

    [Serializable]
    public class TileRenderElement {
        [SerializeField] private Tile tile;
        [SerializeField] private GameObject prefab;

        public Tile Tile => this.tile;
        public GameObject Prefab => this.prefab;
    }
}
