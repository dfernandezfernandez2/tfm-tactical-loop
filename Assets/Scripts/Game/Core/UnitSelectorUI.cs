namespace Game.Core {
    using System.Collections.Generic;
    using Battle;
    using Unit;
    using UnityEngine;

    public class UnitSelectorUI : MonoBehaviour {
        [SerializeField] private GameObject unitsPanel;
        [SerializeField] private OrderUnitUI unitPrefab;

        private readonly List<OrderUnitUI> _units = new();

        public void Init(IReadOnlyList<UnitObject> unitObjects) {
            foreach (UnitObject unit in unitObjects) {
                OrderUnitUI orderUnitUI = Instantiate(this.unitPrefab, this.unitsPanel.transform);
                orderUnitUI.SetUnit(unit);
                this._units.Add(orderUnitUI);
            }
        }

        public void End() {
            foreach (OrderUnitUI unit in this._units) {
                Destroy(unit.gameObject);
            }

            this._units.Clear();
        }

        public void SetSelected(int index, bool selected) {
            if (index < 0 || index >= this._units.Count) {
                return;
            }

            this._units[index].SetSelected(selected);
        }
    }
}
