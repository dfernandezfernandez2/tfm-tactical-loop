namespace Game.Battle.Item {
    using Effects;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item/Create Item")]
    public class Item : ScriptableObject {
        public string itemName;
        [TextArea] public string description;
        public Target target;
        public ItemEffect effect;
    }
}
