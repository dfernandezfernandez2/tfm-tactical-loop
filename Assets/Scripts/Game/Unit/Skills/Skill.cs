namespace Game.Unit.Skills {
    using System.Collections.Generic;
    using Battle.Item;
    using Effects;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Create Skill")]
    public class Skill : ScriptableObject {
        [Header("General")]
        public string skillName;
        public int apCost;
        public int manaCost;

        [Header("Target")]
        public Target target;
        public int range;

        [Header("Animation")]
        public string animationName;

        [Header("Effect")]
        public List<SkillEffect> effects = new();

    }
}
