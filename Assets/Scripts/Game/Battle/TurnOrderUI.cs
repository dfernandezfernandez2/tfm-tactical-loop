namespace Game.Battle {
    using System;
    using System.Collections.Generic;
    using Core;
    using Unit;
    using UnityEngine;

    public class TurnOrderUI : MonoBehaviour {
        [SerializeField] private GameObject turnOrderPanelView;
        [SerializeField] private GameObject turnOrderPanel;
        [SerializeField] private OrderUnitUI orderUnitUIPrefab;

        private readonly List<UnitObject> _units = new();
        private readonly List<OrderUnitUI> _unitsUI = new();
        private int _currentTurnIndex;
        private int _viewDeepNumber;
        private int _viewStartIndex;

        private void Awake() => this.turnOrderPanelView.SetActive(false);

        public void Reset() {
            this._units.Clear();
            this.ResetUnitsUI();
            this._currentTurnIndex = 0;
            this._viewStartIndex = 0;
        }

        private void Update() {
            if (InputUtils.IsSwapNextSelected()) {
                this.Move(1);
            }

            if (InputUtils.IsSwapPreviousSelected()) {
                this.Move(-1);
            }

            if (InputUtils.IsRestoreSelected()) {
                this.UpdateCurrentTurn(this._currentTurnIndex);
            }
        }

        private void ResetUnitsUI() {
            foreach (OrderUnitUI orderUnitUI in this._unitsUI) {
                Destroy(orderUnitUI.gameObject);
            }

            this._unitsUI.Clear();
        }

        public void Show(List<UnitObject> units, int deepNumber) {
            this.Reset();
            this._viewDeepNumber = Math.Min(deepNumber, units.Count);
            this._units.AddRange(units);
            this.turnOrderPanelView.SetActive(true);
        }

        private void Show(int from, int number) {
            from = WrapIndex(from, this._units.Count);

            while (number > 0) {
                OrderUnitUI orderUnitUI = Instantiate(this.orderUnitUIPrefab, this.turnOrderPanel.transform);
                orderUnitUI.SetUnit(this._units[from]);
                this._unitsUI.Add(orderUnitUI);

                from = (from + 1) % this._units.Count;
                number--;
            }
        }

        public void UpdateCurrentTurn(int turn) {
            this._currentTurnIndex = WrapIndex(turn, this._units.Count);
            this.RestoreView();
        }

        private void RestoreView() {
            this._viewStartIndex = this._currentTurnIndex;
            this.RebuildView();
        }

        private void Move(int movement) {
            this._viewStartIndex = WrapIndex(this._viewStartIndex + movement, this._units.Count);
            this.RebuildView();
        }

        private void RebuildView() {
            this.ResetUnitsUI();
            this.Show(this._viewStartIndex, this._viewDeepNumber);
            int selectedIndex = this.GetVisibleIndexOfCurrentTurn();
            if (selectedIndex >= 0) {
                this._unitsUI[selectedIndex].SetSelected(true);
            }
        }

        private int GetVisibleIndexOfCurrentTurn() {
            for (int i = 0; i < this._viewDeepNumber; i++) {
                int unitIndex = WrapIndex(this._viewStartIndex + i, this._units.Count);
                if (unitIndex == this._currentTurnIndex) {
                    return i;
                }
            }

            return -1;
        }

        public void Hide() => this.turnOrderPanelView.SetActive(false);

        private static int WrapIndex(int index, int count) => ((index % count) + count) % count;
    }
}
