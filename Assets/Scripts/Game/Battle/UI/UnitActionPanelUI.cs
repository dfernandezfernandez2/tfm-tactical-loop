namespace Game.Battle.UI {
    using System;
    using System.Collections.Generic;
    using Actions;
    using Core;
    using UnityEngine;

    public class UnitActionPanelUI : MonoBehaviour {
        private const int _maxVisibleButtons = 5;

        [SerializeField] private GameObject unitActionPanel;
        [SerializeField] private Transform unitActionButtonContainer;
        [SerializeField] private UnitActionButtonUI buttonPrefab;
        [SerializeField] private TurnManager turnManager;

        private readonly List<UnitActionButtonUI> _buttons = new();
        private IReadOnlyList<IBattleAction> _currentActions;
        private int _firstVisibleIndex;
        private bool _isActive;

        private int _selectedIndexButton = -1;

        private RectTransform _unitActionPanelTransform;

        private void Awake() {
            this._unitActionPanelTransform = this.unitActionPanel.GetComponent<RectTransform>();
            this.Hide();
        }

        private void Update() {
            if (!this._isActive) {
                return;
            }

            if (this.IsMouseOverPanel()) {
                if (InputUtils.IsScrollUpSelected()) {
                    this.SelectPreviousAvailable();
                }

                if (InputUtils.IsScrollDownSelected()) {
                    this.SelectNextAvailable();
                }
            }

            this.HandleKeyboardInput();
        }

        public event Action OnBack;

        public void Init(IReadOnlyList<IBattleAction> actions) => this._currentActions = actions;

        public void Show() {
            this.unitActionPanel.SetActive(true);
            this.BuildButtons();
            this._firstVisibleIndex = 0;
            this.SelectFirstAvailable();
            this._isActive = true;
        }

        public void Hide() {
            this.ClearButtons();
            this.unitActionPanel.SetActive(false);
            this._selectedIndexButton = -1;
            this._firstVisibleIndex = 0;
            this._isActive = false;
        }

        private void RefreshButtons() {
            foreach (UnitActionButtonUI button in this._buttons) {
                button.RefreshIsAvailable();
            }

            if (this._selectedIndexButton >= 0 &&
                this._selectedIndexButton < this._buttons.Count &&
                this._buttons[this._selectedIndexButton].IsAvailable) {
                this.EnsureSelectedIsVisible();
                this.ApplyVisibility();
                this.ApplySelection();
                return;
            }

            this.SelectFirstAvailable(this._selectedIndexButton >= 0 ? this._selectedIndexButton : 0);
        }

        private void BuildButtons() {
            this.ClearButtons();

            for (int i = 0; i < this._currentActions.Count; i++) {
                IBattleAction action = this._currentActions[i];
                UnitActionButtonUI button = Instantiate(this.buttonPrefab, this.unitActionButtonContainer);
                int index = i;

                button.Init(
                    action.GetName(),
                    () => this.turnManager.DoAction(action),
                    () => this.SetSelectedIndex(index),
                    () => this.turnManager.CanDoAction(action)
                );

                this._buttons.Add(button);
            }

            this.ApplyVisibility();
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
            if (this._selectedIndexButton < 0 || this._selectedIndexButton >= this._buttons.Count) {
                return;
            }

            this._buttons[this._selectedIndexButton].OnEnter();
            this.RefreshButtons();
        }

        private void SelectFirstAvailable(int startIndex = 0) {
            if (this._buttons.Count == 0) {
                this.ClearSelection();
                return;
            }

            startIndex = Mathf.Clamp(startIndex, 0, this._buttons.Count - 1);

            for (int i = startIndex; i < this._buttons.Count; i++) {
                if (!this._buttons[i].IsAvailable) {
                    continue;
                }

                this.SetSelectedIndex(i);
                return;
            }

            for (int i = 0; i < startIndex; i++) {
                if (!this._buttons[i].IsAvailable) {
                    continue;
                }

                this.SetSelectedIndex(i);
                return;
            }

            this.ClearSelection();
        }

        private void SelectNextAvailable() {
            if (this._buttons.Count == 0) {
                return;
            }

            int startIndex = this._selectedIndexButton < 0 ? -1 : this._selectedIndexButton;

            for (int offset = 1; offset <= this._buttons.Count; offset++) {
                int index = (startIndex + offset) % this._buttons.Count;
                if (!this._buttons[index].IsAvailable) {
                    continue;
                }

                this.SetSelectedIndex(index);
                return;
            }
        }

        private void SelectPreviousAvailable() {
            if (this._buttons.Count == 0) {
                return;
            }

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
            if (index < 0 || index >= this._buttons.Count) {
                return;
            }

            this._selectedIndexButton = index;
            this.EnsureSelectedIsVisible();
            this.ApplyVisibility();
            this.ApplySelection();
        }

        private void ClearSelection() {
            this._selectedIndexButton = -1;
            this.ApplySelection();
        }

        private void EnsureSelectedIsVisible() {
            if (this._selectedIndexButton < 0) {
                return;
            }

            if (this._selectedIndexButton < this._firstVisibleIndex) {
                this._firstVisibleIndex = this._selectedIndexButton;
            }
            else if (this._selectedIndexButton >= this._firstVisibleIndex + _maxVisibleButtons) {
                this._firstVisibleIndex = this._selectedIndexButton - _maxVisibleButtons + 1;
            }

            int maxStart = Mathf.Max(0, this._buttons.Count - _maxVisibleButtons);
            this._firstVisibleIndex = Mathf.Clamp(this._firstVisibleIndex, 0, maxStart);
        }

        private void ApplyVisibility() {
            for (int i = 0; i < this._buttons.Count; i++) {
                bool isVisible = i >= this._firstVisibleIndex && i < this._firstVisibleIndex + _maxVisibleButtons;
                this._buttons[i].gameObject.SetActive(isVisible);
            }
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

        private bool IsMouseOverPanel() =>
            RectTransformUtility.RectangleContainsScreenPoint(
                this._unitActionPanelTransform,
                Input.mousePosition,
                null
            );
    }
}
