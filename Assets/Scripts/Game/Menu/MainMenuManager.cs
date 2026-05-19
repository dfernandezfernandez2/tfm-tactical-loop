namespace Game.Menu {
    using Options;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [RequireComponent(typeof(MainMenuUI))]
    [RequireComponent(typeof(OptionsUI))]
    public class MainMenuManager : MonoBehaviour {
        private MainMenuUI _mainMenuUI;
        private OptionsUI _optionsUI;

        private void Awake() {
            this._mainMenuUI = this.GetComponent<MainMenuUI>();
            this._optionsUI = this.GetComponent<OptionsUI>();
        }

        public void ExitGame() => Application.Quit();

        public void StartGame() => SceneManager.LoadScene("TeamSelectionScene");

        public void Options() {
            this._mainMenuUI.Hide();
            this._optionsUI.Show();
        }

        public void ReturnToMenu() {
            this._mainMenuUI.Show();
            this._optionsUI.Hide();
        }
    }
}
