namespace Game.IA {
    using System.Collections.Generic;
    using System.Linq;
    using Battle.Actions;
    using global::Unit.Data;
    using Map.Battle;
    using Map.Battle.Data;
    using Unit;
    using UnityEngine;

    public class EnemyTurnController {
        private readonly BattleMapManager _battleMapManager;

        public EnemyTurnController(BattleMapManager battleMapManager) => this._battleMapManager = battleMapManager;

        public IReadOnlyList<DecisionResult> CalculateTurn(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            IReadOnlyList<IBattleAction> availableActions) {
            List<DecisionResult> bestPlan = null;
            float bestScore = float.MinValue;
            foreach (List<DecisionResult> plan in this.GeneratePlans(enemy, turnOrder, availableActions.ToList())) {
                float score = ScorePlan(enemy, turnOrder, plan);
                if (!(score > bestScore)) {
                    continue;
                }

                bestScore = score;
                bestPlan = plan;
            }

            return bestPlan ?? new List<DecisionResult> { DecisionResult.Wait() };
        }

        private IEnumerable<List<DecisionResult>> GeneratePlans(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            List<IBattleAction> availableActions) {
            GridPosition startPosition = enemy.GetUnit().GetGridPosition();
            int currentAp = enemy.GetUnit().GetCurrentAp();

            foreach (List<DecisionResult> plan in this.GeneratePlans(enemy, turnOrder, startPosition, currentAp,
                         availableActions)) {
                yield return plan;
            }
        }

        private IEnumerable<List<DecisionResult>> GeneratePlans(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            GridPosition currentPosition, int remainingAp, IReadOnlyList<IBattleAction> availableActions) {
            if (remainingAp <= 0) {
                yield return new List<DecisionResult>();
                yield break;
            }

            List<DecisionResult> possibleActions = this
                .GetPossibleActions(enemy, turnOrder, currentPosition, availableActions)
                .Where(x => x.Action.GetApCost() <= remainingAp)
                .ToList();

            if (possibleActions.Count == 0) {
                yield return new List<DecisionResult> { DecisionResult.Wait() };
                yield break;
            }

            foreach (DecisionResult action in possibleActions) {
                if (action.Action is WaitAction) {
                    yield return new List<DecisionResult> { action };
                    continue;
                }

                int nextRemainingAp = remainingAp - action.Action.GetApCost();
                GridPosition nextPosition = action.TargetPosition ?? currentPosition;

                List<IBattleAction> nextAvailableActions = new(availableActions);
                nextAvailableActions.RemoveAll(x => x.GetType() == action.Action.GetType());

                bool hasChildren = false;
                IEnumerable<List<DecisionResult>> plans = this.GeneratePlans(enemy, turnOrder, nextPosition,
                    nextRemainingAp, nextAvailableActions);
                foreach (List<DecisionResult> plan in plans) {
                    hasChildren = true;
                    List<DecisionResult> fullPlan = new() { action };
                    fullPlan.AddRange(plan);
                    yield return fullPlan;
                }

                if (!hasChildren) {
                    yield return new List<DecisionResult> { action };
                }
            }
        }

        private static float ScorePlan(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            IReadOnlyList<DecisionResult> plan) {
            GridPosition currentPosition = enemy.GetUnit().GetGridPosition();
            float score = 0f;

            UnitObject closestBefore = GetClosestEnemy(enemy, turnOrder, currentPosition);
            int distanceBefore = closestBefore == null
                ? 0
                : GetDistance(currentPosition, closestBefore.GetUnit().GetGridPosition());

            foreach (DecisionResult decision in plan) {
                switch (decision.Action) {
                    case AttackAction:
                        UnitObject target = GetUnitAtPosition(turnOrder, decision.TargetPosition);
                        if (target == null) {
                            break;
                        }

                        int damage = EstimateDamage(enemy, target);
                        int hp = target.GetUnit().GetCurrentHp();
                        score += damage * 10f;
                        score += 20f;
                        if (damage >= hp) {
                            score += 100f;
                        }

                        if (IsWeakTarget(target)) {
                            score += 15f;
                        }

                        break;

                    case MovementAction:
                        currentPosition = decision.TargetPosition;
                        break;

                    case WaitAction:
                        break;
                }
            }

            UnitObject closestAfter = GetClosestEnemy(enemy, turnOrder, currentPosition);
            int distanceAfter = closestAfter == null
                ? 0
                : GetDistance(currentPosition, closestAfter.GetUnit().GetGridPosition());
            score += (distanceBefore - distanceAfter) * 5f;
            bool attacked = plan.Any(x => x.Action is AttackAction);
            if (!attacked && closestAfter != null) {
                score -= distanceAfter * 2f;
            }

            return score;
        }

        private List<DecisionResult> GetPossibleActions(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            GridPosition currentPosition, IReadOnlyList<IBattleAction> availableActions) {
            List<DecisionResult> actions = new() { DecisionResult.Wait() };

            foreach (IBattleAction action in availableActions) {
                switch (action) {
                    case AttackAction attackAction:
                        UnitObject killTarget = GetKillableTarget(enemy, turnOrder, currentPosition);

                        if (killTarget != null) {
                            actions.Add(new DecisionResult(attackAction, killTarget.GetUnit().GetGridPosition()));
                        }

                        actions.AddRange(from target in GetAttackableTargets(enemy, turnOrder, currentPosition)
                            where killTarget == null || target != killTarget
                            select new DecisionResult(attackAction, target.GetUnit().GetGridPosition()));
                        break;

                    case MovementAction movementAction:
                        actions.AddRange(this.GetCandidateMovementPositions(enemy, turnOrder, currentPosition)
                            .Select(moveTarget => new DecisionResult(movementAction, moveTarget)));
                        break;
                }
            }

            return actions;
        }

        private static IEnumerable<UnitObject> GetAttackableTargets(UnitObject enemy,
            IReadOnlyList<UnitObject> turnOrder, GridPosition currentPosition) {
            int range = enemy.GetUnit().GetAttackRange();
            return turnOrder
                .Where(unit => unit != enemy)
                .Where(unit => unit.GetTeam().GetBattleTeam() != enemy.GetTeam().GetBattleTeam())
                .Where(unit => !unit.GetUnit().IsDead())
                .Where(unit => GetDistance(currentPosition, unit.GetUnit().GetGridPosition()) <= range)
                .OrderBy(unit => unit.GetUnit().GetCurrentHp());
        }

        private static UnitObject
            GetKillableTarget(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder, GridPosition currentPosition) =>
            (from target in GetAttackableTargets(enemy, turnOrder, currentPosition)
                let expectedDamage = EstimateDamage(enemy, target)
                where expectedDamage >= target.GetUnit().GetCurrentHp()
                select target).FirstOrDefault();

        private IEnumerable<GridPosition> GetCandidateMovementPositions(UnitObject enemy,
            IReadOnlyList<UnitObject> turnOrder, GridPosition currentPosition) {
            int movement = enemy.GetUnit().GetCurrentMovement();
            IReadOnlyList<TileData> reachableTiles =
                this._battleMapManager.GetReachableTiles(currentPosition, movement);

            UnitObject closestTarget = GetClosestEnemy(enemy, turnOrder, currentPosition);
            if (closestTarget == null) {
                return reachableTiles
                    .Select(x => x.TileGridPosition)
                    .Where(x => !x.Equals(currentPosition))
                    .Take(6);
            }

            return reachableTiles
                .Select(x => x.TileGridPosition)
                .Where(x => !x.Equals(currentPosition))
                .OrderBy(x => GetDistance(x, closestTarget.GetUnit().GetGridPosition()))
                .Take(6);
        }

        private static UnitObject GetClosestEnemy(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder,
            GridPosition currentPosition) =>
            turnOrder
                .Where(unit => unit != enemy)
                .Where(unit => unit.GetTeam().GetBattleTeam() != enemy.GetTeam().GetBattleTeam())
                .Where(unit => !unit.GetUnit().IsDead())
                .OrderBy(unit => GetDistance(currentPosition, unit.GetUnit().GetGridPosition()))
                .FirstOrDefault();

        private static UnitObject GetUnitAtPosition(IReadOnlyList<UnitObject> turnOrder, GridPosition position) =>
            turnOrder.FirstOrDefault(x =>
                !x.GetUnit().IsDead() &&
                x.GetUnit().GetGridPosition().Equals(position));

        private static bool IsWeakTarget(UnitObject target) =>
            target.GetUnit().GetCurrentHp() < target.GetUnit().GetMaxHp() * 0.5f;

        private static int EstimateDamage(UnitObject attacker, UnitObject defender) {
            List<KeyValuePair<StatType, float>> attackerStats =
                attacker.GetUnit().GetCurrentStats(
                    StatType.Atk,
                    StatType.Accuracy,
                    StatType.CritChance);

            List<KeyValuePair<StatType, float>> defenderStats =
                defender.GetUnit().GetCurrentStats(
                    StatType.Def,
                    StatType.Evasion);

            float atk = attackerStats.First(x => x.Key == StatType.Atk).Value;
            float accuracy = attackerStats.First(x => x.Key == StatType.Accuracy).Value;
            float critChance = attackerStats.First(x => x.Key == StatType.CritChance).Value;

            float def = defenderStats.First(x => x.Key == StatType.Def).Value;
            float evasion = defenderStats.First(x => x.Key == StatType.Evasion).Value;

            float hitChance = accuracy / (accuracy + evasion);
            hitChance = Mathf.Clamp(hitChance, 0.1f, 0.95f);

            float normalDamage = Mathf.Max(1f, atk - def);
            float critDamage = Mathf.Max(1f, (atk * 1.5f) - def);

            float expectedDamage = hitChance * (((1f - critChance) * normalDamage) + (critChance * critDamage));
            return Mathf.Max(1, Mathf.RoundToInt(expectedDamage));
        }

        private static int GetDistance(GridPosition a, GridPosition b) =>
            Mathf.Abs(a.Position.x - b.Position.x) +
            Mathf.Abs(a.Position.y - b.Position.y);
    }
}
