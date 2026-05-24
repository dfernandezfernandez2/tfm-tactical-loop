namespace Game.Menu {
    using Controls;
    using Options;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    [RequireComponent(typeof(GameOptionsUI))]
    [RequireComponent(typeof(MainGameOptionsUI))]
    public class GameOptionsManager : MonoBehaviour {
        [SerializeField] private Button optionButton;
        private GameOptionsUI _gameOptionsUI;
        private bool _hasGameplayLock;
        private bool _isActive;
        private MainGameOptionsUI _mainGameOptionsUI;
        private float _previousTimeScale = 1f;

        private void Awake() {
            this._gameOptionsUI = this.GetComponent<GameOptionsUI>();
            this._mainGameOptionsUI = this.GetComponent<MainGameOptionsUI>();
        }

        private void Update() {
            if (this._isActive) {
                return;
            }

            if (InputUtils.IsPauseKeyboardSelected()) {
                this.ShowMainOptions();
            }
        }

        public void ShowMainOptions() {
            this.PauseGameplay();
            this._isActive = true;
            this.optionButton.gameObject.SetActive(false);
            this._gameOptionsUI.Hide();
            this._mainGameOptionsUI.Show();
        }

        public void ShowOptionsPanel() {
            this.PauseGameplay();
            this._isActive = true;
            this._mainGameOptionsUI.Hide();
            this._gameOptionsUI.Show();
        }

        public void Hide() {
            this.ResumeGameplay();
            this._isActive = false;
            this.optionButton.gameObject.SetActive(true);
            this._mainGameOptionsUI.Hide();
            this._gameOptionsUI.Hide();
        }

        public void ExitGame() {
            this.ResumeGameplay();
            Application.Quit();
        }

        public void RestartGame() {
            this.ResumeGameplay();
            SceneManager.LoadScene("TeamSelectionScene");
        }

        private void PauseGameplay() {
            if (this._hasGameplayLock) {
                return;
            }

            this._hasGameplayLock = true;
            GameInputLock.Lock();
            this._previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        private void ResumeGameplay() {
            if (!this._hasGameplayLock) {
                return;
            }

            this._hasGameplayLock = false;
            GameInputLock.Unlock();
            Time.timeScale = this._previousTimeScale;
        }
    }
}
