namespace Game.Unit {
    using System.Collections.Generic;
    using System.Linq;
    using Battle.Actions;
    using Skills;
    using UnityEngine;

    public class UnitActions : MonoBehaviour {
        [SerializeField] private List<Skill> skills = new();

        private readonly IReadOnlyList<IBattleAction> _basicActions = new List<IBattleAction> {
            new MovementSelectionAction(),
            new AttackSelectionAction(),
            new SkillSelectionAction(),
            new ItemSelectionAction(),
            new WaitAction()
        };

        private readonly List<SkillAction> _skillActions = new();

        private void Awake() {
            foreach (Skill skill in this.skills) {
                this._skillActions.Add(new SkillAction(skill));
            }
        }

        public IReadOnlyList<IBattleAction> GetBasicActions() => this._basicActions;
        public IReadOnlyList<IBattleAction> GetSkillActions() => this._skillActions.AsReadOnly();

        public IReadOnlyList<IBattleAction> GetAllAvailableActions() =>
            this.GetBasicActions().Concat(
                this.GetSkillActions()).ToList();
    }
}
