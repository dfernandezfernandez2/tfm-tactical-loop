namespace Game.Battle.UI {
    using System;
    using System.Collections.Generic;
    using Actions;
    using UnityEngine;

    public class UnitActionPanelUI : MonoBehaviour {
        [SerializeField] private GameObject unitActionPanel;
        [SerializeField] private Transform unitActionButtonContainer;
        [SerializeField] private UnitActionButtonUI buttonPrefab;
        [SerializeField] private TurnManager turnManager;

        private readonly List<UnitActionButtonUI> _buttons = new();

        private int _selectedIndexButton = -1;

        private void Awake() => this.Hide();

        private void Update() {
            if (!this.unitActionPanel.activeSelf) {
                return;
            }

            this.HandleKeyboardInput();
        }

        public void Show() {
            this.unitActionPanel.SetActive(true);
            this.BuildButtons();
            this.SelectFirstAvailable();
        }

        public void Hide() {
            this.ClearButtons();
            this.unitActionPanel.SetActive(false);
            this._selectedIndexButton = -1;
        }

        private void RefreshButtons() {
            foreach (UnitActionButtonUI button in this._buttons) {
                bool canDoAction = this.turnManager.CanDoAction(button.ActionType);
                button.SetAvailable(canDoAction);
            }

            this.SelectFirstAvailable(this._selectedIndexButton);
        }

        private void BuildButtons() {
            this.ClearButtons();
            foreach (ActionType actionType in Enum.GetValues(typeof(ActionType))) {
                UnitActionButtonUI button = Instantiate(this.buttonPrefab, this.unitActionButtonContainer);
                bool canDoAction = this.turnManager.CanDoAction(actionType);
                button.Init(actionType, type => this.turnManager.DoAction(type), canDoAction);
                this._buttons.Add(button);
            }
        }

        private void ClearButtons() {
            foreach (UnitActionButtonUI button in this._buttons) {
                Destroy(button.gameObject);
            }

            this._buttons.Clear();
        }

        private void HandleKeyboardInput() {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                this.SelectPreviousAvailable();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                this.SelectNextAvailable();
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
                this.ExecuteSelected();
            }
        }

        private void ExecuteSelected() {
            this.turnManager.DoAction(this._buttons[this._selectedIndexButton].ActionType);
            this.RefreshButtons();
        }

        private void SelectFirstAvailable(int startIndex = 0) {
            this.ClearSelection();
            int index = this._buttons.FindIndex(startIndex, b => b.IsAvailable);
            if (index != -1) {
                this.SetSelectedIndex(index);
            }
            else if (startIndex >= 0) {
                // case that there are not buttons available from the requested index and need to iterate from the first
                this.SelectFirstAvailable();
            }
        }

        private void SelectNextAvailable() => this.SelectFirstAvailable(this._selectedIndexButton + 1);

        private void SelectPreviousAvailable() {
            int startIndex = this._selectedIndexButton < 0 ? 0 : this._selectedIndexButton;
            for (int offset = 1; offset <= this._buttons.Count; offset++) {
                int index = (startIndex - offset + this._buttons.Count) % this._buttons.Count;
                if (!this._buttons[index].IsAvailable) {
                    continue;
                }

                this.SetSelectedIndex(index);
                return;
            }
        }

        private void SetSelectedIndex(int index) {
            this._selectedIndexButton = index;
            this.ApplySelection();
        }

        private void ClearSelection() {
            this._selectedIndexButton = -1;
            this.ApplySelection();
        }

        private void ApplySelection() {
            for (int i = 0; i < this._buttons.Count; i++) {
                this._buttons[i].SetSelected(i == this._selectedIndexButton);
            }
        }
    }
}
