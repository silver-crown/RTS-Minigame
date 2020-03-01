using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Content that goes into the BbbtSidePanel.
    /// </summary>
    public abstract class BbbtSidePanelContent
    {
        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="rect">The rect to draw inside.</param>
        public abstract void Draw(Rect rect);
    }
}