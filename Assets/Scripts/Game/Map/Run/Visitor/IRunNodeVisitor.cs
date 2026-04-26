namespace Game.Map.Run.Visitor {
    public interface IRunNodeVisitor<TArg> {
        void Visit(RunNode node, TArg context);
    }
}
