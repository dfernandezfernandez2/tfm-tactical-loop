namespace Game.Map.Battle {
    using System;
    using UnityEngine;

    [Serializable]
    public class GridConfiguration {
        [SerializeField] private float tileWidth;
        [SerializeField] private float tileHeight;
        [SerializeField] private float heightStep;
        [SerializeField] private float unitAnchorYOffset;

        public float TileWidth => this.tileWidth;
        public float TileHeight => this.tileHeight;
        public float HeightStep => this.heightStep;
        public float UnitAnchorYOffset => this.unitAnchorYOffset;
    }
}
