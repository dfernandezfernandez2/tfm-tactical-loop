namespace Map {
    using UnityEngine;

    public class WorldRender : MonoBehaviour {
        [SerializeField] private GridConfiguration gridConfiguration;

        public Vector3 GridToWorld(GridPosition gridPosition) {
            float isoX = (gridPosition.Position.x - gridPosition.Position.y) * (this.gridConfiguration.TileWidth / 2f);
            float isoY = (gridPosition.Position.x + gridPosition.Position.y) *
                         (this.gridConfiguration.TileHeight / 2f);

            float heightOffset = gridPosition.Height * this.gridConfiguration.HeightStep;

            return new Vector3(isoX, isoY + heightOffset, isoY);
        }
    }
}
