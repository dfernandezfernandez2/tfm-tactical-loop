namespace Game.Unit.Skills {
    using System.Collections.Generic;
    using Battle;
    using Battle.Item;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Create")]
    public class Skill : ScriptableObject {
        [Header("General")] public string skillName;

        public int apCost;
        public int manaCost;

        [Header("Target")] public Target target;

        public int range;
        public SelectionType selectionType = SelectionType.Default;

        [Header("Animation")] public string animationName;

        [Header("Effect")] public List<SkillEffect> effects = new();
    }
}
