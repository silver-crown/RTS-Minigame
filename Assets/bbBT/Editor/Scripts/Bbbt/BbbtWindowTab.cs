using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// A single tab to be displayed in the BbbtWindow.
    /// </summary>
    public class BbbtWindowTab
    {
        /// <summary>
        /// The tab's rect.
        /// </summary>
        private Rect _rect;

        /// <summary>
        /// The id of the last created node.
        /// </summary>
        public int LastNodeID;

        /// <summary>
        /// The behaviour tree currently loaded into the editor window.
        /// </summary>
        public BbbtBehaviourTree Tree;

        /// <summary>
        /// The nodes that make up the behaviour tree.
        /// </summary>
        public List<BbbtNode> Nodes;

        /// <summary>
        /// The current connections between nodes.
        /// </summary>
        public List<BbbtConnection> Connections;

        /// <summary>
        /// The amount by which the entire window has been dragged from its initial position.
        /// </summary>
        public Vector3 WindowOffset = Vector3.zero;

        /// <summary>
        /// Whether the tab has unsaved changes.
        /// </summary>
        public bool IsUnsaved = false;

        /// <summary>
        /// The style of the tab.
        /// </summary>
        private GUIStyle _style;


        /// <summary>
        /// Instantiates a new BbbtWindowTab.
        /// </summary>
        /// <param name="tree">The tree to use in the tab.</param>
        /// <param name="style">The style of the tab.</param>
        public BbbtWindowTab(BbbtBehaviourTree tree, GUIStyle style)
        {
            Nodes = new List<BbbtNode>();
            Connections = new List<BbbtConnection>();

            Tree = tree;
            //_style = style;
            LastNodeID = 0;

            // Calculated the size needed to fit the tree's name in the tab.
            _rect.size = new GUIStyle("PreButton").CalcSize(new GUIContent(tree.name + "*"));
        }


        /// <summary>
        /// Draws the tab.
        /// </summary>
        /// <param name="tabs">The full list of tabs to be drawn.</param>
        /// <param name="index">The tab's index in the editor window, used to determine the tab's position.</param>
        public void Draw(List<BbbtWindowTab> tabs, int index)
        {
            // Figure out the rect's position by adding together the widths of the preceding tabs.
            _rect.position = new Vector2(5, 2);
            foreach (var tab in tabs)
            {
                if (tab == this)
                {
                    break;
                }
                _rect.position += Vector2.right * tab._rect.width;
            }
            string label = IsUnsaved ? Tree.name : Tree.name + "*";
            GUI.Box(_rect, Tree.name + "*", "PreButton");
        }
    }
}