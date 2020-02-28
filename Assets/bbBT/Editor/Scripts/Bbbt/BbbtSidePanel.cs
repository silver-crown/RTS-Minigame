using System.Collections.Generic;
using UnityEditor;
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
        private Rect _panelRect;

        /// <summary>
        /// The rect for the navigation bar to the very left of the window.
        /// </summary>
        private Rect _navigationBarRect;

        /// <summary>
        ///  The rect of the navigation bar's border.
        /// </summary>
        private Rect _navigationBarBorderRect;

        /// <summary>
        /// The rect of the panel's border.
        /// </summary>
        private Rect _panelBorderRect;

        /// <summary>
        /// The rect that defines the area where the mouse can drag the panel.
        /// </summary>
        private Rect _dragAreaRect;

        /// <summary>
        /// The panel's style.
        /// </summary>
        private GUIStyle _style;

        /// <summary>
        /// The panel's border style.
        /// </summary>
        private GUIStyle _borderStyle;

        /// <summary>
        /// Whether the panel is currently being dragged (resized).
        /// </summary>
        private bool _isDragged = false;

        /// <summary>
        /// Maps a button in the navigation rect to content in the side panel for opening/closing the content.
        /// </summary>
        private List<KeyValuePair<Texture2D, BbbtSidePanelContent>> _textureToContent = null;

        /// <summary>
        /// The currently selected content to display in the panel.
        /// </summary>
        private BbbtSidePanelContent _selectedContent = null;


        /// <summary>
        /// Constructs a side panel.
        /// </summary>
        /// <param name="window">The window the panel belongs to.</param>
        public BbbtSidePanel(BbbtWindow window)
        {
            // Assign variables.
            _window = window;

            // Set up rects
            _navigationBarRect = new Rect(0, 0, 48, window.position.height);
            SetupRects(170.0f);

            // Set up style of the panel.
            var background = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            background.SetPixel(0, 0, new Color(0.16f, 0.16f, 0.16f, 1.0f));
            background.Apply();

            _style = new GUIStyle();
            _style.normal.background = background;

            // Set up style of the border.
            var borderBackground = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            borderBackground.SetPixel(0, 0, new Color(0.12f, 0.12f, 0.12f, 1.0f));
            borderBackground.Apply();

            _borderStyle = new GUIStyle();
            _borderStyle.normal.background = borderBackground;

            // Populate the panel with content.
            // TODO: Rewrite this in a more reusable and less ugly way.
            var guid = AssetDatabase.FindAssets("CommandHistoryBrowserIcon t:texture2d")[0];
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var commandBrowserIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            _textureToContent = new List<KeyValuePair<Texture2D, BbbtSidePanelContent>>()
            {
                new KeyValuePair<Texture2D, BbbtSidePanelContent>(
                    commandBrowserIcon,
                    new BbbtCommandHistoryBrowser(window)
                )
            };
        }

        /// <summary>
        /// Draws the panel.
        /// </summary>
        public void Draw()
        {
            _navigationBarRect.height = _window.position.height - _navigationBarRect.y;
            SetupRects(_panelRect.width);

            // Draw the navigation bar with border.
            GUI.Box(_navigationBarRect, "", _style);
            GUI.Box(_navigationBarBorderRect, "", _borderStyle);

            // Draw the buttons in the navigation bar.
            for (int i = 0; i < _textureToContent.Count; i++)
            {
                Rect rect = new Rect(
                    _navigationBarRect.position + Vector2.down * i * _navigationBarRect.width,
                    Vector2.one * _navigationBarRect.width
                );
                if (GUI.Button(rect, _textureToContent[i].Key, _style))
                {
                    if (_selectedContent != _textureToContent[i].Value)
                    {
                        _selectedContent = _textureToContent[i].Value;
                    }
                    else
                    {
                        _selectedContent = null;
                    }
                }
            }

            if (_selectedContent != null)
            {

                // Draw the background/border of the panel.
                GUI.Box(_panelRect, "", _style);
                GUI.Box(_panelBorderRect, "", _borderStyle);

                // Display a horizontal resize cursor over the drag area.
                if (_isDragged)
                {
                    // If we're dragging, display it no matter where in the window we are.
                    EditorGUIUtility.AddCursorRect(
                        new Rect(Vector2.zero, Vector2.one * 99999.0f),
                        MouseCursor.ResizeHorizontal
                    );
                }
                else
                {
                    // Not dragging, only display it when over the drag area.
                    EditorGUIUtility.AddCursorRect(_dragAreaRect, MouseCursor.ResizeHorizontal);
                }

                // Draw the content of the panel.
                _selectedContent.Draw(_panelRect);
            }
        }

        /// <summary>
        /// Sets up rects according to the width of the navigation bar.
        /// </summary>
        /// <param name="width">The width of the main panel.</param>
        private void SetupRects(float width)
        {
            _navigationBarBorderRect = new Rect(
                _navigationBarRect.xMax,
                _navigationBarRect.y,
                1,
                _navigationBarRect.height
            );
            _panelRect = new Rect(
                _navigationBarBorderRect.xMax,
                _navigationBarRect.y,
                width,
                _navigationBarRect.height
            );
            _panelBorderRect = new Rect(
                _panelRect.xMax,
                _navigationBarRect.y,
                1,
                _navigationBarRect.height
            );
            _dragAreaRect = new Rect(
                _panelBorderRect.x - 1.0f,
                _panelBorderRect.y,
                _panelBorderRect.width + 2.0f,
                _panelBorderRect.height
            );
        }

        /// <summary>
        /// Drags the panel.
        /// </summary>
        /// <param name="delta">The amount to drag the panel by.</param>
        private void Drag(float delta)
        {
            SetupRects(Mathf.Clamp(_panelRect.width + delta, 0.0f, _window.position.width));
        }

        /// <summary>
        /// Processes events.
        /// </summary>
        /// <param name="e">The events to be handled.</param>
        public void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    // LMB down.
                    if (e.button == 0)
                    {
                        // Check if the mouse is within the area where the panel can be dragged.
                        if (_dragAreaRect.Contains(e.mousePosition))
                        {
                            // Start dragging the panel.
                            _isDragged = true;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    // LMB up.
                    if (e.button == 0)
                    {
                        // Stop dragging
                        _isDragged = false;
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseDrag:
                    if (_isDragged)
                    {
                        // Drag the panel by the amount the mouse moved.
                        Drag(e.delta.x);
                        GUI.changed = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns the total width of the panel including border.
        /// </summary>
        /// <returns>The total width of the panel including border</returns>
        public float GetTotalWidth()
        {
            float width = _navigationBarRect.width + _navigationBarBorderRect.width;
            if (_selectedContent != null)
            {
                width += _panelRect.width + _panelBorderRect.width;
            }
            return width;
        }
    }
}