namespace Game.UI {
    using TMPro;
    using Unit;
    using UnityEngine;
    using UnityEngine.UI;

    public class UnitSelectionUI : MonoBehaviour {
        [SerializeField] private TMP_Text unitText;
        [SerializeField] private Image unitImage;
        private Color _initialColor;

        private Sprite _initialSprite;
        private string _initialText;

        private UnitObject _unitObject;

        private void Awake() {
            this._initialSprite = this.unitImage.sprite;
            this._initialText = this.unitText.text;
            this._initialColor = this.unitImage.color;
        }

        public void SetUnit(UnitObject unitObject) {
            if (unitObject != null) {
                this.unitText.text = unitObject.GetName();
                this.unitImage.sprite = unitObject.GetSprite();
                Color unitImageColor = this.unitImage.color;
                unitImageColor.a = 1f;
                this.unitImage.color = unitImageColor;
                this._unitObject = unitObject;
            }
            else {
                this.unitText.text = this._initialText;
                this.unitImage.sprite = this._initialSprite;
                this.unitImage.color = this._initialColor;
                this._unitObject = null;
            }
        }

        public UnitObject GetUnit() => this._unitObject;
    }
}
