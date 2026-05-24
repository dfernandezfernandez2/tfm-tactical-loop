namespace Game.Menu {
    using System.Collections.Generic;
    using Controls;
    using UnityEngine;

    [RequireComponent(typeof(GameOptionsManager))]
    public class MainGameOptionsUI : MonoBehaviour {
        [SerializeField] private GameObject mainGameOptionsPanel;
        [SerializeField] private List<MenuButtonUI> buttons;
        private int _currentButtonIndex;

        private GameOptionsManager _gameOptionsManager;

        private bool _isActive;

        private void Awake() {
            this._gameOptionsManager = this.GetComponent<GameOptionsManager>();
            for (int i = 0; i < this.buttons.Count; i++) {
                this.buttons[i].Init(i, this.Select);
            }
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

            if (InputUtils.IsCancelKeyBoardSelected()) {
                this._gameOptionsManager.Hide();
            }
        }

        private void Select(int index) {
            this.buttons[this._currentButtonIndex].UnSelect();
            this._currentButtonIndex = index;
            this.buttons[this._currentButtonIndex].Select();
        }

        public void Show() {
            this._isActive = true;
            this.mainGameOptionsPanel.SetActive(true);
            this._currentButtonIndex = 0;
            this.buttons[this._currentButtonIndex].Select();
            for (int i = 1; i < this.buttons.Count; i++) {
                this.buttons[i].UnSelect();
            }
        }

        public void Hide() {
            this._isActive = false;
            this.mainGameOptionsPanel.SetActive(false);
        }

        private void Movement(int movement) =>
            this.Select(Mathf.Clamp(this._currentButtonIndex + movement, 0, this.buttons.Count - 1));
    }
}
