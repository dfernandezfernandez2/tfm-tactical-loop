namespace Game.Battle.IA {
    using Actions;
    using Map.Data;

    public class DecisionResult {
        public DecisionResult(IBattleAction action, GridPosition targetPosition) {
            this.Action = action;
            this.TargetPosition = targetPosition;
        }

        public IBattleAction Action { get; private set; }
        public GridPosition TargetPosition { get; private set; }

        public static DecisionResult Wait() => new(new WaitAction(), null);
    }
}
