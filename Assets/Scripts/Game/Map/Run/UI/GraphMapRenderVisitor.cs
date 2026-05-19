namespace Game.Map.Run.UI {
    using System;
    using System.Collections.Generic;
    using Data;
    using Visitor;

    public class GraphMapRenderVisitor : IRunNodeVisitor<(int Level, MapNode PreviousNode)> {
        private readonly HashSet<(MapNode From, MapNode To)> _createdConnections = new();
        private readonly HashSet<RunNode> _expandedNodes = new();
        private readonly Func<NodeUI, NodeUI, NodeConnectionUI> _instanceNodeConnection;
        private readonly Func<MapNode, int, NodeUI> _instantiateNode;

        private readonly Dictionary<RunNode, MapNode> _mapNodesByRunNode = new();
        private readonly Action<MapNode> _onMapNode;

        public GraphMapRenderVisitor(Func<MapNode, int, NodeUI> instantiateNode,
            Func<NodeUI, NodeUI, NodeConnectionUI> instanceNodeConnection, Action<MapNode> onMapNode) {
            this._instantiateNode = instantiateNode;
            this._instanceNodeConnection = instanceNodeConnection;
            this._onMapNode = onMapNode;
        }

        public void Visit(RunNode node, (int Level, MapNode? PreviousNode) ctx) {
            MapNode currentNode = this.GetOrCreateMapNode(node, ctx.Level);
            if (ctx.PreviousNode == null) {
                this._onMapNode(currentNode);
            }
            else {
                this.CreateConnection(ctx.PreviousNode, currentNode);
            }

            if (!this._expandedNodes.Add(node)) {
                return;
            }

            foreach (RunNode nextNode in node.NextNodes) {
                nextNode.Accept(this, (ctx.Level + 1, currentNode));
            }
        }

        private MapNode GetOrCreateMapNode(RunNode node, int level) {
            if (this._mapNodesByRunNode.TryGetValue(node, out MapNode existing)) {
                return existing;
            }

            MapNode mapNode = new(node);
            NodeUI nodeUI = this._instantiateNode(mapNode, level);
            mapNode.Node = nodeUI;
            this._mapNodesByRunNode[node] = mapNode;
            return mapNode;
        }

        private void CreateConnection(MapNode previousNode, MapNode currentNode) {
            if (!this._createdConnections.Add((previousNode, currentNode))) {
                return;
            }

            NodeConnectionUI connectionUI = this._instanceNodeConnection(previousNode.Node, currentNode.Node);
            previousNode.NextNodeConnections.Add(new ConnectionNode(connectionUI, currentNode));
        }
    }
}
