namespace Map.Battle.Renderer {
    using UnityEngine;

    public class WorldRender {
        private readonly GridConfiguration _gridConfiguration;

        public WorldRender(GridConfiguration gridConfiguration) => this._gridConfiguration = gridConfiguration;

        public Vector3 GridToWorld(int x, int y, int height) {
            float isoX = (x - y) * (this._gridConfiguration.TileWidth / 2f);
            float isoY = (x + y) * (this._gridConfiguration.TileHeight / 2f);

            float heightOffset = height * this._gridConfiguration.HeightStep;

            return new Vector3(isoX, isoY + heightOffset, isoY);
        }
    }
}
