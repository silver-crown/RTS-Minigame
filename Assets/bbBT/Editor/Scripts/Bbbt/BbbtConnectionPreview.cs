using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Used for previewing a connection that is currently being created.
    /// </summary>
    public class BbbtConnectionPreview
    {
        /// <summary>
        /// The point from which to draw the preview.
        /// </summary>
        private BbbtConnectionPoint _point;


        /// <summary>
        /// Constructs a BbbtConnectionPreview.
        /// </summary>
        /// <param name="point">The point from which to draw the preview.</param>
        public BbbtConnectionPreview(BbbtConnectionPoint point)
        {
            _point = point;
        }

        /// <summary>
        /// Draws the connection point preview from its associated point to the cursor position.
        /// </summary>
        public void Draw()
        {
            // Draw the connection preview
            if (_point.Type == BbbtConnectionPointType.In)
            {
                Handles.DrawBezier(
                    _point.Rect.center,
                    Event.current.mousePosition,
                    _point.Rect.center - Vector2.up * 50f,
                    Event.current.mousePosition + Vector2.up * 50f,
                    Color.white,
                    null,
                    2f
                );
            }
            else
            {
                Handles.DrawBezier(
                       _point.Rect.center,
                       Event.current.mousePosition,
                       _point.Rect.center + Vector2.up * 50f,
                       Event.current.mousePosition - Vector2.up * 50f,
                       Color.white,
                       null,
                       2f
                   );
            }
            GUI.changed = true;
        }
    }
}
