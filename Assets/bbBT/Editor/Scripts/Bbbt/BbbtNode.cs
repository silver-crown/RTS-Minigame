using Bbbt.Commands;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
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
        /// The behaviour used as a base for the node's behaviour.
        /// </summary>
        public BbbtBehaviour BaseBehaviour { get; protected set; }

        /// <summary>
        /// The behaviour instance specific to this node.
        /// </summary>
        public BbbtBehaviour Behaviour { get; set; }

        /// <summary>
        /// The node's rect.
        /// </summary>
        public Rect Rect { get; protected set; }

        /// <summary>
        /// The tab the node belongs to.
        /// </summary>
        public BbbtWindowTab Tab { get; protected set; }

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
        /// The position on which we started dragging the node.
        /// </summary>
        private Vector2 _dragStartPosition;

        /// <summary>
        /// Whether the node is currently selected.
        /// </summary>
        public bool IsSelected { get; set; } = false;

        /// <summary>
        /// Action invoked when the node is removed.
        /// </summary>
        private Action<BbbtNode> _onClickRemoveNode;


        /// <summary>
        /// Sets up a new BbbtNode.
        /// </summary>
        /// <param name="id">The node's id.</param>
        /// <param name="baseBehaviour">The behaviour to use as a template for the node's exported behaviour.</param>
        /// <param name="tab">The tab the node belongs to.</param>
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
        /// <param name="isSelected">Whether the node should start out selected.</param>
        /*/// <param name="behaviourSaveData">
        /// The save data associated used to reconstruct the behaviour of the node if loading the node from file.
        /// </param>*/
        public void Setup(
            int id,
            BbbtBehaviour baseBehaviour,
            Vector2 position,
            BbbtWindowTab tab,
            float width,
            float height,
            GUIStyle style,
            GUIStyle selectedStyle,
            GUIStyle inPointStyle,
            GUIStyle outPointStyle,
            Action<BbbtConnectionPoint> inPointOnClick,
            Action<BbbtConnectionPoint> outPointOnClick,
            Action<BbbtNode> onClickRemoveNode,
            bool isSelected = false,
            //BbbtBehaviourSaveData behaviourSaveData = null
            BbbtBehaviour behaviour = null)
        {
            Id = id;
            Tab = tab;
            _style = style;
            _selectedStyle = selectedStyle;
            BaseBehaviour = baseBehaviour;

            // Create a copy of the provided behaviour so that we don't write changes to it if we are not playing
            if (!Application.isPlaying)
            {
                Behaviour = Instantiate(BaseBehaviour);
                if (/*behaviourSaveData*/behaviour != null)
                {
                    Behaviour = behaviour;
                    //Behaviour.LoadSaveData(behaviourSaveData);
                }
            }

            Rect = new Rect(position.x, position.y, width, height);


            // Set up the node type icon
            _typeIconStyle = new GUIStyle();
            _typeIconStyle.normal.background = AssetDatabase.LoadAssetAtPath<Texture2D>(
                Path.Combine("Assets", "bbBT", "Editor", "Textures", BaseBehaviour.name + ".png")
            );

            // Create the rects, make them some reasonable size.
            _typeIconRect = new Rect(
                Rect.position + Rect.size / 4.0f,
                Rect.size / 2.0f
            );

            Vector2 labelSize = new GUIStyle("Tooltip").CalcSize(new GUIContent(BaseBehaviour.name));

            _labelRect = new Rect(
                new Vector2(Rect.center.x - labelSize.x / 2.0f, Rect.yMax + 10.0f),
                labelSize
            );

            // Create connectors.
            // Make the InPoint (but not for root).
            if (BaseBehaviour as BbbtRoot == null)
            {
                InPoint = new BbbtConnectionPoint(this, BbbtConnectionPointType.In, inPointStyle, inPointOnClick);
            }
            // Make the InPoint (but not for leaves).
            if (BaseBehaviour as BbbtLeafBehaviour == null)
            {
                OutPoint = new BbbtConnectionPoint(this, BbbtConnectionPointType.Out, outPointStyle, outPointOnClick);
            }

            _onClickRemoveNode = onClickRemoveNode;
            IsSelected = isSelected;
        }

        /// <summary>
        /// Moves the node. Used when the node is being dragged with the mouse.
        /// </summary>
        /// <param name="delta">The amount by which to drag the node.</param>
        public void Drag(Vector2 delta)
        {
            Rect = new Rect(Rect.position + delta, Rect.size);
            _typeIconRect.position += delta;
            _labelRect.position += delta;
        }

        /// <summary>
        /// Draws the node.
        /// </summary>
        public void Draw()
        {
            // Draw connection points.
            InPoint?.Draw(Rect);
            OutPoint?.Draw(Rect);

            // Draw the node itself.
            GUIStyle currentStyle = IsSelected ? _selectedStyle : _style;
            GUI.Box(Rect, new GUIContent("", BaseBehaviour.name), currentStyle);
            GUI.Box(_typeIconRect, "", _typeIconStyle);
        }

        /// <summary>
        /// Draws the node's label undearneath the node.
        /// </summary>
        public void DrawLabel()
        {
            GUI.Box(_labelRect, BaseBehaviour.name, "Tooltip");
        }

        /// <summary>
        /// Processes events.
        /// </summary>
        /// <param name="e">The events to be processed.</param>
        /// <returns>Returns true if the GUI should change, false otherwise.</returns>
        public bool ProcessEvents(BbbtWindow window, Event e)
        {
            switch (e.type)
            {
                // Mouse button was just pressed.
                case EventType.MouseDown:
                    // LMB
                    if (e.button == 0)
                    {
                        // Are we clicking inside the rect?
                        if (Rect.Contains(e.mousePosition))
                        {
                            // Clicked inside the rect, start dragging and highlight the node.
                            if (!Application.isPlaying)
                            {
                                _isDragged = true;
                                _dragStartPosition = Rect.position;
                            }
                            IsSelected = true;

                            return true;
                        }
                        else if (IsSelected)
                        {
                            // Clicked outside, deselect the node.
                            IsSelected = false;
                            return true;
                        }
                    }

                    if (e.button == 1 && IsSelected && Rect.Contains(e.mousePosition))
                    {
                        if (!Application.isPlaying)
                        {
                            CreateContextMenu();
                            e.Use();
                        }
                    }
                    break;

                // Mouse button released.
                case EventType.MouseUp:
                    if (_isDragged && Rect.position != _dragStartPosition)
                    {
                        var position = Rect.position;
                        window.CurrentTab.CommandManager.Do(
                            new MoveNodeCommand(window, this, position - _dragStartPosition)
                        );
                        Drag(_dragStartPosition - position);
                    }
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
            }

            // GUI did not change.
            return false;
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
            return new BbbtNodeSaveData(
                Id,
                BaseBehaviour.name,
                Behaviour,//.ToSaveData(),
                Rect.x,
                Rect.y,
                IsSelected
            );
        }
    }
}