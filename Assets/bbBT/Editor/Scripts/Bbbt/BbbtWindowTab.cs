using Bbbt.Commands;
using Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// A single tab to be displayed in the BbbtWindow.
    /// </summary>
    public class BbbtWindowTab
    {
        /// <summary>
        /// The tab's command manager.
        /// </summary>
        public CommandManager CommandManager { get; protected set; }

        /// <summary>
        /// The tab's command history.
        /// </summary>
        public BbbtTabCommandHistory CommandHistory { get; protected set; }

        /// <summary>
        /// The tab's rect.
        /// </summary>
        private Rect _rect;

        /// <summary>
        /// The id of the last created node.
        /// </summary>
        public int LastNodeID;

        /// <summary>
        /// The behaviour tree currently loaded into the editor window.
        /// </summary>
        public BbbtBehaviourTree Tree;

        /// <summary>
        /// The nodes that make up the behaviour tree.
        /// </summary>
        public List<BbbtNode> Nodes;

        /// <summary>
        /// The current connections between nodes.
        /// </summary>
        public List<BbbtConnection> Connections;

        /// <summary>
        /// The amount by which the entire window has been dragged from its initial position.
        /// </summary>
        public Vector3 WindowOffset = Vector3.zero;

        /// <summary>
        /// Whether the tab has unsaved changes.
        /// </summary>
        public bool IsUnsaved = false;

        /// <summary>
        /// Whether the tab is being dragged.
        /// </summary>
        private bool _isDragged = false;

        /// <summary>
        /// The position at which the tab previously moved. A non-zero value indicates that we should reference this
        /// value when deciding whether to move the tab back to its previous position.
        /// </summary>
        private Vector2 _tabMovedPosition = Vector2.zero;

        /// <summary>
        /// The mouse delta when the tab previously moved.
        /// </summary>
        private Vector2 _tabMovedDelta = Vector2.zero;


        /// <summary>
        /// Instantiates a new BbbtWindowTab.
        /// </summary>
        /// <param name="tree">The tree to use in the tab.</param>
        /// <param name="style">The style of the tab.</param>
        public BbbtWindowTab(BbbtBehaviourTree tree, GUIStyle style)
        {
            Nodes = new List<BbbtNode>();
            Connections = new List<BbbtConnection>();

            Tree = tree;
            //_style = style;
            LastNodeID = 0;

            // Calculated the size needed to fit the tree's name in the tab.
            _rect.size = new GUIStyle("PreButton").CalcSize(new GUIContent(tree.name + "*"));
        }


        /// <summary>
        /// Draws the tab.
        /// </summary>
        /// <param name="topBarRect">The rect of the top bar containing the tab.</param>
        /// <param name="tabs">The full list of tabs to be drawn.</param>
        /// <param name="isActive">Whether the tab is active.</param>
        public void Draw(Rect topBarRect, List<BbbtWindowTab> tabs, bool isActive = false)
        {
            // Figure out the rect's position by adding together the widths of the preceding tabs.
            _rect.position = topBarRect.position + new Vector2(3, 2);
            foreach (var tab in tabs)
            {
                if (tab == this)
                {
                    break;
                }
                _rect.position += Vector2.right * tab._rect.width;
            }
            string label = IsUnsaved ? Tree.name + "*" : Tree.name;
            string style = isActive ? "PreButtonBlue" : "PreButton";
            _rect.size = new GUIStyle("PreButton").CalcSize(new GUIContent(label));
            GUI.Box(_rect, label, style);
        }

        /// <summary>
        /// Processes events.
        /// </summary>
        /// <param name="e">The events to be processed.</param>
        /// <returns>
        /// Returns true if the GUI should change, false otherwise.
        /// A result of true should also be treated as the tab being set to active.
        /// </returns>
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                // Mouse button was just pressed.
                case EventType.MouseDown:
                    // LMB
                    if (e.button == 0)
                    {
                        // Are we clicking inside the rect?
                        if (_rect.Contains(e.mousePosition))
                        {
                            // Clicked inside the rect, select this tab and start dragging.
                            _isDragged = true;
                            e.Use();
                            return true;
                        }
                    }
                    // MMB
                    if (e.button == 2)
                    {
                        // Clicking inside rect?
                        if (_rect.Contains(e.mousePosition))
                        {
                            CloseTab();
                            e.Use();
                        }
                    }
                    // RMB
                    if (e.button == 1)
                    {
                        if (_rect.Contains(e.mousePosition))
                        {
                            CreateContextMenu(e.mousePosition);
                            e.Use();
                        }
                    }
                    break;

                // Mouse button released.
                case EventType.MouseUp:
                    _isDragged = false;
                    _tabMovedPosition = Vector2.zero;
                    _tabMovedDelta = Vector2.zero;
                    break;

                // Mouse moved.
                case EventType.MouseDrag:
                    // Is this tab currently being dragged?
                    if (_isDragged)
                    {
                        // Tab is being dragged, move with the mouse.
                        Drag(e.mousePosition, e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            // GUI did not change.
            return false;
        }

        /// <summary>
        /// Creates a context menu
        /// </summary>
        /// <param name="position">The position on which to create the context menu.</param>
        private void CreateContextMenu(Vector2 position)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Close Tab"), false, CloseTab);
            menu.ShowAsContext();
        }

        /// <summary>
        /// Closes the tab.
        /// </summary>
        private void CloseTab()
        {
            var window = EditorWindow.GetWindow<BbbtWindow>();
            window.TabToRemove = this;
        }

        /// <summary>
        /// Drags the tab.
        /// </summary>
        /// <param name="mousePosition">The position of the mouse.</param>
        /// <param name="delta">The amount by which to drag the tab.</param>
        private void Drag(Vector2 mousePosition, Vector2 delta)
        {
            var window = EditorWindow.GetWindow<BbbtWindow>();

            // 1. Move the tab if we moved beyond the rect's borders.
            // 2. If we've already moved the tab, store the position at which the tab moved and don't move
            // the tab back unless we move back the stored position. This is to prevent tabs jumping back and forth.
            // 3. If, however we've moved within the boundaries (or beyond) of the tab again, we revert to the behaviour
            // in step 1.
            if (_tabMovedPosition == Vector2.zero)
            {
                if (mousePosition.x < _rect.position.x)
                {
                    window.DecrementTabIndex(this);
                    _tabMovedPosition = mousePosition;
                    _tabMovedDelta = delta;
                }
                if (mousePosition.x > _rect.position.x + _rect.width)
                {
                    window.IncrementTabIndex(this);
                    _tabMovedPosition = mousePosition;
                    _tabMovedDelta = delta;
                }
            }
            else
            {
                // Check if the mouse is moving the same direction as it did when the tab moved.
                bool sameDirection = Mathf.Sign(delta.x) == Mathf.Sign(_tabMovedDelta.x);

                // If we're moving in the same direction.
                if (sameDirection)
                {
                    // Check if we've moved within or beyond the tab's new rect position.
                    if (delta.x > 0 && mousePosition.x > _rect.position.x ||
                        delta.x < 0 && mousePosition.x < _rect.position.x + _rect.width)
                    {
                        // Start treating our dragging logic as though we never moved.
                        _tabMovedPosition = Vector2.zero;
                        _tabMovedDelta = Vector2.zero;
                    }
                }
                // If we're moving the opposite direction.
                else
                {
                    // Check if we've moved back within or beyond the tab's old rect position.
                    if (delta.x > 0 && mousePosition.x > _tabMovedPosition.x ||
                        delta.x < 0 && mousePosition.x < _tabMovedPosition.x)
                    {
                        // Move the tab back and treat it as though it had never been dragged.
                        if (delta.x < 0) window.DecrementTabIndex(this);
                        if (delta.x > 0) window.IncrementTabIndex(this);
                        _tabMovedPosition = Vector2.zero;
                        _tabMovedDelta = Vector2.zero;
                    }
                }
            }
        }

        /// <summary>
        /// Resets the CommandManager/command strings for this tab.
        /// </summary>
        public void ResetCommands()
        {
            CommandManager = new CommandManager();
            CommandHistory = new BbbtTabCommandHistory(CommandManager);
            CommandManager.Do(new LastResetCommand());
        }

        /// <summary>
        /// Sets the behaviour children according to the nodes' position in the editor.
        /// </summary>
        public void SetBehaviourChildren()
        {
            // Clear all behaviours' children
            // This is necessary to avoid double-adding children.
            foreach (var node in Nodes)
            {
                node.Behaviour.RemoveChildren();
            }

            // Set up the nodes' children,
            // making sure that the order matches the children's order in the editor (left-to-right).
            var parentToChildren = new Dictionary<BbbtNode, List<BbbtNode>>();
            foreach (var connection in Connections)
            {
                var parent = connection.OutPoint.Node;
                var child = connection.InPoint.Node;

                if (!parentToChildren.ContainsKey(parent))
                {
                    parentToChildren[parent] = new List<BbbtNode>();
                    parentToChildren[parent].Add(child);
                }
                else
                {
                    // Insert the new child before the first existing child that is to the right of the new child.
                    BbbtNode childToTheRightOfNewChild = null;
                    foreach (var existingChild in parentToChildren[parent])
                    {
                        if (existingChild.Rect.x > child.Rect.x)
                        {
                            childToTheRightOfNewChild = existingChild;
                            break;
                        }
                    }
                    if (childToTheRightOfNewChild != null)
                    {
                        parentToChildren[parent].Insert(
                            parentToChildren[parent].IndexOf(childToTheRightOfNewChild),
                            child
                        );
                    }
                    else
                    {
                        parentToChildren[parent].Add(child);
                    }
                }
            }

            // Actually add the children based on the ordering we figured out.
            foreach (var parent in parentToChildren.Keys)
            {
                foreach (var child in parentToChildren[parent])
                {
                    parent.Behaviour.AddChild(child.Behaviour);
                }
            }
        }

        /// <summary>
        /// Finds a node in the tab with a given id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>The node with the given id.</returns>
        public BbbtNode FindNodeWithId(int id)
        {
            return Nodes.Where(n => n.Id == id).First();
        }

        /// <summary>
        /// Finds the node which the given node is a child of.
        /// </summary>
        /// <param name="node">The node to find the parent of.</param>
        /// <returns>The parent of the given node.</returns>
        public BbbtNode FindParentNode(BbbtNode node)
        {
            foreach (var potentialParentNode in Nodes)
            {
                var root = potentialParentNode.Behaviour as BbbtRoot;
                var decorator = potentialParentNode.Behaviour as BbbtDecoratorBehaviour;
                var composite = potentialParentNode.Behaviour as BbbtCompositeBehaviour;

                if (root != null && root.Child == node.Behaviour)
                {
                    return potentialParentNode;
                }
                else if (decorator != null && decorator.Child == node.Behaviour)
                {
                    return potentialParentNode;
                }
                else if (composite != null && composite.Children != null &&
                         composite.Children.Contains(node.Behaviour))
                {
                    return potentialParentNode;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a root node exists in the tab.
        /// </summary>
        /// <returns>True if root node exists, false otherwise.</returns>
        public bool DoesRootExist()
        {
            foreach (var node in Nodes)
            {
                if (node.Behaviour as BbbtRoot != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}