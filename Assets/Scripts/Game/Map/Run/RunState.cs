namespace Game.Map.Run {
    public class RunState {

        public RunGraph RunGraph {get; private set;}

        public RunState() {
            this.RunGraph = RunGraphGenerator.Generate();
            this.RunGraph.CurrentNode.Complete();
        }
    }
}
