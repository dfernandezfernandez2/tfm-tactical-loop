namespace Game.Battle.Map.Data {
    using System;
    using UnityEngine;

    [Serializable]
    public class GridConfiguration {
        [SerializeField] private float tileWidth;
        [SerializeField] private float tileHeight;
        [SerializeField] private float pixelsPerUnit = 100f;
        [SerializeField] private int scale = 3;
        [SerializeField] private float unitAnchorYOffset;

        public float TileWidth => this.tileWidth / this.pixelsPerUnit * this.scale;
        public float TileHeight => this.tileHeight / this.pixelsPerUnit * this.scale / 2f;
        public float UnitAnchorYOffset => this.unitAnchorYOffset;
    }
}
