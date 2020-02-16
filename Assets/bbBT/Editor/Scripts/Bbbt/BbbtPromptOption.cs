using System;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// A BbbtPromptOption is one of the buttons that the user can click when faced with a prompt in the bbBT editor.
    /// </summary>
    public class BbbtPromptOption
    {
        /// <summary>
        /// The rect for drawing the prompt's button.
        /// </summary>
        public Rect Rect { get; protected set; }

        /// <summary>
        /// The text to display in the button.
        /// </summary>
        private string _text;

        /// <summary>
        /// Action invoked when the button is clicked.
        /// </summary>
        private Action _onClick;


        /// <summary>
        /// Constructs a new BbbtPromptOption.
        /// </summary>
        /// <param name="text">The text to display in the option's button.</param>
        /// <param name="onClick">The action to perform when the button is clicked.</param>
        public BbbtPromptOption(string text, Action onClick)
        {
            _text = text;
            _onClick = onClick;

            Rect = new Rect(Vector2.zero, new GUIStyle("LargeButton").CalcSize(new GUIContent(_text)));
        }

        /// <summary>
        /// Draws the button to the screen.
        /// </summary>
        /// <param name="position">The position of the button.</param>
        /// <returns>True if the button was clicked, false otherwise.</returns>
        public bool Draw(Vector2 position)
        {
            // Position the button properly
            Rect = new Rect(position, Rect.size);

            // Draw the button and check if it was clicked.
            if (GUI.Button(Rect, _text, "LargeButton"))
            {
                // Button was clicked.
                _onClick?.Invoke();
                return true;
            }

            // Button was not clicked.
            return false;
        }
    }
}