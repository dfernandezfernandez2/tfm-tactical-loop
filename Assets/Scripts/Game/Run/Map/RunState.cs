namespace Game.Run.Map {
    public class RunState {
        public RunGraph RunGraph { get; } = RunGraphGenerator.Generate();
    }
}
