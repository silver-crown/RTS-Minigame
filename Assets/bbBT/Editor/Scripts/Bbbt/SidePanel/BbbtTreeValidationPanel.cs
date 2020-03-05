using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Side panel content used for validating the structure of the behaviour tree.
    /// </summary>
    public class BbbtTreeValidationPanel : BbbtSidePanelContent
    {
        /// <summary>
        /// The bbBT editor window.
        /// </summary>
        private BbbtWindow _window;

        /// <summary>
        /// Nodes to display warnings/errors for.
        /// </summary>
        private List<BbbtNode> _nodes;

        /// <summary>
        /// Warnings to display.
        /// </summary>
        private Dictionary<BbbtNode, List<string>> _warnings;

        /// <summary>
        /// Errors to display.
        /// </summary>
        private Dictionary<BbbtNode, List<string>> _errors;

        /// <summary>
        /// The style to use for headers.
        /// </summary>
        private GUIStyle _headerStyle;

        /// <summary>
        /// The style to use for warnings.
        /// </summary>
        private GUIStyle _warningStyle;

        /// <summary>
        /// The style to use for errors.
        /// </summary>
        private GUIStyle _errorStyle;

        /// <summary>
        /// Creates a new BbbtTreeValidationPanel instance.
        /// </summary>
        /// <param name="window">The bbBT editor window.</param>
        public BbbtTreeValidationPanel(BbbtWindow window)
        {
            _window = window;

            _headerStyle = new GUIStyle();
            _headerStyle.normal.textColor = Color.white;
            _headerStyle.fontSize = 14;

            _warningStyle = new GUIStyle();
            _warningStyle.normal.textColor = Color.yellow;
            _warningStyle.fontSize = 14;

            _errorStyle = new GUIStyle();
            _errorStyle.normal.textColor = Color.red;
            _errorStyle.fontSize = 14;
        }

        /// <summary>
        /// Validates the tree.
        /// </summary>
        private void Validate()
        {
            void AddNode(BbbtNode node)
            {
                if (!_nodes.Contains(node))
                {
                    if (node.Behaviour as BbbtRoot != null)
                    {
                        _nodes.Insert(0, node);
                    }
                    else
                    {
                        _nodes.Add(node);
                    }
                }
            }

            void AddError(BbbtNode node, string error)
            {
                AddNode(node);
                if (!_errors.ContainsKey(node))
                {
                    _errors[node] = new List<string>();
                }
                _errors[node].Add(error);
            }

            void AddWarning(BbbtNode node, string warning)
            {
                AddNode(node);
                if (!_warnings.ContainsKey(node))
                {
                    _warnings[node] = new List<string>();
                }
                _warnings[node].Add(warning);
            }

            _nodes = new List<BbbtNode>();
            _warnings = new Dictionary<BbbtNode, List<string>>();
            _errors = new Dictionary<BbbtNode, List<string>>();

            _window.CurrentTab.SetBehaviourChildren();

            // Find all the nodes and categorise them.
            BbbtRoot rootBehaviour = null;
            var nodesFromRoot = new List<BbbtNode>();
            var compositeNodes = new List<BbbtNode>();
            var decoratorNodes = new List<BbbtNode>();
            foreach (var node in _window.CurrentTab.Nodes)
            {
                if (node.Behaviour as BbbtRoot != null)
                {
                    rootBehaviour = node.Behaviour as BbbtRoot;
                    if (rootBehaviour.Child == null)
                    {
                        AddError(node, "Missing child.");
                    }
                    else
                    {
                        // Find all the nodes which come from the root node.
                        var nodesToVisit = new Stack<BbbtNode>();
                        nodesToVisit.Push(_window.CurrentTab.FindNodeWithId(rootBehaviour.Child.NodeId));
                        while (nodesToVisit.Count != 0)
                        {
                            var visitedNode = nodesToVisit.Pop();
                            nodesFromRoot.Add(visitedNode);
                            if (visitedNode.Behaviour as BbbtCompositeBehaviour != null)
                            {
                                var behaviour = visitedNode.Behaviour as BbbtCompositeBehaviour;
                                if (behaviour.Children != null)
                                {
                                    foreach (var child in behaviour.Children)
                                    {
                                        nodesToVisit.Push(_window.CurrentTab.FindNodeWithId(child.NodeId));
                                    }
                                }
                            }
                            else if (visitedNode.Behaviour as BbbtDecoratorBehaviour != null)
                            {
                                var behaviour = visitedNode.Behaviour as BbbtDecoratorBehaviour;
                                if (behaviour.Child != null)
                                {
                                    nodesToVisit.Push(_window.CurrentTab.FindNodeWithId(behaviour.NodeId));
                                }
                            }
                        }
                    }
                }

                if (node.Behaviour as BbbtDecoratorBehaviour)
                {
                    decoratorNodes.Add(node);
                }
                if (node.Behaviour as BbbtCompositeBehaviour)
                {
                    compositeNodes.Add(node);
                }

                // Check if the node has no parent (warning).
                if (node.Behaviour as BbbtRoot == null && _window.CurrentTab.FindParentNode(node) == null)
                {
                    AddWarning(node, "Missing parent.");
                }
            }

            /*
            // Check if there is no root node (error).
            if (rootBehaviour == null)
            {
                _errors[null].Add("No root node.");
            }
            */

            // Check if there are any childless composite nodes.
            foreach (var node in compositeNodes)
            {
                var behaviour = node.Behaviour as BbbtCompositeBehaviour;
                if (behaviour.Children == null)
                {
                    if (nodesFromRoot.Contains(node))
                    {
                        AddError(node, "Missing child.");
                    }
                    else
                    {
                        AddWarning(node, "Missing child.");
                    }
                }
            }

            // Check if there are any childless decorator nodes.
            foreach (var node in decoratorNodes)
            {
                var behaviour = node.Behaviour as BbbtDecoratorBehaviour;
                if (behaviour.Child == null)
                {
                    if (nodesFromRoot.Contains(node))
                    {
                        AddError(node, "Missing child.");
                    }
                    else
                    {
                        AddWarning(node, "Missing child.");
                    }
                }
            }
        }

        public override void Draw(Rect rect)
        {
            if (_window != null && _window.CurrentTab != null)
            {
                Validate();
                float usedVerticalSpace = 0.0f;
                float x = rect.x + 5.0f;
                float width = rect.xMax - x;

                // Display warnings and errors for each node.
                foreach (var node in _nodes)
                {
                    var headerRect = new Rect(x, usedVerticalSpace, width, 20.0f);
                    if (GUI.Button(headerRect, node.BaseBehaviour.name + " (node #" + node.Id + ")", _headerStyle))
                    {
                        _window.SelectNode(node);
                    }
                    usedVerticalSpace += 20.0f;
                    // Display errors.
                    if (_errors.ContainsKey(node))
                    {
                        foreach (var error in _errors[node])
                        {
                            var errorRect = new Rect(x + 5.0f, usedVerticalSpace, width - 5.0f, 20.0f);
                            if (GUI.Button(errorRect, error, _errorStyle))
                            {
                                _window.SelectNode(node);
                            }
                            usedVerticalSpace += 20.0f;
                        }
                    }

                    // Display warnings.
                    if (_warnings.ContainsKey(node))
                    {
                        foreach (var warning in _warnings[node])
                        {
                            var warningRect = new Rect(x + 5.0f, usedVerticalSpace, width - 5.0f, 20.0f);
                            if (GUI.Button(warningRect, warning, _warningStyle))
                            {
                                _window.SelectNode(node);
                            }
                            usedVerticalSpace += 20.0f;
                        }
                    }
                }
            }
        }
    }
}