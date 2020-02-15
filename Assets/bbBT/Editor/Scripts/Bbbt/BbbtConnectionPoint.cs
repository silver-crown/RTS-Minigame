using System;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// The type a BbbtConnectionPoint can be of. 
    /// <b>In</b>: A point that leads into a node. 
    /// <b>Out</b>: A point that leads from a node. Leaf nodes have no out connector.
    /// </summary>
    public enum BbbtConnectionPointType { In, Out }

    /// <summary>
    /// A point on a BbbtNode which can be used to connect it to another node.
    /// </summary>
    public class BbbtConnectionPoint
    {
        /// <summary>
        /// The connection point's rect.
        /// </summary>
        public Rect Rect;

        /// <summary>
        /// The node to which the connection point belongs.
        /// </summary>
        public BbbtNode Node;

        /// <summary>
        /// The type (in or out) of the connection point.
        /// </summary>
        public BbbtConnectionPointType Type;

        /// <summary>
        /// The connection point's GUIStyle.
        /// </summary>
        private GUIStyle _style;

        /// <summary>
        /// Action invoked when the connection point is clicked on.
        /// </summary>
        private Action<BbbtConnectionPoint> _onClick;


        /// <summary>
        /// Constructs a BbbtConnectionPoint.
        /// </summary>
        /// <param name="node">The node to which the connection point belongs.</param>
        /// <param name="type">The type (in or out) of the connection point.</param>
        /// <param name="style">The connection point's GUIStyle.</param>
        /// <param name="onClick">The callback method to be used when the point is clicked on.</param>
        public BbbtConnectionPoint(
            BbbtNode node,
            BbbtConnectionPointType type,
            GUIStyle style,
            Action<BbbtConnectionPoint> onClick)
        {
            Node = node;
            Type = type;
            _style = style;
            _onClick = onClick;
            Rect = new Rect(0, 0, 20f, 20f);
        }

        /// <summary>
        /// Draws the connection point.
        /// </summary>
        /// <param name="_nodeRect">The rect of the node to which the connection point belongs.</param>
        public void Draw(Rect _nodeRect)
        {
            // Set the connection point's x position centered on the node's x position.
            Rect.x = _nodeRect.x + _nodeRect.width * 0.5f - Rect.width * 0.5f;
            
            // We need to place the connection point differently depending on its type.
            switch (Type)
            {
                // In connection
                case BbbtConnectionPointType.In:
                    // Place above the node. The + xf is to make it look connected.
                    Rect.y = _nodeRect.y - Rect.height + 13.0f;
                    break;
                /// Out connection
                case BbbtConnectionPointType.Out:
                    // Place underneath the node. The - xf is to make it look connected.
                    Rect.y = _nodeRect.y + _nodeRect.height - 13.0f;
                    break;
            }

            // Draw the point and check if it was pressed.
            if (GUI.Button(Rect, "", _style))
            {
                // Button was pressed.
                _onClick?.Invoke(this);
            }
        }

        /// <summary>
        /// Moves the connection point. Should only be called from BbbtNode.Draw().
        /// </summary>
        /// <param name="delta">The amount by which to drag the point.</param>
        public void Drag(Vector2 delta)
        {
            Rect.position += delta;
        }
    }
}