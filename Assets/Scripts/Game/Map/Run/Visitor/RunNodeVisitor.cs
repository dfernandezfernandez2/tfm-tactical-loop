namespace Game.Map.Run.Visitor {
    using System.Collections.Generic;

    public abstract class RunNodeVisitor<TArg> : IRunNodeVisitor<TArg> {

        private readonly HashSet<RunNode> _visited = new();

        public virtual void Visit(RunNode node, TArg arg) {
            if (!this._visited.Add(node)) {
                return;
            }
            foreach (RunNode nextNode in node.NextNodes) {
                nextNode.Accept(this, arg);
            }
        }
    }
}
