namespace Game.UI.MainMenu {
    using System;
    using System.Collections.Generic;
    using Game.Core;
    using Game.Map.Battle;
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
        [SerializeField] private List<MainMenuButtonUI> buttons;
        [SerializeField] private Canvas mainMenuOptionsCanvas;

        private int _currentButtonIndex;
        private bool _isActive;

        private void Awake() {
            TextAsset map = Resources.Load<TextAsset>("Map/Battle/MainMenu");
            this.battleMapLoader.Load(map.text);
            foreach (UnitElement unitElement in this.unitElements) {
                GameObject unit = new("Unit");
                unit.transform.SetParent(this.mapGameObject.transform);
                SpriteRenderer sprite = unit.AddComponent<SpriteRenderer>();
                sprite.sprite = unitElement.Sprite;
                sprite.sortingLayerName = "Unit";
                unit.transform.position = this.worldRender.GridToWorld(new GridPosition(unitElement.Position, 0));
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
    }
}
