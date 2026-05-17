namespace Game.Unit {
    using System;
    using Data;
    using global::Unit.Data;
    using Map.Battle.Data;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class UnitDamageResolver {
        private readonly Func<StatType, float> _getCurrentStat;
        private readonly Func<GridPosition> _getGridPosition;

        public UnitDamageResolver(Func<StatType, float> getCurrentStat, Func<GridPosition> getGridPosition) {
            this._getCurrentStat = getCurrentStat;
            this._getGridPosition = getGridPosition;
        }

        public AttackResult DoAttack(Unit objective, int? fixedDamage = null, bool canFail = true,
            bool applyDefense = true, bool canCrit = true) {
            if (objective == null) {
                return AttackResult.Miss();
            }

            if (objective.IsDead()) {
                return AttackResult.Miss(true);
            }

            if (canFail && !this.RollHit(objective)) {
                return AttackResult.Miss();
            }

            bool isCrit = canCrit && this.RollCrit();
            int resolvedDamage = this.CalculateDamage(objective, isCrit, fixedDamage, applyDefense);
            objective.AddStat(StatType.Hp, -resolvedDamage);
            return AttackResult.Hit(resolvedDamage, isCrit, objective.IsDead());
        }

        private bool RollHit(Unit target) {
            float accuracy = this._getCurrentStat(StatType.Accuracy);
            float evasion = target.GetCurrentStat(StatType.Evasion);

            float hitChance = accuracy / (accuracy + evasion);
            hitChance = Mathf.Clamp(hitChance, 0.1f, 0.95f);

            return Random.value <= hitChance;
        }

        private bool RollCrit() {
            float critChance = this._getCurrentStat(StatType.CritChance);
            return Random.value <= critChance;
        }

        private int CalculateDamage(Unit target, bool isCrit, int? damage = -1, bool applyDefense = false) {
            float atk = damage ?? this._getCurrentStat(StatType.Atk);
            float def = applyDefense ? target.GetCurrentStat(StatType.Def) : 0f;

            float dmg = isCrit ? atk * 1.5f : atk;
            dmg = this.ApplyHeightDamageModifier(dmg, target);
            float final = Mathf.Max(0, dmg - def);

            return final <= 0 ? 1 : Mathf.RoundToInt(final);
        }

        private float ApplyHeightDamageModifier(float damage, Unit target) {
            int attackerHeight = this._getGridPosition().Height;
            int targetHeight = target.GridPosition.Height;
            if (attackerHeight > targetHeight) {
                damage *= 1.2f;
            }
            else if (attackerHeight < targetHeight) {
                damage *= 0.8f;
            }

            return damage;
        }
    }
}
