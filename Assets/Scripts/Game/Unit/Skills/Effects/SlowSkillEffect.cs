namespace Game.Unit.Skills.Effects {
    using System.Collections;
    using Effect.Status;
    using Map.Battle;
    using Map.Battle.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unit/Skills/Effects/Slow")]
    public class SlowSkillEffect : AbstractNonDeadSkillEffect {
        [SerializeField] private int duration;
        [SerializeField] private int amount;

        public override IEnumerator Apply(UnitObject user, GridPosition target, BattleMapManager battleMapManager) {
            UnitObject unitObjectTarget = battleMapManager.GetUnit(target);
            yield return unitObjectTarget.EffectController.ApplyEffect(new SlowEffect(this.duration, this.amount));
        }
    }
}
