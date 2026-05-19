namespace Game.Battle.Unit.Skills.Effects {
    using System.Collections;
    using Effect.Buff;
    using Map;
    using Map.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Courage")]
    public class CourageSkillEffect : AbstractNonDeadSkillEffect {
        [SerializeField] private int amount;
        [SerializeField] private int duration;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            yield return user.EffectController.ApplyEffect(new CourageEffect(this.duration, this.amount));
        }
    }
}
