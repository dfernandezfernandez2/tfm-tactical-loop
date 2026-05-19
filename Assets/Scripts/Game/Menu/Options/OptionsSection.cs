namespace Game.Menu.Options {
    using UnityEngine;

    public abstract class OptionsSection : MonoBehaviour {
        public void Show() => this.gameObject.SetActive(true);

        public void Hide() => this.gameObject.SetActive(false);

        public abstract string GetTitle();
    }
}
