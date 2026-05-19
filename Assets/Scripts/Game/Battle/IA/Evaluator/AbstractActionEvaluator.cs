namespace Game.Battle.IA.Evaluator {
    using System.Collections.Generic;
    using Actions;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;
    using UnityEngine;

    public abstract class AbstractActionEvaluator<T> : IActionEvaluator where T : IBattleAction {
        protected readonly BattleMapManager BattleMapManager;

        protected AbstractActionEvaluator(BattleMapManager battleMapManager) =>
            this.BattleMapManager = battleMapManager;

        public bool CanEvaluate(IBattleAction action) => action is T;

        public IEnumerable<DecisionResult> GetDecisions(ActionContext context, IBattleAction action) =>
            this.GetDecisions(context, (T)action);

        public float GetScore(ActionContext context, DecisionResult decision) =>
            this.GetScore(context, (T)decision.Action, decision);

        protected abstract IEnumerable<DecisionResult> GetDecisions(ActionContext context, T action);

        protected abstract float GetScore(ActionContext context, T action, DecisionResult decision);

        protected static bool IsWeakTarget(UnitObject target) =>
            target.Unit.GetCurrentStat(StatType.Hp) < target.Unit.GetMaxStat(StatType.Hp) * 0.5f;

        protected static int GetDistance(GridPosition a, GridPosition b) =>
            Mathf.Abs(a.Position.x - b.Position.x) +
            Mathf.Abs(a.Position.y - b.Position.y);
    }
}
