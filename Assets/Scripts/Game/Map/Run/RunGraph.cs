namespace Game.Map.Run {
    using Visitor;

    public class RunGraph {
        public RunNode StartNode { get; set; }
        public RunNode CurrentNode { get; set; }

        public RunGraph(RunNode startNode) {
            this.StartNode = startNode;
            this.CurrentNode = startNode;
        }

        public void Accept<TContext>(IRunNodeVisitor<TContext> visitor, TContext context) {
            this.StartNode.Accept(visitor, context);
        }

    }
}
