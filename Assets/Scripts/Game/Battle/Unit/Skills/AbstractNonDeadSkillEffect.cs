namespace Game.Battle.Unit.Skills {
    public abstract class AbstractNonDeadSkillEffect : SkillEffect {
        public override bool CanApply(UnitObject target) => target != null && !target.Unit.IsDead();
    }
}
