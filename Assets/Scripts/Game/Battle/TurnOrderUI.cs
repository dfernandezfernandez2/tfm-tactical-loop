namespace Game.Battle {
    using System.Collections.Generic;
    using Unit;
    using UnityEngine;

    public class TurnOrderUI : MonoBehaviour {
        [SerializeField] private GameObject turnOrderPanel;
        [SerializeField] private OrderUnitUI orderUnitUIPrefab;

        private readonly List<UnitObject> _units = new();
        private readonly List<OrderUnitUI> _unitsUI = new();
        private int _currentTurnIndex;

        private void Awake() => this.turnOrderPanel.SetActive(false);

        public void Reset() {
            this._currentTurnIndex = 0;
            this._units.Clear();
            this._unitsUI.Clear();
        }

        public void Show(List<UnitObject> units) {
            this.Reset();
            this._units.AddRange(units);
            foreach (UnitObject unit in units) {
                OrderUnitUI orderUnitUI = Instantiate(this.orderUnitUIPrefab, this.turnOrderPanel.transform);
                orderUnitUI.SetUnit(unit);
                this._unitsUI.Add(orderUnitUI);
            }

            this.turnOrderPanel.SetActive(true);
        }

        public void UpdateCurrentTurn(int turn) {
            this._unitsUI[this._currentTurnIndex].SetSelected(false);
            this._currentTurnIndex = turn;
            this._unitsUI[this._currentTurnIndex].SetSelected(true);
        }

        public void Hide() => this.turnOrderPanel.SetActive(false);
    }
}
