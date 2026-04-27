namespace Game.Map.Run {
    public class RunState {
        public RunState() {
            this.RunGraph = RunGraphGenerator.Generate();
            this.RunGraph.CurrentNode.Complete();
        }

        public RunGraph RunGraph { get; }
    }
}
