namespace Game.Battle.Unit.Skills {
    using System.Collections.Generic;
    using Item;
    using Selection;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Create")]
    public class Skill : ScriptableObject {
        [Header("General")] public string skillName;

        public int apCost;
        public int manaCost;

        [Header("Target")] public Target target;

        public int range;
        public bool applyHeightLineOfSight;
        public SelectionType selectionType = SelectionType.Default;

        [Header("Animation")] public string animationName;

        [Header("Sound")] public string soundName;

        [Header("Effect")] public List<SkillEffect> effects = new();
    }
}
