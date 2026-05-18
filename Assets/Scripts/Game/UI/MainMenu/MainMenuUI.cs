namespace Game.UI.MainMenu {
    using System;
    using System.Collections.Generic;
    using Core;
    using Map.Battle;
    using Map.Battle.Data;
    using Map.Battle.Generation.Data;
    using Map.Battle.Parser;
    using UnityEngine;

    [Serializable]
    public struct UnitElement {
        public Sprite Sprite;
        public Vector2Int Position;
    }

    public class MainMenuUI : MonoBehaviour {
        [SerializeField] private BattleMapLoader battleMapLoader;
        [SerializeField] private GameObject mapGameObject;
        [SerializeField] private Vector3 mapScale;
        [SerializeField] private Vector3 mapPosition;
        [SerializeField] private List<UnitElement> unitElements;
        [SerializeField] private WorldRender worldRender;
        [SerializeField] private List<MenuButtonUI> buttons;
        [SerializeField] private Canvas mainMenuOptionsCanvas;

        private int _currentButtonIndex;
        private bool _isActive;

        private void Awake() {
            this.battleMapLoader.Load(GetMapText());
            foreach (UnitElement unitElement in this.unitElements) {
                GameObject unit = new("Unit");
                unit.transform.SetParent(this.mapGameObject.transform);
                SpriteRenderer sprite = unit.AddComponent<SpriteRenderer>();
                sprite.sprite = unitElement.Sprite;
                sprite.sortingLayerName = "World";
                GridPosition gridPosition = new(unitElement.Position, 0);
                unit.transform.position = this.worldRender.GridToWorld(gridPosition);
                unit.GetComponentInChildren<SpriteRenderer>().sortingOrder = WorldRender.GetSortingOrder(gridPosition);
                unit.transform.localScale = new Vector3(2f, 2f, 2f);
            }

            this.mapGameObject.transform.localScale = this.mapScale;
            this.mapGameObject.transform.position = this.mapPosition;
            this._isActive = true;
            this.buttons[this._currentButtonIndex].Select();
        }

        private void Update() {
            if (!this._isActive) {
                return;
            }

            if (InputUtils.IsDownSelected()) {
                this.Movement(1);
            }

            if (InputUtils.IsUpSelected()) {
                this.Movement(-1);
            }

            if (InputUtils.IsEnterSelected()) {
                this.buttons[this._currentButtonIndex].DoOnClick();
            }
        }

        private void Movement(int movement) {
            this.buttons[this._currentButtonIndex].UnSelect();
            this._currentButtonIndex = Mathf.Clamp(this._currentButtonIndex + movement, 0, this.buttons.Count - 1);
            this.buttons[this._currentButtonIndex].Select();
        }

        public void Show() {
            this._isActive = true;
            this.mainMenuOptionsCanvas.transform.gameObject.SetActive(true);
        }

        public void Hide() {
            this._isActive = false;
            this.mainMenuOptionsCanvas.transform.gameObject.SetActive(false);
        }

        private static string GetMapText() {
            GeneratedTile[,] generatedTiles = new GeneratedTile[9, 5];
            for (int x = 0; x < generatedTiles.GetLength(0); x++) {
                for (int y = 0; y < generatedTiles.GetLength(1); y++) {
                    generatedTiles[x, y] = new GeneratedTile(TileType.Floor, TileTypeVariant.Stone);
                }
            }

            return TxtMapLegend.SerializeMap(generatedTiles);
        }
    }
}
