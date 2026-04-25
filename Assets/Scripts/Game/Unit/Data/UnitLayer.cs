namespace Game.Unit.Data {
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public readonly struct UnitLayer {
        private readonly Dictionary<Vector2Int, int> _directionsLayers;

        public UnitLayer(params KeyValuePair<int, Vector2Int>[] directionsLayers) {
            this._directionsLayers = new Dictionary<Vector2Int, int>();
            foreach (KeyValuePair<int, Vector2Int> directionsLayer in directionsLayers) {
                this._directionsLayers.Add(directionsLayer.Value, directionsLayer.Key);
            }
        }

        public List<KeyValuePair<int, float>> GetChangeLayer(Vector2Int direction) => this._directionsLayers
            .Select(keyValuePair =>
                new KeyValuePair<int, float>(keyValuePair.Value, direction == keyValuePair.Key ? 1f : 0f)).ToList();
    }
}
