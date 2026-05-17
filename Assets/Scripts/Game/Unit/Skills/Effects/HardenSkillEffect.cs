namespace Game.Unit.Skills.Effects {
    using System.Collections;
    using Effect.Buff;
    using Map.Battle;
    using Map.Battle.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Harden")]
    public class HardenSkillEffect : AbstractNonDeadSkillEffect {
        [SerializeField] private int amount;
        [SerializeField] private int duration;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            yield return user.EffectController.ApplyEffect(new HardenEffect(this.duration, this.amount));
        }
    }
}
