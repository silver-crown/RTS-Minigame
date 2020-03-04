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
        /// Warnings to display.
        /// </summary>
        private List<string> _warnings;

        /// <summary>
        /// Errors to display.
        /// </summary>
        private List<string> _errors;

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
            _warnings = new List<string>();
            _errors = new List<string>();

            _window.CurrentTab.SetBehaviourChildren();

            // For each behaviour in the tree, validate the behaviour and display any warnings or errors.
            BbbtRoot rootBehaviour = null;
            foreach (var node in _window.CurrentTab.Nodes)
            {
                if (node.Behaviour as BbbtRoot != null)
                {
                    rootBehaviour = node.Behaviour as BbbtRoot;
                    if (rootBehaviour.Child == null)
                    {
                        _errors.Add("Root node has no child.");
                    }
                }

                // Check if the node's behaviour has any children.
                var decorator = node.Behaviour as BbbtDecoratorBehaviour;
                var composite = node.Behaviour as BbbtCompositeBehaviour;
                if (decorator != null && decorator.Child == null || composite != null && composite.Children == null)
                {
                    _warnings.Add(node.BaseBehaviour.name + " (node #" + node.Id + ") has no child.");
                }
            }

            // Check if there is no root node.
            if (rootBehaviour == null)
            {
                _errors.Add("No root node.");
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

                // Display errors.
                foreach (var error in _errors)
                {
                    var errorRect = new Rect(x, usedVerticalSpace, width, 20.0f);
                    GUI.Label(errorRect, error, _errorStyle);
                    usedVerticalSpace += 20.0f;
                }

                // Display warnings.
                foreach (var warning in _warnings)
                {
                    var warningRect = new Rect(x, usedVerticalSpace, width, 20.0f);
                    GUI.Label(warningRect, warning, _warningStyle);
                    usedVerticalSpace += 20.0f;
                }
            }
        }
    }
}