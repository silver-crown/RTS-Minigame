using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    public enum BbbtPromptProcessEventStatus { NoChange, Changed, Finished }

    /// <summary>
    /// A pop-up window with some text where you have to push a button to get rid of it.
    /// Usa
    /// </summary>
    public class BbbtPrompt
    {
        /// <summary>
        /// The prompt's rect.
        /// </summary>
        private Rect _rect;

        /// <summary>
        /// The text to display in the prompt.
        /// </summary>
        private string _text;

        /// <summary>
        /// The options available for selection in the prompt.
        /// </summary>
        private List<BbbtPromptOption> _options;

        /// <summary>
        /// The window the prompt should be displayed in.
        /// </summary>
        private BbbtWindow _window;


        /// <summary>
        /// Constructs a new BbbtPrompt.
        /// </summary>
        /// <param name="text">The text to be displayed in the prompt.</param>
        /// <param name="options">The options available for selection in the prompt.</param>
        public BbbtPrompt(string text, List<BbbtPromptOption> options)
        {
            _text = text;
            _options = options;

            _rect = new Rect(0, 0, 400, 200);

            _window = EditorWindow.GetWindow<BbbtWindow>();
        }

        /// <summary>
        /// Draws the prompt to the screen.
        /// </summary>
        /// <returns>True if one of the buttons was pressed, false otherwise.</returns>
        public bool Draw()
        {
            // Figure out the position of the rect based on the window's size. We need to do this every draw
            // since the window size could change.
            _rect.x = _window.position.width / 2.0f - _rect.width / 2.0f;
            _rect.y = _window.position.height / 2.0f - _rect.height / 2.0f;

            // Draw the rect.
            GUI.Box(_rect, _text);
            
            // Draw the options in the bottom left of the pop-up.
            for (int i = _options.Count - 1; i >= 0; i--)
            {
                // Find the position of the button.
                Vector2 position = new Vector2(
                    _rect.x + _rect.width - 5.0f,
                    _rect.y + _rect.height - 35.0f
                );

                for (int j = _options.Count - 1; j >= i; j--)
                {
                    position.x -= _options[j].Rect.width + 5.0f;
                }

                // Draw the button and check if it was clicked.
                if (_options[i].Draw(position))
                {
                    return true;
                }
            }
            
            // No button clicked.
            return false;
        }
    }
}