#nullable enable
namespace Game.Map.Run.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Visitor;

    public class MapRunRender : MonoBehaviour {
        [SerializeField] private GameObject canvas;
        [SerializeField] private RectTransform contentPrefab;
        [SerializeField] private RectTransform connectionsObjectGroup;
        [SerializeField] private GameObject rowLevelNodesPrefab;
        [SerializeField] private NodeUI nodePrefab;
        [SerializeField] private NodeConnectionUI nodeConnectionPrefab;

        private readonly List<NodeConnectionUI> _nodeConnections = new();
        private readonly Dictionary<int, Transform> _rowsByLevel = new();

        private RectTransform _connectionsObjectGroupTransform = null!;
        private RectTransform _contentRectTransform = null!;
        private MapNode? _currentNode;

        private void Awake() {
            this._contentRectTransform = Instantiate(this.contentPrefab, this.canvas.transform);
            this._connectionsObjectGroupTransform = Instantiate(this.connectionsObjectGroup, this.canvas.transform);
            this._connectionsObjectGroupTransform.SetAsFirstSibling();
            this._contentRectTransform.gameObject.SetActive(false);
            this._connectionsObjectGroupTransform.gameObject.SetActive(false);
        }

        private void Start() => this.InitMap();

        public void InitMap(RunGraph? graph = null) {
            graph ??= RunGraphGenerator.Generate();
            this._rowsByLevel.Clear();
            this._nodeConnections.Clear();
            GraphMapRenderVisitor visitor = new(this.InstantiateNode, this.InstantiateNodeConnection,
                node => this._currentNode ??= node);
            graph.Accept(visitor, (0, (MapNode?)null));
        }

        public void Show() => this.ShowMap();

        public bool ShowMap() {
            if (this._currentNode == null) {
                return false;
            }

            this._contentRectTransform.gameObject.SetActive(true);
            this._connectionsObjectGroupTransform.gameObject.SetActive(true);
            this.StartCoroutine(this.RefreshConnectionsNextFrame());
            this._currentNode.Node.Select();
            foreach (ConnectionNode connection in this._currentNode.NextNodeConnections) {
                connection.MapNode.Node.Enable();
                connection.NodeConnection.Select();
            }

            return true;
        }

        private IEnumerator RefreshConnectionsNextFrame() {
            yield return null;
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(this._contentRectTransform);
            yield return null;
            foreach (NodeConnectionUI connection in this._nodeConnections) {
                connection.Refresh();
            }
        }

        private NodeUI InstantiateNode(RunNode node, int level) {
            Transform row = this.GetOrCreateRow(level);
            NodeUI nodeUI = Instantiate(this.nodePrefab, row);
            nodeUI.Init(node);
            return nodeUI;
        }

        private Transform GetOrCreateRow(int level) {
            if (this._rowsByLevel.TryGetValue(level, out Transform row)) {
                return row;
            }

            GameObject rowObject = Instantiate(this.rowLevelNodesPrefab, this._contentRectTransform);
            row = rowObject.transform;
            this._rowsByLevel[level] = row;
            return row;
        }

        private NodeConnectionUI InstantiateNodeConnection(NodeUI previous, NodeUI next) {
            NodeConnectionUI connection = Instantiate(this.nodeConnectionPrefab, this._connectionsObjectGroupTransform);
            connection.Init(previous.GetComponent<RectTransform>(), next.GetComponent<RectTransform>(),
                this._connectionsObjectGroupTransform);
            this._nodeConnections.Add(connection);
            return connection;
        }

        private class GraphMapRenderVisitor : IRunNodeVisitor<(int Level, MapNode? PreviousNode)> {
            private readonly HashSet<(MapNode From, MapNode To)> _createdConnections = new();
            private readonly HashSet<RunNode> _expandedNodes = new();
            private readonly Func<NodeUI, NodeUI, NodeConnectionUI> _instanceNodeConnection;
            private readonly Func<RunNode, int, NodeUI> _instantiateNode;

            private readonly Dictionary<RunNode, MapNode> _mapNodesByRunNode = new();
            private readonly Action<MapNode> _onMapNode;

            public GraphMapRenderVisitor(Func<RunNode, int, NodeUI> instantiateNode,
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

                NodeUI nodeUI = this._instantiateNode(node, level);
                MapNode mapNode = new(nodeUI);
                this._mapNodesByRunNode[node] = mapNode;
                return mapNode;
            }

            private void CreateConnection(MapNode previousNode, MapNode currentNode) {
                if (!this._createdConnections.Add((previousNode, currentNode))) {
                    return;
                }

                NodeConnectionUI connectionUI = this._instanceNodeConnection(previousNode.Node, currentNode.Node);
                previousNode.NextNodeConnections.Add(new ConnectionNode(connectionUI, currentNode));
                currentNode.PreviousNodeConnections.Add(new ConnectionNode(connectionUI, previousNode));
            }
        }

        private class MapNode {
            public readonly List<ConnectionNode> NextNodeConnections = new();
            public readonly NodeUI Node;
            public readonly List<ConnectionNode> PreviousNodeConnections = new();

            public MapNode(NodeUI node) => this.Node = node;
        }

        private class ConnectionNode {
            public readonly MapNode MapNode;
            public readonly NodeConnectionUI NodeConnection;

            public ConnectionNode(NodeConnectionUI nodeConnection, MapNode mapNode) {
                this.NodeConnection = nodeConnection;
                this.MapNode = mapNode;
            }
        }
    }
}
