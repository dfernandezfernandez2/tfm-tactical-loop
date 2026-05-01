namespace Game.Reward {
    using System;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class RewardOptionButtonUI : MonoBehaviour {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text rewardName;
        [SerializeField] private Image background;
        [SerializeField] private Sprite selectedBackground;
        [SerializeField] private Sprite unSelectedBackground;

        private IReward _reward;

        public void Init(IReward reward, Action onClick) {
            this._reward = reward;
            this.rewardName.text = reward.GetName() + "\n" + reward.GetDescription();
            this.button.image.sprite = reward.GetIcon();
            this.button.onClick.RemoveAllListeners();
            this.button.onClick.AddListener(() => onClick?.Invoke());
        }

        public IReward GetReward() => this._reward;

        public void Select() {
            this.background.sprite = this.selectedBackground;
            Color color = this.background.color;
            color.a = 1f;
            this.background.color = color;
        }

        public void UnSelect() {
            this.background.sprite = this.unSelectedBackground;
            Color color = this.background.color;
            color.a = 0f;
            this.background.color = color;
        }
    }
}
