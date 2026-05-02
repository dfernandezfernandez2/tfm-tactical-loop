namespace Game.UI.MainMenu.Options {
    using UnityEngine;
    using UnityEngine.UI;

    public class LanguageUI : MonoBehaviour {
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Image background;

        public void Select() {
            this.background.sprite = this.selectedSprite;
            Color color = this.background.color;
            color.a = 1f;
            this.background.color = color;
        }

        public void UnSelect() {
            this.background.sprite = null;
            Color color = this.background.color;
            color.a = 0f;
            this.background.color = color;
        }
    }
}
