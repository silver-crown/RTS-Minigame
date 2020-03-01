using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Help menu/manual displayed in the BbbtSidePanel.
    /// </summary>
    public class BbbtHelpMenu : BbbtSidePanelContent
    {
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
        /// Constructs a new BbbtHelpMenu.
        /// </summary>
        public BbbtHelpMenu()
        {
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
        }
        
        
        public override void Draw(Rect rect)
        {
            // For each section turn the key into a button which toggles its own content.
            float x = rect.x + 5.0f;
            float height = 20.0f;
            float width = rect.xMax - x - 5.0f;
            float usedVerticalSpace = 0.0f;
            foreach (var section in _sections)
            {
                var buttonRect = new Rect(x, usedVerticalSpace, width, height);
                if (GUI.Button(buttonRect, section.Header, _headerStyle))
                {
                    section.IsActive = !section.IsActive;
                }
                usedVerticalSpace += height;

                if (section.IsActive)
                {
                    float contentHeight = _contentStyle.CalcHeight(new GUIContent(section.Content), width);
                    var contentRect = new Rect(x, usedVerticalSpace, width, contentHeight);
                    GUI.Label(contentRect, section.Content, _contentStyle);
                    usedVerticalSpace += contentHeight;
                }
                usedVerticalSpace += 2.0f;
            }
        }
    }
}