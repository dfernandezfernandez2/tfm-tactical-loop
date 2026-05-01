namespace Game.UI {
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class MainMenuButtonUI : MonoBehaviour {

        [SerializeField] private Image image;
        [SerializeField] private Sprite selectedSpriteBackground;

        private Button _button;

        private void Awake() => this._button = this.GetComponent<Button>();

        public void Select() {
            this.image.sprite = this.selectedSpriteBackground;
            Color color = this.image.color;
            color.a = 1f;
            this.image.color = color;
        }

        public void UnSelect() {
            this.image.sprite = null;
            Color color = this.image.color;
            color.a = 0f;
            this.image.color = color;
        }

        public void DoOnClick() => this._button.onClick.Invoke();
    }
}
