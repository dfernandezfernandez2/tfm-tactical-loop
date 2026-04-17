namespace Game.Battle.Item {
    using UnityEngine;

    [CreateAssetMenu(menuName = "Item")]
    public class Item : ScriptableObject {
        public string itemName;
        [TextArea] public string description;
        public ItemTarget target;
    }
}
