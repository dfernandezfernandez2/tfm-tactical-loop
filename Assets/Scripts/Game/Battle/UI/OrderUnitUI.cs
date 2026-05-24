namespace Game.Battle.UI {
    using Data;
    using Unit;
    using UnityEngine;
    using UnityEngine.UI;

    public class OrderUnitUI : MonoBehaviour {
        [SerializeField] private Image highlightImage;
        [SerializeField] private Image unitImage;
        [SerializeField] private Image unitBorderImage;
        [SerializeField] private Image deadUnitImage;

        private UnitObject _unit;

        private void Update() {
            if (this.deadUnitImage == null || this._unit == null) {
                return;
            }

            this.deadUnitImage.gameObject.SetActive(this._unit.Unit.IsDead());
        }

        public void SetUnit(UnitObject unit) {
            this._unit = unit;
            this.unitImage.sprite = unit.GetSprite();
            if (this.unitBorderImage != null) {
                this.unitBorderImage.color = unit.Team.GetBattleTeam().GetColor();
            }

            this.SetSelected(false);
        }

        public void SetSelected(bool isSelected) => this.highlightImage.gameObject.SetActive(isSelected);
    }
}
