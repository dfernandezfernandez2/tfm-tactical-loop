namespace Game.Battle.UI {
    using System;
    using System.Collections.Generic;
    using Actions;
    using Core;
    using UnityEngine;

    public class UnitActionPanelUI : MonoBehaviour {
        [SerializeField] private GameObject unitActionPanel;
        [SerializeField] private Transform unitActionButtonContainer;
        [SerializeField] private UnitActionButtonUI buttonPrefab;
        [SerializeField] private TurnManager turnManager;

        private readonly List<UnitActionButtonUI> _buttons = new();
        private IReadOnlyList<IBattleAction> _currentActions;
        private bool _isActive;

        private int _selectedIndexButton = -1;

        private void Awake() => this.Hide();

        private void Update() {
            if (!this._isActive) {
                return;
            }

            this.HandleKeyboardInput();
        }

        public event Action OnBack;

        public void Init(IReadOnlyList<IBattleAction> actions) => this._currentActions = actions;

        public void Show() {
            this.unitActionPanel.SetActive(true);
            this.BuildButtons();
            this.SelectFirstAvailable();
            this._isActive = true;
        }

        public void Hide() {
            this.ClearButtons();
            this.unitActionPanel.SetActive(false);
            this._selectedIndexButton = -1;
            this._isActive = false;
        }

        private void RefreshButtons() {
            foreach (UnitActionButtonUI button in this._buttons) {
                button.RefreshIsAvailable();
            }

            this.SelectFirstAvailable(this._selectedIndexButton);
        }

        private void BuildButtons() {
            this.ClearButtons();
            for (int i = 0; i < this._currentActions.Count; i++) {
                IBattleAction action = this._currentActions[i];
                UnitActionButtonUI button = Instantiate(this.buttonPrefab, this.unitActionButtonContainer);
                int index = i;
                button.Init(action.GetName(), () => this.turnManager.DoAction(action),
                    () => this.SetSelectedIndex(index),
                    () => this.turnManager.CanDoAction(action));
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
            if (InputUtils.IsUpSelected()) {
                this.SelectPreviousAvailable();
            }

            if (InputUtils.IsDownSelected()) {
                this.SelectNextAvailable();
            }

            if (InputUtils.IsEnterSelected()) {
                this.ExecuteSelected();
            }

            if (InputUtils.IsCancelSelected()) {
                this.GoBack();
            }
        }

        private void ExecuteSelected() {
            this._buttons[this._selectedIndexButton].OnEnter();
            this.RefreshButtons();
        }

        private void SelectFirstAvailable(int startIndex = 0) {
            if (startIndex == -1) {
                return;
            }

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

        private void GoBack() {
            this.OnBack?.Invoke();
            this.OnBack = null;
        }
    }
}
