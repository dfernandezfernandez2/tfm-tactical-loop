namespace Game.Battle.Item {
    using Effects;
    using Translation;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Create Item")]
    public class Item : ScriptableObject {
        [Header("Translation")] [SerializeField]
        private string id;

        public Target target;
        public ItemEffect effect;

        public string GetName() => TranslatorManager.Get($"item.{this.id}.name");
        public string GetDescription() => TranslatorManager.Get($"item.{this.id}.description");
        public string GetId() => this.id;
    }
}
