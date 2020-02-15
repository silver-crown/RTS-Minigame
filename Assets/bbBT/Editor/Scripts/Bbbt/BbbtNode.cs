using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// The type of a BbbtNode.
    /// </summary>
    public enum BbbtNodeType { Root, Repeater, Selector, Sequence, Leaf }

    /// <summary>
    /// A node for use in the bbBT behaviour tree editor.
    /// </summary>
    public class BbbtNode : ScriptableObject
    {
        /// <summary>
        /// A unique id used to reference the node.
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// The type of the node.
        /// </summary>
        public BbbtNodeType Type { get; protected set; }

        /// <summary>
        /// The node's rect.
        /// </summary>
        private Rect _rect;

        /// <summary>
        /// The rect used to show the icon for the node's type.
        /// </summary>
        private Rect _typeIconRect;

        /// <summary>
        /// The rect used for positioning the node's label.
        /// </summary>
        private Rect _labelRect;

        /// <summary>
        /// The node's GUIStyle.
        /// </summary>
        private GUIStyle _style;

        /// <summary>
        /// The style of the rect which displays the icon for the node's type.
        /// </summary>
        private GUIStyle _typeIconStyle;

        /// <summary>
        /// The style used for the node when selected.
        /// </summary>
        private GUIStyle _selectedStyle;

        /// <summary>
        /// The connection point that leads in to this node.
        /// </summary>
        public BbbtConnectionPoint InPoint;

        /// <summary>
        /// The connection point that leads out from this node.
        /// </summary>
        public BbbtConnectionPoint OutPoint;

        /// <summary>
        /// Whether the node is currently being dragged.
        /// </summary>
        private bool _isDragged = false;

        /// <summary>
        /// Whether the node is currently selected.
        /// </summary>
        public bool IsSelected { get; protected set; } = false;

        /// <summary>
        /// Action invoked when the node is removed.
        /// </summary>
        private Action<BbbtNode> _onClickRemoveNode;

        /// <summary>
        /// PLACEHOLDER: An action attached to the node. Just other behaviour trees now for testing.
        /// Maybe turn this into a generic object and query its type?
        /// </summary>
        public BbbtBehaviourTree AttachedAction;

        /// <summary>
        /// The label attached to the node's rect indicating the attached action.
        /// </summary>
        private string _actionLabel;

        /// <summary>
        /// The time the last time this node was clicked.
        /// </summary>
        private double _lastClickTime = 0.0f;

        /// <summary>
        /// The max time between two clicks that can produce a double click.
        /// </summary>
        private double _doubleClickMaxTime = 0.3f;


        /// <summary>
        /// Sets up a new BbbtNode.
        /// </summary>
        /// <param name="id">The node's id.</param>
        /// <param name="type">The type of the node.</param>
        /// <param name="position">The position of the node.</param>
        /// <param name="width">The width of the node.</param>
        /// <param name="height">The height of the node.</param>
        /// <param name="style">The node's GUIStyle.</param>
        /// <param name="selectedStyle">The style used for the node when selected.</param>
        /// <param name="inPointStyle">The style used for the attached ingoing connection point.</param>
        /// <param name="outPointStyle">The style used for the attached outgoing connection point.</param>
        /// <param name="inPointOnClick">
        /// Callback method invoked when the attached ingoing connection point is clicked.
        /// </param>
        /// <param name="outPointOnClick">
        /// Callback method invoked when the attached outgoing connection point is clicked.
        /// </param>
        /// <param name="onClickRemoveNode">Action invoked when the node is removed.</param>
        /// <param name="actionLabel">The label used to identify the action attached to the node.</param>
        /// <param name="isSelected">Whether the node should start out selected.</param>
        public void Setup(
            int id,
            BbbtNodeType type,
            Vector2 position,
            float width,
            float height,
            GUIStyle style,
            GUIStyle selectedStyle,
            GUIStyle inPointStyle,
            GUIStyle outPointStyle,
            Action<BbbtConnectionPoint> inPointOnClick,
            Action<BbbtConnectionPoint> outPointOnClick,
            Action<BbbtNode> onClickRemoveNode,
            string actionLabel,
            bool isSelected = false)
        {
            Id = id;
            Type = type;
            _rect = new Rect(position.x, position.y, width, height);

            // Set up the node type icon
            // Create style
            _typeIconStyle = new GUIStyle();
            _typeIconStyle.normal.background = AssetDatabase.LoadAssetAtPath<Texture2D>(
                Path.Combine("Assets", "bbBT", "Editor", "Textures", type.ToString().ToLower() + ".png")
            );

            // Create the rects, make them some reasonable size.
            _typeIconRect = new Rect(
                _rect.position + _rect.size / 4.0f,
                _rect.size / 2.0f
            );

            _labelRect = new Rect(
                new Vector2(_rect.x + 10.0f, _rect.y + _rect.height - 22.5f),
                new Vector2(_rect.width - 10.0f * 2.0f, 30.0f)
            );

            _style = style;
            _selectedStyle = selectedStyle;

            if (Type != BbbtNodeType.Root)
            {
                InPoint = new BbbtConnectionPoint(this, BbbtConnectionPointType.In, inPointStyle, inPointOnClick);
            }

            if (Type != BbbtNodeType.Leaf)
            {
                OutPoint = new BbbtConnectionPoint(this, BbbtConnectionPointType.Out, outPointStyle, outPointOnClick);
            }

            _onClickRemoveNode = onClickRemoveNode;
            _actionLabel = actionLabel;
            IsSelected = isSelected;
        }

        /// <summary>
        /// Moves the node. Used when the node is being dragged with the mouse.
        /// </summary>
        /// <param name="delta">The amount by which to drag the node.</param>
        public void Drag(Vector2 delta)
        {
            _rect.position += delta;
            _typeIconRect.position += delta;
            _labelRect.position += delta;
        }

        /// <summary>
        /// Draws the node.
        /// </summary>
        public void Draw()
        {
            // Draw connection points.
            InPoint?.Draw(_rect);
            OutPoint?.Draw(_rect);

            // Draw the node itself.
            GUIStyle currentStyle = IsSelected ? _selectedStyle : _style;
            GUI.Box(_rect, "", currentStyle);
            GUI.Box(_typeIconRect, "", _typeIconStyle);


            if (Type == BbbtNodeType.Leaf)
            {
                GUI.Label(_labelRect, _actionLabel, currentStyle);
            }
        }

        /// <summary>
        /// Processes events.
        /// </summary>
        /// <param name="e">The events to be processed.</param>
        /// <returns>Returns true if the GUI should change, false otherwise.</returns>
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
                            // Clicked inside the rect, start dragging and highlight the node.
                            _isDragged = true;
                            IsSelected = true;

                            // Check if we double clicked the node (only leaf nodes for now).
                            if (Type == BbbtNodeType.Leaf &&
                                EditorApplication.timeSinceStartup - _lastClickTime < _doubleClickMaxTime)
                            {
                                // Go to the behaviour tree attached to the node if any.
                                if (_actionLabel != "")
                                {
                                    // Find and load the behaviour tree with the name of the associated action label.
                                    var guid = AssetDatabase.FindAssets(_actionLabel + " t:BbbtBehaviourTree")[0];
                                    var path = AssetDatabase.GUIDToAssetPath(guid);
                                    var asset = AssetDatabase.LoadAssetAtPath(path, typeof(BbbtBehaviourTree));
                                    var window = EditorWindow.GetWindow<BbbtWindow>();
                                    window.TreeToLoad = (BbbtBehaviourTree)asset;
                                    //AssetDatabase.OpenAsset(asset.GetInstanceID());
                                    _isDragged = false;
                                    return false;
                                }
                            }

                            _lastClickTime = EditorApplication.timeSinceStartup;
                            return true;
                        }
                        else if (IsSelected)
                        {
                            // Clicked outside, deselect the node.
                            IsSelected = false;
                            return true;
                        }
                    }

                    if (e.button == 1 && IsSelected && _rect.Contains(e.mousePosition))
                    {
                        CreateContextMenu();
                        e.Use();
                    }
                    break;

                // Mouse button released.
                case EventType.MouseUp:
                    _isDragged = false;
                    break;

                // Mouse moved.
                case EventType.MouseDrag:
                    // Is this node currently being dragged?
                    if (_isDragged)
                    {
                        // Node is being dragged, move with the mouse.
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
                // Something dropped into the editor window.
                case EventType.DragExited:
                    // Check that it's dropped into this node.
                    if (_rect.Contains(e.mousePosition))
                    {
                        // Check that this is a leaf node.
                        if (Type == BbbtNodeType.Leaf)
                        {
                            // Check if the asset being dropped is a tree.
                            var droppedTree = DragAndDrop.objectReferences[0] as BbbtBehaviourTree;

                            if (droppedTree != null)
                            {
                                GUI.changed = true;
                                IsSelected = true;
                                AttachBehaviourTree(droppedTree);
                                _actionLabel = droppedTree.name;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        IsSelected = false;
                        GUI.changed = true;
                    }
                    break;
            }

            // GUI did not change.
            return false;
        }

        /// <summary>
        /// Attached a behaviour tree to the leaf node.
        /// </summary>
        /// <param name="tree"></param>
        private void AttachBehaviourTree(BbbtBehaviourTree tree)
        {
            AttachedAction = tree;
        }

        /// <summary>
        /// Creates a context menu for actions related to the node.
        /// </summary>
        private void CreateContextMenu()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            menu.ShowAsContext();
        }

        /// <summary>
        /// Removes the node.
        /// </summary>
        private void OnClickRemoveNode()
        {
            _onClickRemoveNode?.Invoke(this);
        }

        /// <summary>
        /// Generates a BbbtNodeSaveData from this node.
        /// </summary>
        /// <returns>The node's save data.</returns>
        public BbbtNodeSaveData ToSaveData()
        {
            return new BbbtNodeSaveData(Id, Type.ToString(), _rect.x, _rect.y, IsSelected, _actionLabel);
        }
    }
}