namespace Game.Reward {
    using Battle.Item;
    using Core.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Reward/Item")]
    public class ItemReward : ScriptableObject, IReward {
        [SerializeField] private Item item;
        [SerializeField] private Sprite sprite;

        public Sprite GetIcon() => this.sprite;
        public string GetName() => this.item.itemName;
        public string GetDescription() => this.item.description;
        public void ApplyReward(RunData runData) => RunData.GetInstance().Inventory.Add(this.item);
    }
}
