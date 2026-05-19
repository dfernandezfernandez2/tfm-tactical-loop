namespace Game.Battle.IA.Evaluator {
    using System.Collections.Generic;
    using Actions;

    public interface IActionEvaluator {
        public bool CanEvaluate(IBattleAction action);

        public IEnumerable<DecisionResult> GetDecisions(ActionContext context, IBattleAction action);

        public float GetScore(ActionContext context, DecisionResult decision);
    }
}
