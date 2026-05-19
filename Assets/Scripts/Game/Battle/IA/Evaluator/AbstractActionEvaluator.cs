namespace Game.Battle.IA.Evaluator {
    using System.Collections.Generic;
    using Actions;
    using Map;

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
    }
}
