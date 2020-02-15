using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// The container window for the Benjamin Bergseth Behaviour Tree editor.
    /// (I might want to work on that name.)
    /// </summary>
    public class BbbtWindow : EditorWindow
    {
        /// <summary>
        /// Used to tell the editor to load a tree.
        /// Used to avoid loading a new tree in the middle of processing events.
        /// </summary>
        public BbbtBehaviourTree TreeToLoad = null;

        /// <summary>
        /// Tabs currently loaded into the BbbtWindow.
        /// </summary>
        private List<BbbtWindowTab> _tabs = null;

        /// <summary>
        /// The currently selected tab.
        /// </summary>
        private BbbtWindowTab _currentTab = null;

        /// <summary>
        /// The texture for the editor window's background.
        /// </summary>
        private static Texture2D _background;

        /// <summary>
        /// A node that was just selected and has yet to be display in the inspector.
        /// </summary>
        private BbbtNode _nodeToOpenInInspector;

        /// <summary>
        /// The preview of the connection currently being created.
        /// </summary>
        private BbbtConnectionPreview _connectionPreview;

        /// <summary>
        /// The GUIStyle used for drawing nodes.
        /// </summary>
        private GUIStyle _nodeStyle;

        /// <summary>
        /// The GUIStyle used for drawing selected nodes.
        /// </summary>
        private GUIStyle _selectedNodeStyle;

        /// <summary>
        /// The style of connection points leading in to a node.
        /// </summary>
        private GUIStyle _inPointStyle;

        /// <summary>
        /// The style of connection points leading out from a node.
        /// </summary>
        private GUIStyle _outPointStyle;

        /// <summary>
        /// The GUIStyle of the top bar.
        /// </summary>
        private GUIStyle _topBarStyle;

        /// <summary>
        /// The style of the tabs in the top bar.
        /// </summary>
        private GUIStyle _tabStyle;

        /// <summary>
        /// The ingoing selection point that has been selected.
        /// </summary>
        private BbbtConnectionPoint _selectedInPoint;

        /// <summary>
        /// The outgoing selection point that has been selected.
        /// </summary>
        private BbbtConnectionPoint _selectedOutPoint;

        /// <summary>
        /// The amount by which to drag the canvas on a given frame.
        /// </summary>
        private Vector3 _drag = Vector3.zero;

        /// <summary>
        /// The amount by which to offset the lines when drawing the grid.
        /// </summary>
        private Vector3 _gridOffset = Vector3.zero;

        /// <summary>
        /// The rect of the top bar.
        /// </summary>
        private Rect _topBarRect;


        /// <summary>
        /// Opens a bbBT window.
        /// </summary>
        [MenuItem("Window/bbBT")]
        public static void OpenWindow()
        {
            var window = GetWindow<BbbtWindow>();
            window.titleContent = new GUIContent("bbBT");
            GenerateBackground();
        }

        /// <summary>
        /// Called when the editor window is enabled.
        /// </summary>
        private void OnEnable()
        {
            // Set up the style of the behaviour tree nodes.
            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/node0.png"
            ) as Texture2D;
            _nodeStyle.normal.textColor = Color.white;
            _nodeStyle.alignment = TextAnchor.MiddleCenter;
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);

            // Set up the style of the behaviour tree nodes when selected.
            _selectedNodeStyle = new GUIStyle();
            _selectedNodeStyle.normal.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/node0 on.png"
            ) as Texture2D;
            _selectedNodeStyle.normal.textColor = Color.white;
            _selectedNodeStyle.alignment = TextAnchor.MiddleCenter;
            _selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

            // Set up the style of in connection points.
            _inPointStyle = new GUIStyle();
            _inPointStyle.normal.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/btn.png"
            ) as Texture2D;
            _inPointStyle.active.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/btn on.png"
            ) as Texture2D;
            _inPointStyle.border = new RectOffset(4, 4, 4, 4);

            // Set up the style of out connection points.
            _outPointStyle = new GUIStyle();
            _outPointStyle.normal.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/btn.png"
            ) as Texture2D;
            _outPointStyle.active.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/btn on.png"
            ) as Texture2D;
            _outPointStyle.border = new RectOffset(4, 4, 4, 4);

            // Set up the style of the top bar.
            _topBarStyle = new GUIStyle();
            _topBarStyle.normal.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/pre toolbar.png"
            ) as Texture2D;
            _topBarStyle.border = new RectOffset(4, 4, 4, 4);

            _topBarRect = new Rect(Vector2.zero, new Vector2(position.width, 20.0f));

            // Set up the style of the tabs in the top bar.
            _tabStyle = new GUIStyle();
            _tabStyle.normal.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/scrubber button.png"
            ) as Texture2D;
            _tabStyle.active.background = EditorGUIUtility.Load(
                "builtin skins/darkskin/images/.png"
            ) as Texture2D;
            _tabStyle.normal.textColor = Color.white;
            _topBarStyle.border = new RectOffset(0, 0, 0, 0);

            // Instantiate the list of nodes and connections.
            _tabs = new List<BbbtWindowTab>();
        }

        /// <summary>
        /// Called more or less when something needs to change in the editor.
        /// </summary>
        private void OnGUI()
        {
            // Check if we need to open a node editor.
            if (_nodeToOpenInInspector != null && Selection.activeObject as BbbtNode != _nodeToOpenInInspector)
            {
                // Open node editor.
                Selection.activeObject = _nodeToOpenInInspector;
                var editor = (BbbtNodeEditor)Editor.CreateEditor(_nodeToOpenInInspector);
                editor.OnInspectorGUI();
                _nodeToOpenInInspector = null;
            }

            DrawBackground();

            DrawGrid(20, 0.15f, Color.grey);
            DrawGrid(100, 0.3f, Color.grey);

            DrawTopBar();
            DrawTabs();

            DrawNodes();
            DrawConnections();

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed)
            {
                Repaint();
            }

            if (TreeToLoad != null)
            {
                LoadTree(TreeToLoad);
            }
        }

        /// <summary>
        /// Generates the background texture for the editor window.
        /// </summary>
        private static void GenerateBackground()
        {
            _background = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            _background.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f, 1.0f));
            _background.Apply();
        }

        /// <summary>
        /// Draws the background of the editor window.
        /// </summary>
        private void DrawBackground()
        {
            // Make the background of the behaviour tree editor dark and good-looking.
            if (_background == null)
            {
                GenerateBackground();
            }
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), _background, ScaleMode.StretchToFill);
        }

        /// <summary>
        /// Draws a grid across the whole editor window.
        /// </summary>
        /// <param name="lineDistance">The distance between each line in the grid.</param>
        /// <param name="opacity">The opacity of the lines in the grid.</param>
        /// <param name="lineColor">The colour of the lines in the grid.</param>
        private void DrawGrid(float lineDistance, float opacity, Color lineColor)
        {
            Handles.BeginGUI();
            Handles.color = new Color(lineColor.r, lineColor.g, lineColor.b, opacity);

            _gridOffset += _drag * 0.5f;
            var offset = new Vector3(_gridOffset.x % lineDistance, _gridOffset.y % lineDistance, 0.0f);

            int horizontalLines = Mathf.CeilToInt((position.height - offset.y) / lineDistance);
            int verticalLines = Mathf.CeilToInt((position.width - offset.x) / lineDistance);

            // Draw horizontal lines.
            for (int i = 0; i < horizontalLines; i++)
            {
                Handles.DrawLine(
                    new Vector3(-offset.x, i * lineDistance, 0) + offset,
                    new Vector3(position.width -offset.x, i * lineDistance, 0) + offset
                );
            }

            // Draw vertical lines.
            for (int i = 0; i < verticalLines; i++)
            {
                Handles.DrawLine(
                    new Vector3(i * lineDistance, -offset.y, 0) + offset,
                    new Vector3(i * lineDistance, position.height - offset.y, 0) + offset
                );
            }

            Handles.EndGUI();
        }

        /// <summary>
        /// Draws the top bar of the editor window.
        /// </summary>
        private void DrawTopBar()
        {
            GUI.Box(_topBarRect, "", _topBarStyle);
        }

        /// <summary>
        /// Draws the tabs in the top bar.
        /// </summary>
        private void DrawTabs()
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                _tabs[i].Draw(_tabs, i);
            }
        }

        /// <summary>
        /// Draws all nodes in the window.
        /// </summary>
        private void DrawNodes()
        {
            if (_currentTab != null)
            {
                _currentTab.Nodes.ForEach((node) => node.Draw());
            }
        }

        /// <summary>
        /// Draws all connections in the behaviour tree.
        /// </summary>
        private void DrawConnections()
        {
            if (_currentTab != null)
            {
                // We use a regular for loop in case a connection gets removed.
                for (int i = 0; i < _currentTab.Connections.Count; i++)
                {
                    _currentTab.Connections[i].Draw();
                }
            }

            _connectionPreview?.Draw();
        }

        /// <summary>
        /// Processes events.
        /// </summary>
        /// <param name="e">The events to be handled.</param>
        private void ProcessEvents(Event e)
        {
            // Don't let the user do anything without a loaded tab/tree.
            if (_currentTab == null) return;

            _drag = Vector3.zero;

            switch (e.type)
            {
                // Mouse button was just pressed.
                case EventType.MouseDown:
                    // LMB pressed.
                    if (e.button == 0)
                    {
                        // Clear connection selection
                        ClearConnectionSelection();
                    }
                    // RMB pressed.
                    if (e.button == 1)
                    {
                        CreateContextMenu(e.mousePosition);
                    }
                    break;
                // Mouse moved.
                case EventType.MouseDrag:
                    // MMB pressed
                    if (e.button == 2)
                    {
                        // Drag the entire window.
                        _drag = e.delta;
                        _currentTab.WindowOffset += _drag;
                        _currentTab.Nodes?.ForEach((node) => { node.Drag(e.delta); });
                        GUI.changed = true;
                        SetUnsavedChangesTabTitle();
                    }
                    break;
                // Started pressing a key.
                case EventType.KeyDown:
                    // Pressed Ctrl-S down.
                    if (e.control && e.keyCode == KeyCode.S)
                    {
                        SaveTree();
                    }
                    // Pressed the delete key
                    if (e.keyCode == KeyCode.Delete)
                    {
                        // Delete the selected node if there is one.
                        var selectedNode = FindSelectedNode();
                        if (selectedNode != null)
                        {
                            RemoveNode(selectedNode);
                            GUI.changed = true;
                        }
                    }
                    // Pressed 0
                    if (e.keyCode == KeyCode.Alpha0)
                    {
                        _gridOffset = Vector3.zero;
                    }
                    break;
                // Something dragged into the editor window.
                // i.e. a behaviour tree dragged in to be attached to a leaf node.
                case EventType.DragUpdated:
                    // Check if we're dragging a valid object into the editor
                    // and update visuals to reflect the fact that it's valid if it is.
                    var draggedTree = DragAndDrop.objectReferences[0] as BbbtBehaviourTree;

                    if (draggedTree != null)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                    }
                    break;
            }
        }

        /// <summary>
        /// Processes node events.
        /// </summary>
        /// <param name="e">The events to be handled.</param>
        private void ProcessNodeEvents(Event e)
        {
            if (_currentTab == null) return;

            // The node that was used in some way during the processing of node events.
            BbbtNode usedNode = null;

            // Iterate in the reverse of our draw order because we want to give nodes in the foreground priority.
            for (int i = _currentTab.Nodes.Count - 1; i >= 0; i--)
            {
                if (_currentTab.Nodes[i].ProcessEvents(e))
                {
                    // This node either moved or was clicked on.
                    GUI.changed = true;
                    usedNode = _currentTab.Nodes[i];
                }
            }

            // Check if usedNode isn't null, i.e. a node was interacted with.
            if (usedNode != null)
            {
                // Put the node that was interacted with on top.
                _currentTab.Nodes.Remove(usedNode);
                _currentTab.Nodes.Add(usedNode);

                // Select the node.
                _nodeToOpenInInspector = usedNode;

                SetUnsavedChangesTabTitle();
            }
        }

        /// <summary>
        /// Creates a context menu.
        /// </summary>
        /// <param name="position">The positioni of the context menu.</param>
        private void CreateContextMenu(Vector2 position)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Add Root"), false, () => AddNode(BbbtNodeType.Root, position));
            menu.AddItem(new GUIContent("Add Selector"), false, () => AddNode(BbbtNodeType.Selector, position));
            menu.AddItem(new GUIContent("Add Sequence"), false, () => AddNode(BbbtNodeType.Sequence, position));
            menu.AddItem(new GUIContent("Add Repeater"), false, () => AddNode(BbbtNodeType.Repeater, position));
            menu.AddItem(new GUIContent("Add Leaf"), false, () => AddNode(BbbtNodeType.Leaf, position));
            menu.ShowAsContext();
        }

        /// <summary>
        /// Adds a new node to the behaviour tree.
        /// </summary>
        /// <param name="type">The type of the node to create.</param>
        /// <param name="position">The position of the node.</param>
        /// <param name="actionLabel">The label used to identify the action attached to the node.</param>
        /// <param name="isSelected">Whether the node should be selected.</param>
        private void AddNode(BbbtNodeType type, Vector2 position, string actionLabel = "", bool isSelected = false)
        {
            var node = CreateInstance<BbbtNode>();
            _currentTab.Nodes.Add(node);
            node.Setup(
                ++_currentTab.LastNodeID,
                type,
                position,
                96,
                96,
                _nodeStyle,
                _selectedNodeStyle,
                _inPointStyle,
                _outPointStyle,
                OnClickInPoint,
                OnClickOutPoint,
                RemoveNode,
                actionLabel,
                isSelected
            );

            SetUnsavedChangesTabTitle();
        }

        /// <summary>
        /// Adds a new node to the behaviour tree.
        /// </summary>
        /// <param name="id">The id of the node.</param>
        /// <param name="type">The type of the node to create.</param>
        /// <param name="position">The position of the node.</param>
        /// <param name="actionLabel">The label used to identify the action attached to the node.</param>
        /// <param name="isSelected">Whether the node should be selected.</param>
        private void AddNode(
            int id,
            BbbtNodeType type,
            Vector2 position,
            string actionLabel = "",
            bool isSelected = false)
        {
            var node = CreateInstance<BbbtNode>();
            _currentTab.Nodes.Add(node);
            node.Setup(
                id,
                type,
                position,
                96,
                96,
                _nodeStyle,
                _selectedNodeStyle,
                _inPointStyle,
                _outPointStyle,
                OnClickInPoint,
                OnClickOutPoint,
                RemoveNode,
                actionLabel,
                isSelected
            );

            SetUnsavedChangesTabTitle();
        }
        
        /// <summary>
        /// Called when an ingoing connection point is clicked.
        /// </summary>
        /// <param name="inPoint">The point which was clicked.</param>
        private void OnClickInPoint(BbbtConnectionPoint inPoint)
        {
            // Check if the in point has a connection going to it.
            // If it does we don't want to do anything with it.
            if (FindConnectionToPoint(inPoint) == null)
            {
                // Select the in point.
                var previousSelectedInPoint = _selectedInPoint;
                _selectedInPoint = inPoint;

                // Out point selected?
                if (_selectedOutPoint != null)
                {
                    // Check that the in point and out point aren't on the same node.
                    if (_selectedOutPoint.Node != _selectedInPoint.Node)
                    {
                        // Not on the same node, create a connection from in point to out point.
                        CreateConnection();
                        ClearConnectionSelection();
                    }
                    else
                    {
                        // Out and in point are on the same node, clear selection.
                        ClearConnectionSelection();
                    }
                }
                else if (previousSelectedInPoint == null)
                {
                    // No point selected previously, create a preview.
                    _connectionPreview = new BbbtConnectionPreview(inPoint);
                }
            }
        }

        /// <summary>
        /// Called when an outgoing connection point is clicked.
        /// </summary>
        /// <param name="outPoint">The point which was clicked.</param>
        private void OnClickOutPoint(BbbtConnectionPoint outPoint)
        {
            // Check if the out point has reached its max capacity.
            // In practice this means that it is a decorator/root and has a connection point.
            if (!IsConnectionPointAtMaxCapacity(outPoint))
            {
                // Select the out point.
                var previousSelectedOutPoint = _selectedOutPoint;
                _selectedOutPoint = outPoint;

                // In point selected?
                if (_selectedInPoint != null)
                {
                    // Check that the in point and out point aren't on the same node.
                    if (_selectedOutPoint.Node != _selectedInPoint.Node)
                    {
                        // Not on the same node, create a connection from in point to out point.
                        CreateConnection();
                        ClearConnectionSelection();
                    }
                    else
                    {
                        // Out and in point are on the same node, clear selection.
                        ClearConnectionSelection();
                    }
                }
                else if (previousSelectedOutPoint == null)
                {
                    // No out point selected, create a preview.
                    _connectionPreview = new BbbtConnectionPreview(outPoint);
                }
            }
        }

        /// <summary>
        /// Creates a connection from the selected out point to the selected in point.
        /// </summary>
        public void CreateConnection()
        {
            _currentTab.Connections?.Add(new BbbtConnection(
                _selectedInPoint,
                _selectedOutPoint,
                (connection) => { _currentTab.Connections.Remove(connection); SetUnsavedChangesTabTitle(); }
            ));

            SetUnsavedChangesTabTitle();
        }

        /// <summary>
        /// Creates a connection between two nodes.
        /// </summary>
        public void CreateConnection(BbbtNode from, BbbtNode to)
        {
            _currentTab.Connections?.Add(new BbbtConnection(
                to.InPoint,
                from.OutPoint,
                (connection) => { _currentTab.Connections.Remove(connection); SetUnsavedChangesTabTitle(); }
            ));


            SetUnsavedChangesTabTitle();
        }

        /// <summary>
        /// Clears the selection of in point and out point.
        /// </summary>
        private void ClearConnectionSelection()
        {
            _selectedInPoint = null;
            _selectedOutPoint = null;
            _connectionPreview = null;
        }

        /// <summary>
        /// Finds a connection leading to/from a connection point.
        /// </summary>
        /// <returns>The connection connected to the connection point if it exists. Null otherwise.</returns>
        private BbbtConnection FindConnectionToPoint(BbbtConnectionPoint point)
        {
            foreach (var connection in _currentTab.Connections)
            {
                if (connection.InPoint == point || connection.OutPoint == point)
                {
                    // Found connection with point.
                    return connection;
                }
            }

            // No connection with point was found.
            return null;
        }

        /// <summary>
        /// Checks whether a connection is currently unable to support more connections.
        /// </summary>
        /// <param name="point">The point to check.</param>
        /// <returns>True if the connection point is at its maximum capacity, false otherwise.</returns>
        private bool IsConnectionPointAtMaxCapacity(BbbtConnectionPoint point)
        {
            switch (point.Type)
            {
                case BbbtConnectionPointType.In:
                    switch (point.Node.Type)
                    {
                        case BbbtNodeType.Root:
                            return true;
                        default:
                            return FindConnectionToPoint(point) != null;
                    }
                case BbbtConnectionPointType.Out:
                    switch (point.Node.Type)
                    {
                        case BbbtNodeType.Leaf:
                            return true;
                        case BbbtNodeType.Repeater:
                        case BbbtNodeType.Root:
                            return FindConnectionToPoint(point) != null;
                        default:
                            return false;
                    }
                default:
                    return true;
            }
        }

        /// <summary>
        /// Removes a node.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        private void RemoveNode(BbbtNode node)
        {
            // Remove connections.
            var connectionsToRemove = new List<BbbtConnection>();
            for (int i = 0; i < _currentTab.Connections.Count; i++)
            {
                // Check if the connection connects to a point on the node to be removed.
                if (_currentTab.Connections[i].InPoint.Node == node || _currentTab.Connections[i].OutPoint.Node == node)
                {
                    // Add the connection to be removed.
                    connectionsToRemove.Add(_currentTab.Connections[i]);
                }
            }
            foreach (var connection in connectionsToRemove)
            {
                _currentTab.Connections.Remove(connection);
            }

            // Remove node.
            _currentTab.Nodes.Remove(node);

            SetUnsavedChangesTabTitle();
        }

        /// <summary>
        /// Finds the currently selected node, if any.
        /// </summary>
        /// <returns>The currently selected node, if any. Null otherwise.</returns>
        private BbbtNode FindSelectedNode()
        {
            foreach (var node in _currentTab.Nodes)
            {
                if (node.IsSelected)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds an asterisk to the window title.
        /// </summary>
        public void SetUnsavedChangesTabTitle()
        {
            var window = GetWindow<BbbtWindow>();
            window.titleContent = new GUIContent("*bbBT - " + _currentTab.Tree.name);
        }

        /// <summary>
        /// Saves the behaviour tree to a file.
        /// </summary>
        private void SaveTree()
        {
            // Store all the nodes.
            var nodeSaveData = new BbbtNodeSaveData[_currentTab.Nodes.Count];
            for (int i = 0; i < _currentTab.Nodes.Count; i++)
            {
                nodeSaveData[i] = _currentTab.Nodes[i].ToSaveData();
            }

            // Store connections.
            var connectionSaveData = new BbbtConnectionSaveData[_currentTab.Connections.Count];
            for (int i = 0; i < _currentTab.Connections.Count; i++)
            {
                connectionSaveData[i] = _currentTab.Connections[i].ToSaveData();
            }

            // Create the behaviour tree save data.
            var behaviourTreeSaveData = new BbbtBehaviourTreeSaveData(
                nodeSaveData,
                connectionSaveData,
                _currentTab.WindowOffset.x,
                _currentTab.WindowOffset.y
            );

            // Save the data to the loaded scriptable object.
            _currentTab.Tree.SaveData(behaviourTreeSaveData);

            var window = GetWindow<BbbtWindow>();
            window.titleContent = new GUIContent("bbBT - " + _currentTab.Tree.name);
        }

        /// <summary>
        /// Loads a behaviour tree from a BbbtBehaviourTree scriptable object.
        /// </summary>
        public void LoadTree(BbbtBehaviourTree tree)
        {
            TreeToLoad = null;
            _currentTab = null;

            // See if the tree is loaded into a tab
            foreach (var tab in _tabs)
            {
                if (tab.Tree == tree)
                {
                    // Tree was already loaded.
                    _currentTab = tab;
                    break;
                }
            }

            // Tree was not loaded, load it in.
            if (_currentTab == null)
            {
                _tabs.Add(new BbbtWindowTab(tree, _tabStyle));
                _currentTab = _tabs[_tabs.Count - 1];

                if (tree.Data != null)
                {
                    // Add nodes
                    foreach (var nodeSaveData in tree.Data.Nodes)
                    {
                        AddNode(
                            nodeSaveData.Id,
                            (BbbtNodeType)Enum.Parse(typeof(BbbtNodeType), nodeSaveData.Type),
                            new Vector2(nodeSaveData.X, nodeSaveData.Y),
                            nodeSaveData.ActionLabel,
                            nodeSaveData.IsSelected
                        );
                        if (_currentTab.LastNodeID < nodeSaveData.Id)
                        {
                            _currentTab.LastNodeID = nodeSaveData.Id;
                        }
                    }

                    // Load connections.
                    foreach (var connectionSaveData in tree.Data.Connections)
                    {
                        CreateConnection(
                            _currentTab.Nodes.Find((node) => node.Id == connectionSaveData.OutNodeId),
                            _currentTab.Nodes.Find((node) => node.Id == connectionSaveData.InNodeId)
                        );
                    }

                    var window = GetWindow<BbbtWindow>();
                    window.titleContent = new GUIContent("bbBT - " + _currentTab.Tree.name);
                }
            }
        }
    }
}