namespace Game.Map.Run.UI.Data {
    using System.Collections.Generic;

    public class MapNode {
        public readonly List<ConnectionNode> NextNodeConnections = new();
        public readonly RunNode RunNode;
        public NodeUI Node;

        public MapNode(RunNode RunNode) => this.RunNode = RunNode;
    }
}
