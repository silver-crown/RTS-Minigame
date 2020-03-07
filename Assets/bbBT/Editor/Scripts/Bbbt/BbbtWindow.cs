using Bbbt.Commands;
using Commands;
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
        /// Used to tell the editor to remove a tab.
        /// Used to avoid removing a tab in the middle of processing events.
        /// </summary>
        public BbbtWindowTab TabToRemove = null;

        /// <summary>
        /// Tabs currently loaded into the BbbtWindow.
        /// </summary>
        private List<BbbtWindowTab> _tabs = null;

        /// <summary>
        /// The currently selected tab.
        /// </summary>
        public BbbtWindowTab CurrentTab { get; protected set; } = null;

        /// <summary>
        /// The currently open prompt if any.
        /// If this is not null all other input should be blocked until the prompt is dealt with.
        /// </summary>
        private BbbtPrompt _prompt = null;

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
        /// Application.isPlaying last time we checked.
        /// </summary>
        private bool _lastIsPlaying;

        /// <summary>
        /// The window's side panel.
        /// </summary>
        private BbbtSidePanel _sidePanel;

        /// <summary>
        /// Whether node labels should always be shown.
        /// </summary>
        private bool _alwaysShowNodeLabels;

        /// <summary>
        /// Whether the window should resize with the side panel.
        /// </summary>
        private bool _resizeWithSidePanel = false;


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
            titleContent = new GUIContent("bbBT");

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


            _tabs = new List<BbbtWindowTab>();
            _sidePanel = new BbbtSidePanel(this);
            if (_resizeWithSidePanel)
            {
                _sidePanel.OnOpenSidePanel += () =>
                {
                    DragWindow(new Vector2(_sidePanel.GetTotalWidth(true) - _sidePanel.GetNavigationBarWidth(), 0.0f));
                };
                _sidePanel.OnCloseSidePanel += () =>
                {
                    DragWindow(-new Vector2(_sidePanel.GetTotalWidth(true) - _sidePanel.GetNavigationBarWidth(), 0.0f));
                };
                _sidePanel.OnResizeSidePanel += (delta) =>
                {
                    DragWindow(new Vector2(delta, 0.0f));
                };
            }
            _alwaysShowNodeLabels = false;
        }

        /// <summary>
        /// Called more or less when something needs to change in the editor.
        /// </summary>
        private void OnGUI()
        {
            // Close all tabs if we leave/enter play mode
            if (_lastIsPlaying != Application.isPlaying)
            {
                while (_tabs != null && _tabs.Count > 0)
                {
                    CloseTab(_tabs[0]);
                }
                _sidePanel = new BbbtSidePanel(this);
            }
            // Check if the behaviour tree has disappeared.
            if (CurrentTab != null && CurrentTab.Tree == null)
            {
                CloseTab(CurrentTab);
            }

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

            DrawNodes();
            DrawConnections();
            if (_alwaysShowNodeLabels) DrawNodeLabels();

            // Block input if we have an open prompt.
            if (_prompt == null)
            {
                ProcessNodeEvents(Event.current);
                ProcessTabEvents(Event.current);
                ProcessEvents(Event.current);
            }

            DrawTopBar();
            DrawTabs();

            _sidePanel.ProcessEvents(Event.current);
            _sidePanel.Draw();
            
            // Draw the prompt if there is one and check if it was handled.
            if (_prompt != null && _prompt.Draw())
            {
                // The prompt was handled, so we can get rid of it.
                _prompt = null;
            }

            if (GUI.changed)
            {
                Repaint();
            }

            if (TreeToLoad != null)
            {
                LoadTree(TreeToLoad);
            }

            if (TabToRemove != null)
            {
                CloseTab(TabToRemove);
                TabToRemove = null;
            }

            _lastIsPlaying = Application.isPlaying;
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

            _topBarRect.x = _sidePanel.GetTotalWidth();
            _topBarRect.width = position.width - _topBarRect.x;
            GUI.Box(_topBarRect, "", _topBarStyle);
        }

        /// <summary>
        /// Draws the tabs in the top bar.
        /// </summary>
        private void DrawTabs()
        {
            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_tabs[i] == CurrentTab)
                {
                    // Highlight current tab
                    _tabs[i].Draw(_topBarRect, _tabs, true);
                }
                else
                {
                    _tabs[i].Draw(_topBarRect, _tabs, false);
                }
            }
        }

        /// <summary>
        /// Closes a tab.
        /// </summary>
        /// <param name="tab">The tab to close.</param>
        /// <param name="force">Whether the tab should be forced to close (i.e. no unsaved changes prompt).</param>
        private void CloseTab(BbbtWindowTab tab, bool force = false)
        {
            // Check if the tab has unsaved changes, if so we want to prompt the user to save the contents of the tab.
            if (tab.Tree != null && tab.IsUnsaved && !force)
            {
                _prompt = new BbbtPrompt(
                    tab.Tree.name + " has unsaved changes. Do you want to save the changes before closing the tab?",
                    new List<BbbtPromptOption>()
                    {
                        new BbbtPromptOption("Save", () => { SaveTab(tab); CloseTab(tab); }),
                        new BbbtPromptOption("Don't Save", () => CloseTab(tab, true)),
                        new BbbtPromptOption("Cancel", () => { })
                    }
                );
            }
            else
            {
                // No unsaved changes (or forced to close), close the tab.
                int index = _tabs.IndexOf(tab);
                _tabs.Remove(tab);
                if (_tabs.Count == 0)
                {
                    CurrentTab = null;
                }
                else if (tab == CurrentTab)
                {
                    CurrentTab = _tabs[Mathf.Clamp(index, 0, _tabs.Count - 1)];
                }
            }
        }

        /// <summary>
        /// Draws all nodes in the window.
        /// </summary>
        private void DrawNodes()
        {
            if (CurrentTab != null && CurrentTab.Nodes != null)
            {
                CurrentTab.Nodes.ForEach((node) => node.Draw());
            }
        }
        
        /// <summary>
        /// Draws all node labe.
        /// </summary>
        private void DrawNodeLabels()
        {
            if (CurrentTab != null && CurrentTab.Nodes != null)
            {
                CurrentTab.Nodes.ForEach((node) => node.DrawLabel());
            }
        }

        /// <summary>
        /// Draws all connections in the behaviour tree.
        /// </summary>
        private void DrawConnections()
        {
            if (CurrentTab != null)
            {
                // We use a regular for loop in case a connection gets removed.
                for (int i = 0; i < CurrentTab.Connections.Count; i++)
                {
                    CurrentTab.Connections[i].Draw();
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
            if (CurrentTab == null) return;

            _drag = Vector3.zero;
            BbbtBehaviour draggedBehavior = null;

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
                        if (!Application.isPlaying)
                        {
                            CreateContextMenu(e.mousePosition);
                        }
                    }
                    break;
                // Mouse moved.
                case EventType.MouseDrag:
                    // MMB pressed
                    if (e.button == 2)
                    {
                        // Drag the entire window.
                        DragWindow(e.delta);
                    }
                    break;
                // Started pressing a key.
                case EventType.KeyDown:
                    // Pressed Ctrl-S down.
                    if (e.control && e.keyCode == KeyCode.S)
                    {
                        if (!Application.isPlaying)
                        {
                            SaveTree();
                        }
                        e.Use();
                    }
                    // Pressed the delete key.
                    if (e.keyCode == KeyCode.Delete)
                    {
                        if (!Application.isPlaying)
                        {
                            // Delete the selected node if there is one.
                            var selectedNode = FindSelectedNode();
                            if (selectedNode != null)
                            {
                                RemoveNode(selectedNode);
                                GUI.changed = true;
                            }
                        }
                        e.Use();
                    }
                    // Pressed tab (select next node).
                    if (e.keyCode == KeyCode.Tab)
                    {
                        if (CurrentTab.Nodes != null && CurrentTab.Nodes.Count > 0)
                        {
                            SelectNode(CurrentTab.Nodes[0]);
                        }
                        e.Use();
                    }
                    // Pressed shift-f (focus on selected node).
                    if (e.shift && e.keyCode == KeyCode.F)
                    {
                        var selectedNode = FindSelectedNode();
                        if (selectedNode != null)
                        {
                            // Find the centre of the node
                            var nodeCenter = selectedNode.Rect.position + selectedNode.Rect.size / 2.0f;

                            // Move the window by the distance between the node centre and window centre.
                            DragWindow(position.size / 2.0f - nodeCenter);
                        }
                        e.Use();
                    }
                    // Pressed ctrl-z (undo).
                    if (e.control && e.keyCode == KeyCode.Z)
                    {
                        CurrentTab.CommandManager.Undo();
                        e.Use();
                    }
                    // Pressed ctrl-y (redo).
                    if (e.control && e.keyCode == KeyCode.Y)
                    {
                        CurrentTab.CommandManager.Redo();
                        e.Use();
                    }
                    // Pressed T (for tooltip).
                    if (e.keyCode == KeyCode.T)
                    {
                        _alwaysShowNodeLabels = !_alwaysShowNodeLabels;
                        GUI.changed = true;
                        e.Use();
                    }
                    break;
                // Something dragged into the editor window.
                case EventType.DragUpdated:
                    if (!Application.isPlaying)
                    {
                        // Check if we're dragging a valid object into the editor
                        // and update visuals to reflect the fact that it's valid if it is.
                        draggedBehavior = DragAndDrop.objectReferences[0] as BbbtBehaviour;

                        if (draggedBehavior != null)
                        {
                            if (!CurrentTab.DoesRootExist() || draggedBehavior as BbbtRoot == null)
                            {
                                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                            }
                        }
                    }
                    break;
                // We stopped dragging whatever we were dragging.
                case EventType.DragExited:
                    if (!Application.isPlaying)
                    {
                        // Check if we dragged a valid object into the editor
                        draggedBehavior = DragAndDrop.objectReferences[0] as BbbtBehaviour;

                        if (draggedBehavior != null)
                        {
                            if (!CurrentTab.DoesRootExist() || draggedBehavior as BbbtRoot == null)
                            {
                                // A behaviour was dropped into the editor,
                                // instantiate a node with the behaviour attached.
                                var node = AddNode(++CurrentTab.LastNodeID, draggedBehavior, e.mousePosition, true);
                                SelectNode(node);
                            }
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Drags the entire window.
        /// </summary>i
        /// <param name="delta">The amound by which to drag the window.</param>
        private void DragWindow(Vector2 delta)
        {
            _drag = delta;
            CurrentTab.WindowOffset += _drag;
            CurrentTab.Nodes?.ForEach((node) => { node.Drag(delta); });
            GUI.changed = true;
        }

        /// <summary>
        /// Processes node events.
        /// </summary>
        /// <param name="e">The events to be handled.</param>
        private void ProcessNodeEvents(Event e)
        {
            if (CurrentTab == null) return;

            // The node that was used in some way during the processing of node events.
            BbbtNode usedNode = null;

            // Iterate in the reverse of our draw order because we want to give nodes in the foreground priority.
            for (int i = CurrentTab.Nodes.Count - 1; i >= 0; i--)
            {
                if (CurrentTab.Nodes[i].ProcessEvents(this, e))
                {
                    // This node either moved or was clicked on.
                    GUI.changed = true;
                    usedNode = CurrentTab.Nodes[i];
                }
            }

            // Check if usedNode isn't null, i.e. a node was interacted with.
            if (usedNode != null)
            {
                if (usedNode.IsSelected)
                {
                    SelectNode(usedNode);
                }
                else
                {
                    // Put the node on top.
                    CurrentTab.Nodes.Remove(usedNode);
                    CurrentTab.Nodes.Add(usedNode);
                }
            }
        }

        /// <summary>
        /// Processes tab events.
        /// </summary>
        /// <param name="e">The events to be handled.</param>
        private void ProcessTabEvents(Event e)
        {
            if (CurrentTab == null) return;

            for (int i = 0; i < _tabs.Count; i++)
            {
                if (_tabs[i].ProcessEvents(e))
                {
                    // This tab either moved or was clicked on. Select the tab.
                    CurrentTab = _tabs[i];
                    GUI.changed = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Selects a node.
        /// </summary>
        /// <param name="node">The node to select.</param>
        public void SelectNode(BbbtNode node)
        {
            // Deselect the selected node.
            var selectedNode = FindSelectedNode();
            if (selectedNode != null)
            {
                selectedNode.IsSelected = false;
            }

            // Select the node and open in inspector.
            node.IsSelected = true;
            _nodeToOpenInInspector = node;

            // Put the node on top.
            CurrentTab.Nodes.Remove(node);
            CurrentTab.Nodes.Add(node);

            SetUnsavedChangesTabTitle(CurrentTab);
        }

        /// <summary>
        /// Creates a context menu.
        /// </summary>
        /// <param name="position">The positioni of the context menu.</param>
        private void CreateContextMenu(Vector2 position)
        {
            // Show an entry for all types of behaviours.
            var menu = new GenericMenu();

            // Root
            foreach (var behaviour in BbbtBehaviour.GetAllInstances<BbbtRoot>())
            {
                // Check if a root node exists
                bool rootExists = CurrentTab.DoesRootExist();

                if (!rootExists)
                {
                    // Add root node to menu
                    menu.AddItem(
                        new GUIContent("Add " + behaviour.name),
                        false,
                        () => AddNode(++CurrentTab.LastNodeID, behaviour, position)
                    );
                }
                else
                {
                    // Add disabled root node to menu
                    menu.AddDisabledItem(new GUIContent("Add " + behaviour.name));
                }
            }

            // Composite
            foreach (var behaviour in BbbtBehaviour.GetAllInstances<BbbtCompositeBehaviour>())
            {
                menu.AddItem(
                       new GUIContent("Composite/Add " + behaviour.name),
                       false,
                       () => AddNode(++CurrentTab.LastNodeID, behaviour, position)
                   );
            }

            // Decorator
            foreach (var behaviour in BbbtBehaviour.GetAllInstances<BbbtDecoratorBehaviour>())
            {
                menu.AddItem(
                       new GUIContent("Decorator/Add " + behaviour.name),
                       false,
                       () => AddNode(++CurrentTab.LastNodeID, behaviour, position)
                   );
            }

            // Leaf
            foreach (var behaviour in BbbtBehaviour.GetAllInstances<BbbtLeafBehaviour>())
            {
                menu.AddItem(
                       new GUIContent("Leaf/Add " + behaviour.name),
                       false,
                       () => AddNode(++CurrentTab.LastNodeID, behaviour, position)
                   );
            }

            menu.ShowAsContext();
        }

        /// <summary>
        /// Adds a new node to the behaviour tree.
        /// </summary>
        /// <param name="id">The id of the node.</param>
        /// <param name="baseBehaviour">The behaviour attached to the node.</param>
        /// <param name="position">The position of the node.</param>
        /// <param name="isSelected">Whether the node should be selected.</param>
        /*/// <param name="behaviourSaveData">
        /// The save data associated used to reconstruct the behaviour of the node if loading the node from file.
        /// </param>*/
        /// <param name="behaviour">The behaviour of the node</param>
        public BbbtNode AddNode(
            int id,
            BbbtBehaviour baseBehaviour,
            Vector2 position,
            bool isSelected = false,
            //BbbtBehaviourSaveData behaviourSaveData = null,
            BbbtBehaviour behaviour = null)
        {
            var node = CreateInstance<BbbtNode>();
            node.Setup(
                id,
                baseBehaviour,
                position,
                CurrentTab,
                96,
                96,
                _nodeStyle,
                _selectedNodeStyle,
                _inPointStyle,
                _outPointStyle,
                OnClickInPoint,
                OnClickOutPoint,
                RemoveNode,
                isSelected,
                //behaviourSaveData,
                behaviour
            );

            CurrentTab.CommandManager.Do(new CreateNodeCommand(this, node));
            return node;
        }
        
        /// <summary>
        /// Called when an ingoing connection point is clicked.
        /// </summary>
        /// <param name="inPoint">The point which was clicked.</param>
        private void OnClickInPoint(BbbtConnectionPoint inPoint)
        {
            if (!Application.isPlaying)
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
        }

        /// <summary>
        /// Called when an outgoing connection point is clicked.
        /// </summary>
        /// <param name="outPoint">The point which was clicked.</param>
        private void OnClickOutPoint(BbbtConnectionPoint outPoint)
        {
            if (!Application.isPlaying)
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
        }

        /// <summary>
        /// Creates a connection from the selected out point to the selected in point.
        /// </summary>
        public void CreateConnection()
        {
            var connection = new BbbtConnection(
                _selectedInPoint,
                _selectedOutPoint,
                (c) =>
                {
                    if (!Application.isPlaying)
                    {
                        CurrentTab.CommandManager.Do(new RemoveConnectionCommand(this, c));
                    }
                }
            );

            CurrentTab.CommandManager.Do(new CreateConnectionCommand(this, connection));
        }

        /// <summary>
        /// Creates a connection between two nodes.
        /// </summary>
        public void CreateConnection(BbbtNode from, BbbtNode to)
        {
            var connection = new BbbtConnection(
                to.InPoint,
                from.OutPoint,
                (c) =>
                {
                    if (!Application.isPlaying)
                    {
                        CurrentTab.CommandManager.Do(new RemoveConnectionCommand(this, c));
                    }
                }
            );

            CurrentTab.CommandManager.Do(new CreateConnectionCommand(this, connection));
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
            foreach (var connection in CurrentTab.Connections)
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
                    // Root nodes have no InPoint, so it should always be considered at max capacity.
                    // Though, this should never be called for a root node if everything is done correctly.
                    if (point.Node.BaseBehaviour as BbbtRoot != null)
                    {
                        return true;
                    }
                    // All other InPoints take a single parent so just check if it already has one.
                    else
                    {
                        return FindConnectionToPoint(point) != null;
                    }
                case BbbtConnectionPointType.Out:
                    // TODO: Leaf nodes are always considered at max OutPoint capacity.
                    // Root nodes and (TODO: decorators) can only have one outgoing connection.
                    if (point.Node.BaseBehaviour as BbbtRoot != null)
                    {
                        return FindConnectionToPoint(point) != null;
                    }
                    // Composite nodes are never at max capacity.
                    else if (point.Node.BaseBehaviour as BbbtCompositeBehaviour != null)
                    {
                        return false;
                    }
                    break;
                default:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes a node.
        /// </summary>
        /// <param name="node">The node to be removed.</param>
        private void RemoveNode(BbbtNode node)
        {
            if (!Application.isPlaying)
            {
                // Remove connections.
                var connectionsToRemove = new List<BbbtConnection>();
                for (int i = 0; i < CurrentTab.Connections.Count; i++)
                {
                    // Check if the connection connects to a point on the node to be removed.
                    if (CurrentTab.Connections[i].InPoint.Node == node ||
                        CurrentTab.Connections[i].OutPoint.Node == node)
                    {
                        // Add the connection to be removed.
                        connectionsToRemove.Add(CurrentTab.Connections[i]);
                    }
                }

                CurrentTab.CommandManager.Do(new RemoveNodeCommand(this, node, connectionsToRemove));
            }
        }

        /// <summary>
        /// Finds the currently selected node, if any.
        /// </summary>
        /// <returns>The currently selected node, if any. Null otherwise.</returns>
        private BbbtNode FindSelectedNode()
        {
            foreach (var node in CurrentTab.Nodes)
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
        /// <param name="tab">The tab whose unsaved state should change.</param>
        /// <param name="isUnsaved">Whether the tree is now unsaved.</param>
        public void SetUnsavedChangesTabTitle(BbbtWindowTab tab, bool isUnsaved = true)
        {
            if (!Application.isPlaying)
            {
                tab.IsUnsaved = isUnsaved;
                GUI.changed = true;
            }
        }

        /// <summary>
        /// Decrements the index of <paramref name="tab"/> in the tab list if possible.
        /// This has the effect of moving it to the left in the top bar.
        /// </summary>
        /// <param name="tab">The tab whose index should be decremented.</param>
        public void DecrementTabIndex(BbbtWindowTab tab)
        {
            int index = _tabs.IndexOf(tab);
            if (index > 0)
            {
                _tabs.Remove(tab);
                _tabs.Insert(index - 1, tab);
            }
        }

        /// <summary>
        /// Increments the index of <paramref name="tab"/> in the tab list if possible.
        /// This has the effect of moving it to the right in the top bar.
        /// </summary>
        /// <param name="tab">The tab whose index should be incremented.</param>
        public void IncrementTabIndex(BbbtWindowTab tab)
        {
            int index = _tabs.IndexOf(tab);
            if (index < _tabs.Count - 1)
            {
                _tabs.Remove(tab);
                _tabs.Insert(index + 1, tab);
            }
        }


        /// <summary>
        /// Saves the behaviour tree in the current tab to a file.
        /// </summary>
        private void SaveTree()
        {
            // Store all the nodes.
            SaveTab(CurrentTab);
        }

        /// <summary>
        /// Saves the contents of a tab.
        /// </summary>
        /// <param name="tab">The tab whose contents to save.</param>
        private void SaveTab(BbbtWindowTab tab)
        {
            tab.SetBehaviourChildren();

            // Store nodes.
            var nodeSaveData = new BbbtNodeSaveData[tab.Nodes.Count];
            //BbbtRootSaveData rootSaveData = null;
            BbbtRoot root = null;
            var behaviours = new List<BbbtBehaviour>();
            for (int i = 0; i < tab.Nodes.Count; i++)
            {
                tab.Nodes[i].Behaviour.NodeId = tab.Nodes[i].Id;
                nodeSaveData[i] = tab.Nodes[i].ToSaveData();
                behaviours.Add(tab.Nodes[i].Behaviour);
                if (tab.Nodes[i].Behaviour as BbbtRoot != null)
                {
                    //rootSaveData = (BbbtRootSaveData)nodeSaveData[i].BehaviourSaveData;
                    root = (BbbtRoot)nodeSaveData[i].Behaviour;
                }
            }

            // Store connections.
            var connectionSaveData = new BbbtConnectionSaveData[tab.Connections.Count];
            for (int i = 0; i < tab.Connections.Count; i++)
            {
                connectionSaveData[i] = tab.Connections[i].ToSaveData();
            }

            // Create the behaviour tree save data.
            var behaviourTreeSaveData = new BbbtBehaviourTreeEditorSaveData(
                nodeSaveData,
                connectionSaveData,
                tab.WindowOffset.x,
                tab.WindowOffset.y
            );

            // Save editor data
            tab.Tree.Save(behaviourTreeSaveData, null);
            SetUnsavedChangesTabTitle(tab, false);

            // Save functional save data if the tree is valid.
            if (root != null/*rootSaveData != null*/)
            {
                bool isValid = true;
                foreach (var behaviour in behaviours)
                {
                    if (behaviour as BbbtRoot != null &&
                        (behaviour as BbbtRoot).Child == null)
                    {
                        isValid = false;
                    }
                    if (behaviour as BbbtCompositeBehaviour != null &&
                        (behaviour as BbbtCompositeBehaviour).Children == null)
                    {
                        isValid = false;
                    }
                    if (behaviour as BbbtDecoratorBehaviour != null &&
                        (behaviour as BbbtDecoratorBehaviour).Child == null)
                    {
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    var saveData = new BbbtBehaviourTreeSaveData(root/*rootSaveData*/);
                    tab.Tree.Save(null, saveData);
                }
            }

            tab.CommandHistory.LastSaveCommand =
                tab.CommandHistory.DoneCommands[tab.CommandHistory.DoneCommands.Count - 1];
        }

        /// <summary>
        /// Loads a behaviour tree from a BbbtBehaviourTree scriptable object.
        /// </summary>
        /// <param name="tree">The tree to open.</param>
        public void LoadTree(BbbtBehaviourTree tree)
        {
            TreeToLoad = null;
            CurrentTab = null;

            // See if the tree is loaded into a tab
            foreach (var tab in _tabs)
            {
                if (tab.Tree == tree)
                {
                    // Tree was already loaded.
                    CurrentTab = tab;
                    break;
                }
            }

            // Tree was not loaded, load it in.
            if (CurrentTab == null)
            {
                _tabs.Add(new BbbtWindowTab(tree, _tabStyle));
                CurrentTab = _tabs[_tabs.Count - 1];
                CurrentTab.ResetCommands();

                if (tree.EditorSaveData != null)
                {
                    var nodeIdToBehaviour = new Dictionary<int, BbbtBehaviour>();
                    if (Application.isPlaying)
                    {
                        // Loading a tree for debugging.
                        foreach (var behaviour in tree.Behaviours)
                        {
                            nodeIdToBehaviour.Add(behaviour.NodeId, behaviour);
                        }
                    }
                    // Add nodes
                    foreach (var nodeSaveData in tree.EditorSaveData.Nodes)
                    {
                        // Get the behaviour instance from the save data's string and check if it's valid.
                        BbbtBehaviour baseBehaviour = null;
                        UnityEngine.Object @object = EditorUtility.InstanceIDToObject(
                            nodeSaveData.BaseBehaviourInstanceId
                        );
                        try
                        {
                            baseBehaviour = (BbbtBehaviour)@object;
                        }
                        catch (InvalidCastException)
                        {
                        }
                        if (baseBehaviour == null)
                        {
                            baseBehaviour = BbbtBehaviour.FindBehaviourWithName(nodeSaveData.BaseBehaviour);
                        }
                        //var behaviourSaveData = nodeSaveData.BehaviourSaveData;
                        var behaviour = nodeSaveData.Behaviour;

                        if (baseBehaviour != null)
                        {
                            var node = AddNode(
                                nodeSaveData.Id,
                                baseBehaviour,
                                new Vector2(nodeSaveData.X, nodeSaveData.Y),
                                nodeSaveData.IsSelected,
                                //behaviourSaveData
                                behaviour
                            );
                            if (CurrentTab.LastNodeID < nodeSaveData.Id)
                            {
                                CurrentTab.LastNodeID = nodeSaveData.Id;
                            }

                            if (Application.isPlaying)
                            {
                                node.Behaviour = nodeIdToBehaviour[nodeSaveData.Id];
                            }
                        }
                        else
                        {
                            Debug.LogError(
                                CurrentTab.Tree.name +
                                ": Couldn't load behaviour '" +
                                nodeSaveData.BaseBehaviour +
                                "'.");
                        }
                    }

                    // Load connections.
                    foreach (var connectionSaveData in tree.EditorSaveData.Connections)
                    {
                        CreateConnection(
                            CurrentTab.Nodes.Find((node) => node.Id == connectionSaveData.OutNodeId),
                            CurrentTab.Nodes.Find((node) => node.Id == connectionSaveData.InNodeId)
                        );
                    }
                }
            }
            CurrentTab.ResetCommands();
            CurrentTab.IsUnsaved = false;
        }
    }
}