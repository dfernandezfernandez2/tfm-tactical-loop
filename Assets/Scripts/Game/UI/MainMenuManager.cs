namespace Game.UI {
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [RequireComponent(typeof(MainMenuUI))]
    public class MainMenuManager : MonoBehaviour {

        private MainMenuUI  _mainMenuUI;

        private void Awake() {
            this._mainMenuUI = this.GetComponent<MainMenuUI>();
        }

        public void ExitGame() => Application.Quit();

        public void StartGame() => SceneManager.LoadScene("TeamSelectionScene");

        public void Options() {
            this._mainMenuUI.Hide();
        }
    }
}
