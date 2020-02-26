using System;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Represents the connection between two BbbtNodes.
    /// </summary>
    public class BbbtConnection
    {
        /// <summary>
        /// The connection point leading that this connection leads in to.
        /// </summary>
        public BbbtConnectionPoint InPoint;

        /// <summary>
        /// The connection point that this connection leads out from.
        /// </summary>
        public BbbtConnectionPoint OutPoint;

        /// <summary>
        /// Action invoked when the button to remove the connection is clicked.
        /// </summary>
        private Action<BbbtConnection> _onClickRemoveConnection;

        /// <summary>
        /// Constructs a new BbbtConnection.
        /// </summary>
        /// <param name="inPoint">The point that the connection leads to.</param>
        /// <param name="outPoint">The point that the connection leads out from.</param>
        /// <param name="onClickRemoveConnection">
        /// The action invoked when the connection removal button is clicked.
        /// </param>
        public BbbtConnection(
            BbbtConnectionPoint inPoint,
            BbbtConnectionPoint outPoint,
            Action<BbbtConnection> onClickRemoveConnection)
        {
            InPoint = inPoint;
            OutPoint = outPoint;
            _onClickRemoveConnection = onClickRemoveConnection;
        }

        /// <summary>
        /// Draws the connection.
        /// </summary>
        public void Draw()
        {
            // Draw the connection
            Handles.DrawBezier(
                InPoint.Rect.center,
                OutPoint.Rect.center,
                InPoint.Rect.center - Vector2.up * 50f,
                OutPoint.Rect.center + Vector2.up * 50f,
                Color.white,
                null,
                2f
            );

            if (!Application.isPlaying)
            {
                // Draw the button to remove the connection.
                bool clicked = Handles.Button(
                    (InPoint.Rect.center + OutPoint.Rect.center) * 0.5f,
                    Quaternion.identity,
                    4,
                    8,
                    Handles.RectangleHandleCap
                );

                if (clicked)
                {
                    // Button was clicked.
                    _onClickRemoveConnection?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// Generates save data from this connection.
        /// </summary>
        /// <returns>The generated save data object.</returns>
        public BbbtConnectionSaveData ToSaveData()
        {
            return new BbbtConnectionSaveData(
                InPoint.Node.Id,
                OutPoint.Node.Id
            );
        }
    }
}