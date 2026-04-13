namespace Game.Battle {
    using Core;
    using Unit;
    using UnityEngine;
    using UnityEngine.UI;

    public class OrderUnitUI : MonoBehaviour {
        [SerializeField] private Image highlightImage;
        [SerializeField] private Image unitImage;
        [SerializeField] private Image unitBorderImage;

        public void SetUnit(UnitObject unit) {
            this.unitImage.sprite = unit.GetSprite();
            this.unitBorderImage.color = unit.GetTeam().GetBattleTeam().GetColor();
            this.SetSelected(false);
        }

        public void SetSelected(bool isSelected) => this.highlightImage.gameObject.SetActive(isSelected);
    }
}
