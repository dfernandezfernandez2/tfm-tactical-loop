namespace Game.Battle {
    using TMPro;
    using Unit;
    using UnityEngine;

    public class OrderUnitUI : MonoBehaviour {
        [SerializeField] private TMP_Text labelText;
        private bool _isSelected;

        private UnitObject _unitObject;

        private void Update() {
            this.labelText.text = this._unitObject?.GetName() ?? "";
            if (this._unitObject != null) {
                this.labelText.text += " team " + this._unitObject.GetTeam().GetBattleTeam();
            }

            if (this._unitObject?.GetUnit()?.IsDead() ?? true) {
                this.labelText.text += " - dead";
            }

            if (this._isSelected) {
                this.labelText.text += " - selected";
            }
        }

        public void SetUnit(UnitObject unit) => this._unitObject = unit;
        public bool SetSelected(bool isSelected) => this._isSelected = isSelected;
    }
}
