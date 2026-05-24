namespace Game.Run.Reward {
    using Battle.Item;
    using Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Item")]
    public class ItemReward : ScriptableObject, IReward {
        [SerializeField] private Item item;
        [SerializeField] private Sprite sprite;

        public Sprite GetIcon() => this.sprite;
        public string GetName() => this.item.GetName();
        public string GetDescription() => this.item.GetDescription();
        public void ApplyReward(RunData runData) => RunData.GetInstance().Inventory.Add(this.item);
    }
}
