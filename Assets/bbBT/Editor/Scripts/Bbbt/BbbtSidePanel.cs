using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// The side panel shown on the left side of the bbBT editor window.
    /// </summary>
    public class BbbtSidePanel
    {
        /// <summary>
        /// The window the panel belongs to.
        /// </summary>
        private BbbtWindow _window;

        /// <summary>
        /// The panel's rect.
        /// </summary>
        private Rect _rect;

        /// <summary>
        /// The rect of the panel's border.
        /// </summary>
        private Rect _borderRect;

        /// <summary>
        /// The panel's style.
        /// </summary>
        private GUIStyle _style;

        /// <summary>
        /// The panel's border style.
        /// </summary>
        private GUIStyle _borderStyle;


        /// <summary>
        /// Constructs a side panel.
        /// </summary>
        /// <param name="window">The window the panel belongs to.</param>
        public BbbtSidePanel(BbbtWindow window)
        {
            // Assign variables.
            _window = window;

            // Set up style of the panel.
            _rect = new Rect(0, 0, 170, window.position.height);

            var background = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            background.SetPixel(0, 0, new Color(0.16f, 0.16f, 0.16f, 1.0f));
            background.Apply();

            _style = new GUIStyle();
            _style.normal.background = background;

            // Set up style of the border.
            _borderRect = new Rect(_rect.width, 0, 1, window.position.height);

            var borderBackground = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            borderBackground.SetPixel(0, 0, new Color(0.12f, 0.12f, 0.12f, 1.0f));
            borderBackground.Apply();

            _borderStyle = new GUIStyle();
            _borderStyle.normal.background = borderBackground;
        }

        /// <summary>
        /// Draws the panel.
        /// </summary>
        public void Draw()
        {
            // Draw the background/border of the panel.
            _rect.height = _window.position.height;
            GUI.Box(_rect, "", _style);
            _borderRect.height = _rect.height;
            GUI.Box(_borderRect, "", _borderStyle);

            // Draw the command history of the currently open tab.
            if (_window.CurrentTab != null)
            {
                _window.CurrentTab.CommandHistoryBrowser.Draw(_rect);
            }
        }

        /// <summary>
        /// Returns the total width of the panel including border.
        /// </summary>
        /// <returns>The total width of the panel including border</returns>
        public float GetTotalWidth()
        {
            return _rect.width + _borderRect.width;
        }
    }
}