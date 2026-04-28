namespace Game.Map.Run.UI {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Serialization;

    [CreateAssetMenu(menuName = "Map/Encounter Nodes")]
    public class EncounterTypeNodeSet : ScriptableObject {
        [SerializeField] private List<EncounterNode> entries = new();

        public Dictionary<EncounterType, Sprite> ToDict() {
            Dictionary<EncounterType, Sprite> dict = new();
            foreach (EncounterNode entry in this.entries.Where(entry => !dict.TryAdd(entry.type, entry.sprite))) {
                Debug.LogWarning($"Skip duplicated enty type {entry.type}");
            }

            return dict;
        }
    }

    [Serializable]
    public class EncounterNode {
        [FormerlySerializedAs("Type")] [SerializeField]
        public EncounterType type;

        [FormerlySerializedAs("Sprite")] [SerializeField]
        public Sprite sprite;
    }
}
