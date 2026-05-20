namespace Game.Run.Map.Visitor {
    public interface IRunNodeVisitor<in TArg> {
        public void Visit(RunNode node, TArg context);
    }
}
