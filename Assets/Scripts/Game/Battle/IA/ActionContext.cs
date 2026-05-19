namespace Game.Battle.IA {
    using System.Collections.Generic;
    using Map.Data;
    using Unit;

    public class ActionContext {
        public ActionContext(UnitObject enemy, IReadOnlyList<UnitObject> turnOrder, GridPosition currentPosition) {
            this.Enemy = enemy;
            this.TurnOrder = turnOrder;
            this.CurrentPosition = currentPosition;
        }

        public UnitObject Enemy { get; }
        public IReadOnlyList<UnitObject> TurnOrder { get; }
        public GridPosition CurrentPosition { get; }
    }
}
