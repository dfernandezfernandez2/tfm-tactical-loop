namespace Game.Battle.Map {
    using Data;
    using UnityEngine;

    public class WorldRender : MonoBehaviour {
        [SerializeField] private GridConfiguration gridConfiguration;

        public Vector3 GridToWorldTiles(GridPosition gridPosition) {
            float width = this.gridConfiguration.TileWidth / 2f;
            float height = this.gridConfiguration.TileHeight / 2f;

            float isoX = (gridPosition.Position.x - gridPosition.Position.y) * width;
            float isoY = (gridPosition.Position.x + gridPosition.Position.y) * height;

            float heightOffset = gridPosition.Height * height;

            return new Vector3(isoX, isoY + heightOffset, 0f);
        }

        public Vector3 GridToWorld(GridPosition gridPosition) {
            Vector3 gridPositionWorld = this.GridToWorldTiles(gridPosition);
            gridPositionWorld.y += this.gridConfiguration.UnitAnchorYOffset;
            gridPositionWorld.z = -1;
            return gridPositionWorld;
        }

        public Vector2Int WorldToGrid(Vector3 position) {
            float halfWidth = this.gridConfiguration.TileWidth / 2f;
            float halfHeight = this.gridConfiguration.TileHeight / 2f;

            float x = ((position.x / halfWidth) + (position.y / halfHeight)) / 2f;
            float y = ((position.y / halfHeight) - (position.x / halfWidth)) / 2f;

            int gridX = Mathf.RoundToInt(x);
            int gridY = Mathf.RoundToInt(y);

            return new Vector2Int(gridX - 1, gridY - 1);
        }

        public static int GetSortingOrder(GridPosition gridPosition) {
            int baseOrder = -(gridPosition.Position.x + gridPosition.Position.y) * 100;
            int heightOrder = gridPosition.Height * 10;
            return baseOrder + heightOrder;
        }
    }
}
