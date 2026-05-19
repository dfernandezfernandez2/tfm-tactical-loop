namespace Game.Battle.IA {
    using System.Collections.Generic;
    using System.Linq;
    using Actions;
    using Evaluator;
    using Map;
    using Map.Data;
    using Unit;
    using Unit.Data;

    public class EnemyTurnController {
        private readonly IReadOnlyList<IActionEvaluator> _actionEvaluators;

        public EnemyTurnController(BattleMapManager battleMapManager) =>
            this._actionEvaluators = new List<IActionEvaluator> {
                new BasicAttackEvaluator(battleMapManager),
                new MovementEvaluator(battleMapManager),
                new SkillEvaluator(battleMapManager)
            };

        public IReadOnlyList<DecisionResult> CalculateTurn(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            IReadOnlyList<IBattleAction> availableActions) {
            List<DecisionResult> bestPlan = null;
            float bestScore = float.MinValue;
            foreach (List<DecisionResult> plan in this.GeneratePlans(enemy, turnOrder, availableActions.ToList())) {
                float score = this.ScorePlan(enemy, turnOrder, plan);
                if (score <= bestScore) {
                    continue;
                }

                bestScore = score;
                bestPlan = plan;
            }

            return bestPlan ?? new List<DecisionResult> { DecisionResult.Wait() };
        }

        private IEnumerable<List<DecisionResult>> GeneratePlans(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            List<IBattleAction> availableActions) {
            GridPosition startPosition = enemy.Unit.GridPosition;
            int currentAp = enemy.Unit.GetCurrentIntStat(StatType.AP);
            foreach (List<DecisionResult> plan in this.GeneratePlans(enemy, turnOrder, startPosition, currentAp,
                         availableActions)) {
                yield return plan;
            }
        }

        private IEnumerable<List<DecisionResult>> GeneratePlans(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            GridPosition currentPosition, int remainingAp, IReadOnlyList<IBattleAction> availableActions) {
            if (remainingAp <= 0) {
                yield return new List<DecisionResult> { DecisionResult.Wait() };
                yield break;
            }

            ActionContext context = new(enemy, turnOrder, currentPosition);
            List<DecisionResult> possibleActions = this.GetPossibleActions(context, availableActions)
                .Where(decision => decision.Action.GetApCost() <= remainingAp)
                .ToList();
            if (possibleActions.Count == 0) {
                yield return new List<DecisionResult> { DecisionResult.Wait() };
                yield break;
            }

            foreach (DecisionResult decision in possibleActions) {
                if (decision.Action is WaitAction) {
                    yield return new List<DecisionResult> { decision };
                    continue;
                }

                int nextRemainingAp = remainingAp - decision.Action.GetApCost();
                GridPosition nextPosition = decision.Action is MovementSelectionAction
                    ? decision.TargetPosition
                    : currentPosition;
                List<IBattleAction> nextAvailableActions = new(availableActions);
                nextAvailableActions.RemoveAll(action => action.GetType() == decision.Action.GetType());
                foreach (List<DecisionResult> nextPlan in this.GeneratePlans(enemy, turnOrder, nextPosition,
                             nextRemainingAp, nextAvailableActions)) {
                    List<DecisionResult> fullPlan = new() { decision };
                    fullPlan.AddRange(nextPlan);
                    yield return fullPlan;
                }
            }
        }

        private IEnumerable<DecisionResult> GetPossibleActions(ActionContext context,
            IReadOnlyList<IBattleAction> availableActions) {
            yield return DecisionResult.Wait();
            foreach (IBattleAction action in availableActions) {
                IActionEvaluator evaluator = this.GetEvaluator(action);
                if (evaluator == null) {
                    continue;
                }

                foreach (DecisionResult decision in evaluator.GetDecisions(context, action)) {
                    yield return decision;
                }
            }
        }

        private float ScorePlan(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            IReadOnlyList<DecisionResult> plan) {
            GridPosition currentPosition = enemy.Unit.GridPosition;
            float score = 0f;
            foreach (DecisionResult decision in plan) {
                ActionContext context = new(enemy, turnOrder, currentPosition);
                IActionEvaluator evaluator = this.GetEvaluator(decision.Action);
                if (evaluator != null) {
                    score += evaluator.GetScore(context, decision);
                }

                if (decision.Action is MovementSelectionAction) {
                    currentPosition = decision.TargetPosition;
                }
            }

            return score;
        }

        private IActionEvaluator GetEvaluator(IBattleAction action) =>
            this._actionEvaluators.FirstOrDefault(evaluator => evaluator.CanEvaluate(action));
    }
}
