namespace Game.UI.End {
    using System.Collections.Generic;
    using Core;
    using MainMenu;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class GameOverManager : MonoBehaviour {
        [SerializeField] private List<MenuButtonUI> buttons;
        [SerializeField] private Canvas main;
        [SerializeField] private Canvas credits;

        private int _currentButtonIndex;
        private bool _isCreditsActive;

        private void Awake() => this.buttons[this._currentButtonIndex].Select();

        private void Update() {
            if (!this._isCreditsActive) {
                this.MainUpdate();
            }
            else {
                this.CreditsUpdate();
            }
        }

        private void Movement(int movement) {
            this.buttons[this._currentButtonIndex].UnSelect();
            this._currentButtonIndex = Mathf.Clamp(this._currentButtonIndex + movement, 0, this.buttons.Count - 1);
            this.buttons[this._currentButtonIndex].Select();
        }

        public void ExitGame() => Application.Quit();

        public void StartGame() => SceneManager.LoadScene("TeamSelectionScene");

        private void MainUpdate() {
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

        private void CreditsUpdate() {
            if (!InputUtils.IsCancelSelected()) {
                return;
            }

            this._isCreditsActive = false;
            this.main.gameObject.SetActive(true);
            this.credits.gameObject.SetActive(false);
        }

        public void ShowCredits() {
            this._isCreditsActive = true;
            this.main.gameObject.SetActive(false);
            this.credits.gameObject.SetActive(true);
        }
    }
}
