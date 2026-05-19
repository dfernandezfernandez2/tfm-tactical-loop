namespace Game.Map.Run {
    public class RunState {
        public RunGraph RunGraph { get; } = RunGraphGenerator.Generate();
    }
}
