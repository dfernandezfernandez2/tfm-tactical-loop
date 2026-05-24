namespace Game.Run.Reward.UI {
    using System;
    using System.Collections.Generic;
    using Audio;
    using Controls;
    using UnityEngine;

    public class RewardSelectionUI : MonoBehaviour {
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform rewardButtonPanelContainer;
        [SerializeField] private RewardOptionButtonUI rewardButtonPrefab;

        private readonly List<RewardOptionButtonUI> _buttons = new();
        private int _currentOption;

        private bool _isActive;

        private void Awake() => this.panel.SetActive(false);

        private void Update() {
            if (!this._isActive || GameInputLock.IsLocked) {
                return;
            }

            if (InputUtils.IsRightSelected()) {
                this.Select(1);
            }

            if (InputUtils.IsLeftSelected()) {
                this.Select(-1);
            }

            if (InputUtils.IsEnterSelected()) {
                this.OnRewardSelected?.Invoke(this._buttons[this._currentOption].GetReward());
            }
        }

        public event Action<IReward> OnRewardSelected;

        public void Show(List<IReward> rewards) {
            AudioManager.Instance.PlayMusic("reward_selection");
            this.ClearOptions();
            this.panel.SetActive(true);
            this._currentOption = 0;
            this._isActive = true;
            foreach (IReward reward in rewards) {
                RewardOptionButtonUI button = Instantiate(
                    this.rewardButtonPrefab,
                    this.rewardButtonPanelContainer
                );
                button.Init(reward, () => this.OnRewardSelected?.Invoke(reward));
                this._buttons.Add(button);
            }

            this._buttons[this._currentOption].Select();
        }

        public void Hide() {
            this._isActive = false;
            this.panel.SetActive(false);
            this.ClearOptions();
        }

        private void ClearOptions() {
            foreach (RewardOptionButtonUI button in this._buttons) {
                Destroy(button.gameObject);
            }

            this._buttons.Clear();
        }

        private void Select(int movement) {
            this._buttons[this._currentOption].UnSelect();
            this._currentOption = Mathf.Clamp(this._currentOption + movement, 0, this._buttons.Count - 1);
            this._buttons[this._currentOption].Select();
        }
    }
}
