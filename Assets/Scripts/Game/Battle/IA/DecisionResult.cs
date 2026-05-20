namespace Game.Battle.IA {
    using Actions;
    using Map.Data;

    public class DecisionResult {
        public DecisionResult(IBattleAction action, GridPosition targetPosition) {
            this.Action = action;
            this.TargetPosition = targetPosition;
        }

        public IBattleAction Action { get; }
        public GridPosition TargetPosition { get; }

        public static DecisionResult Wait() => new(new WaitAction(), null);
    }
}
