namespace Game.Passive {
    using System.Collections;
    using Battle.Unit;
    using Battle.Unit.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Passive/Revive")]
    public class RevivePassive : Passive {
        [SerializeField] private int amountByMap = 2;
        [SerializeField] private float percentageOfLife = 0.1f;

        private int _pendingRevives;

        public override IEnumerator OnDeadUnit(UnitObject targetUnit) {
            if (this._pendingRevives <= 0) {
                yield break;
            }

            this._pendingRevives--;
            Unit unit = targetUnit.Unit;
            float maxHp = unit.GetMaxStat(StatType.Hp);
            float heal = unit.AddStat(StatType.Hp, maxHp * this.percentageOfLife);
            yield return targetUnit.PlayRevive((int)heal);
        }

        public override void OnMapStart() => this._pendingRevives = this.amountByMap;
    }
}
