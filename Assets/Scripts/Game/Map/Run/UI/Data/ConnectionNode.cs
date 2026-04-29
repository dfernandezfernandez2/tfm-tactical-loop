namespace Game.Map.Run.UI.Data {
    public class ConnectionNode {
        public readonly MapNode MapNode;
        public readonly NodeConnectionUI NodeConnection;

        public ConnectionNode(NodeConnectionUI nodeConnection, MapNode mapNode) {
            this.NodeConnection = nodeConnection;
            this.MapNode = mapNode;
        }
    }
}
