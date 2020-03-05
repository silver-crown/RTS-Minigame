using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Help menu/manual displayed in the BbbtSidePanel.
    /// </summary>
    public class BbbtHelpMenu : BbbtSidePanelContent
    {
        /// <summary>
        /// The window the menu is in.
        /// </summary>
        private BbbtWindow _window;

        /// <summary>
        /// The sections of the help menu.
        /// </summary>
        private List<BbbtHelpSection> _sections;

        /// <summary>
        /// The help menu's button style.
        /// </summary>
        private GUIStyle _headerStyle;

        /// <summary>
        /// The help menu's content style.
        /// </summary>
        private GUIStyle _contentStyle;

        /// <summary>
        /// The style used for the borders between sections.
        /// </summary>
        private GUIStyle _borderStyle;

        /// <summary>
        /// The y offset for placing the menu's content. Determined by scrolling.
        /// </summary>
        private float _yOffset;

        /// <summary>
        /// How much vertical space was used last time the menu was drawn.
        /// </summary>
        private float _usedVerticalSpaceLastDraw = 0.0f;

        ///// <summary>
        ///// Texture for closed sections (indicating that they can be expanded).
        ///// </summary>
        //private Texture2D _downArrow;
        //
        ///// <summary>
        ///// Texture for open sections (indicating that they can be collapsed).
        ///// </summary>
        //private Texture2D _upArrow;


        /// <summary>
        /// Constructs a new BbbtHelpMenu.
        /// </summary>
        public BbbtHelpMenu(BbbtWindow window)
        {
            _window = window;
            // Set up styles
            _headerStyle = new GUIStyle();
            _headerStyle.richText = true;
            _headerStyle.normal.textColor = Color.white;
            _headerStyle.fontSize = 14;

            _contentStyle = new GUIStyle();
            _contentStyle.richText = true;
            _contentStyle.normal.textColor = Color.white;
            _contentStyle.fontSize = 12;
            _contentStyle.wordWrap = true;

            var background = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            background.SetPixel(1, 1, new Color(0.13f, 0.13f, 0.13f, 1.0f));
            background.Apply();
            _borderStyle = new GUIStyle();
            _borderStyle.normal.background = background;

            // Find the help .txt files and store their content.
            // Find the bbBT/Help folder. I'll hard code the location for now to make sure the system works.
            _sections = new List<BbbtHelpSection>();
            string folder = Path.Combine(Application.dataPath, "bbBT", "Editor", "Help");
            string[] filePaths = Directory.GetFiles(folder);
            foreach (string filePath in filePaths)
            {
                if (Path.GetExtension(filePath) == ".txt")
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    string content = File.ReadAllText(filePath);
                    _sections.Add(new BbbtHelpSection("<b>" + fileName + "</b>", content));
                }
            }

            //_downArrow = AssetDatabaseWrapper.FindTexture2D("DownArrow");
            //_upArrow = AssetDatabaseWrapper.FindTexture2D("UpArrow");
        }
        
        public override void Draw(Rect rect)
        {
            if (_usedVerticalSpaceLastDraw < _window.position.height)
            {
                _yOffset = 0.0f;
            }
            // For each section turn the key into a button which toggles its own content.
            float x = rect.x + 5.0f;
            float height = 20.0f;
            float width = rect.xMax - x - 5.0f;
            float usedVerticalSpace = _yOffset;
            foreach (var section in _sections)
            {
                // Header.
                GUI.Box(new Rect(rect.x, usedVerticalSpace, rect.width, 1.0f), "", _borderStyle);
                var buttonRect = new Rect(x, usedVerticalSpace, width, height);
                var header = new GUIContent(section.Header);
                var arrowRect = new Rect(buttonRect.xMax - 20.0f, usedVerticalSpace, 20.0f, 20.0f);

                // Truncate long text
                if (_headerStyle.CalcSize(header).x > width)
                {
                    header.text += "...";
                }

                while (_headerStyle.CalcSize(header).x > width)
                {
                    // -8: -5 to skip over </b>, then -3 to skip over ...
                    header.text = header.text.Remove(header.text.Length - 8, 1);
                }

                // Draw the header.
                if (GUI.Button(buttonRect, header, _headerStyle))
                {
                    section.IsActive = !section.IsActive;
                }
                usedVerticalSpace += height;
                GUI.Box(new Rect(rect.x, usedVerticalSpace, rect.width, 1.0f), "", _borderStyle);

                // Content
                if (section.IsActive)
                {
                    float contentHeight = _contentStyle.CalcHeight(new GUIContent(section.Content), width - 10.0f);
                    var contentRect = new Rect(x + 5.0f, usedVerticalSpace, width - 5.0f, contentHeight);
                    GUI.Label(contentRect, section.Content, _contentStyle);
                    usedVerticalSpace += contentHeight + 2.0f;
                }
            }
            GUI.Box(new Rect(rect.x, usedVerticalSpace, rect.width, 1.0f), "", _borderStyle);
            _usedVerticalSpaceLastDraw = usedVerticalSpace - _yOffset;
        }

        public override void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.ScrollWheel:
                    if (_usedVerticalSpaceLastDraw > _window.position.height)
                    {
                        _yOffset = Mathf.Clamp(
                            _yOffset - e.delta.y * 5.0f,
                            _window.position.height - _usedVerticalSpaceLastDraw,
                            0.0f
                        );
                        GUI.changed = true;
                        e.Use();
                    }
                    break;
            }
        }
    }
}