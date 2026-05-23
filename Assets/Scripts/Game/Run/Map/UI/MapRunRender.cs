namespace Game.Run.Map.UI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Controls;
    using Data;
    using UnityEngine;
    using UnityEngine.UI;

    public class MapRunRender : MonoBehaviour {
        [SerializeField] private GameObject canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform contentPrefab;
        [SerializeField] private RectTransform connectionsObjectGroup;
        [SerializeField] private GameObject rowLevelNodesPrefab;
        [SerializeField] private NodeUI nodePrefab;
        [SerializeField] private NodeConnectionUI nodeConnectionPrefab;

        private readonly List<NodeConnectionUI> _nodeConnections = new();
        private readonly Dictionary<int, Transform> _rowsByLevel = new();

        private RectTransform _connectionsObjectGroupTransform = null!;
        private RectTransform _contentRectTransform = null!;
        private MapNode _currentKeyboardSelectedNode;
        private MapNode? _currentNode;
        private RunGraph _graph;
        private bool _isActive;

        private void Awake() {
            this._contentRectTransform = Instantiate(this.contentPrefab, this.canvas.transform);
            this._connectionsObjectGroupTransform =
                Instantiate(this.connectionsObjectGroup, this._contentRectTransform);
            this.canvas.gameObject.SetActive(false);
        }

        private void Update() {
            if (!this._isActive) {
                return;
            }

            this.HandleKeyBoard();
        }

        public void InitMap(RunGraph graph) {
            this._graph = graph;
            this._rowsByLevel.Clear();
            this._nodeConnections.Clear();
            GraphMapRenderVisitor visitor = new(this.InstantiateNode, this.InstantiateNodeConnection,
                node => this._currentNode ??= node);
            graph.Accept(visitor, (0, null));
        }

        public void ShowMap() {
            if (this._currentNode == null) {
                return;
            }

            this.StartCoroutine(this.ShowMapWhenLayoutIsReady());
        }

        private IEnumerator ShowMapWhenLayoutIsReady() {
            this.canvas.gameObject.SetActive(true);
            this.canvasGroup.alpha = 0f;

            this._currentNode.Node.Enable();
            this._currentNode.Node.Select();

            foreach (ConnectionNode connection in this._currentNode.NextNodeConnections) {
                connection.MapNode.Node.Enable();
                connection.NodeConnection.Select();
            }

            yield return null;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(this._contentRectTransform);

            yield return null;

            foreach (NodeConnectionUI connection in this._nodeConnections) {
                connection.Refresh();
            }

            this.canvasGroup.alpha = 1f;

            this._currentKeyboardSelectedNode = this._currentNode;
            this._isActive = true;
        }

        public bool HasNext() =>
            !(this._currentNode == null || this._currentNode.NextNodeConnections.Exists(node => node.MapNode.RunNode.EncounterType == EncounterType.End));

        private void Hide() {
            this._isActive = false;
            this.canvas.gameObject.SetActive(false);
        }

        private void HandleKeyBoard() {
            if (InputUtils.IsRightSelected()) {
                if (this._currentKeyboardSelectedNode == this._currentNode) {
                    this._currentKeyboardSelectedNode = this._currentNode.NextNodeConnections[0].MapNode;
                    this._currentKeyboardSelectedNode.Node.Select();
                }
            }

            if (InputUtils.IsLeftSelected()) {
                if (this._currentKeyboardSelectedNode != this._currentNode) {
                    this._currentKeyboardSelectedNode.Node.UnSelect();
                    this._currentKeyboardSelectedNode = this._currentNode;
                }
            }

            if (InputUtils.IsUpSelected()) {
                if (this._currentKeyboardSelectedNode == this._currentNode) {
                    MapNode nextSelected = this._currentNode.NextNodeConnections[0].MapNode;
                    nextSelected.Node.Select();
                    this._currentKeyboardSelectedNode = nextSelected;
                }
                else {
                    int currenSelectedPosition =
                        this._currentNode.NextNodeConnections.FindIndex(m =>
                            m.MapNode == this._currentKeyboardSelectedNode);
                    int backPosition = currenSelectedPosition == 0
                        ? this._currentNode.NextNodeConnections.Count - 1
                        : currenSelectedPosition - 1;
                    this._currentKeyboardSelectedNode.Node.UnSelect();
                    this._currentKeyboardSelectedNode = this._currentNode.NextNodeConnections[backPosition].MapNode;
                    this._currentKeyboardSelectedNode.Node.Select();
                }
            }

            if (InputUtils.IsDownSelected()) {
                if (this._currentKeyboardSelectedNode == this._currentNode) {
                    MapNode nextSelected = this._currentNode.NextNodeConnections[^1].MapNode;
                    nextSelected.Node.Select();
                    this._currentKeyboardSelectedNode = nextSelected;
                }
                else {
                    int currenSelectedPosition =
                        this._currentNode.NextNodeConnections.FindIndex(m =>
                            m.MapNode == this._currentKeyboardSelectedNode);
                    int nextPosition = currenSelectedPosition == this._currentNode.NextNodeConnections.Count - 1
                        ? 0
                        : currenSelectedPosition + 1;
                    this._currentKeyboardSelectedNode.Node.UnSelect();
                    this._currentKeyboardSelectedNode = this._currentNode.NextNodeConnections[nextPosition].MapNode;
                    this._currentKeyboardSelectedNode.Node.Select();
                }
            }

            if (InputUtils.IsEnterSelected()) {
                if (this._currentKeyboardSelectedNode != this._currentNode) {
                    this.OnNodeSelected(this._currentKeyboardSelectedNode);
                }
            }
        }

        private NodeUI InstantiateNode(MapNode node, int level) {
            Transform row = this.GetOrCreateRow(level);
            NodeUI nodeUI = Instantiate(this.nodePrefab, row);
            nodeUI.Init(node);
            nodeUI.OnClick += this.OnNodeSelected;
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

        private void OnNodeSelected(MapNode node) {
            if (this._currentNode != null) {
                foreach (ConnectionNode currentNodeNextNodeConnection in this._currentNode.NextNodeConnections) {
                    currentNodeNextNodeConnection.NodeConnection.UnSelect();
                    currentNodeNextNodeConnection.MapNode.Node.Disable();
                }

                this._currentNode.Node.UnSelect();
                this._currentNode.Node.Disable();
            }

            this._currentNode = node;
            this._graph.CurrentNode = node.RunNode;
            this.Hide();
            this.OnSelect?.Invoke(node.RunNode);
        }

        public event Action<RunNode> OnSelect;
    }
}
