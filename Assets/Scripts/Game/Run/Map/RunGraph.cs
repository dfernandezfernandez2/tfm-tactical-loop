namespace Game.Run.Map {
    using Visitor;

    public class RunGraph {
        public RunGraph(RunNode startNode) {
            this.StartNode = startNode;
            this.CurrentNode = startNode;
        }

        private RunNode StartNode { get; }
        public RunNode CurrentNode { get; set; }

        public void Accept<TContext>(IRunNodeVisitor<TContext> visitor, TContext context) =>
            this.StartNode.Accept(visitor, context);
    }
}
