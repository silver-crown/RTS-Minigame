using MoonSharp.Interpreter;
using RTS.UI.Debugging;
using UnityEngine;

namespace RTS.Debugging
{
    /// <summary>
    /// Used for displaying creating and logging debug messages in-game.
    /// </summary>
    [MoonSharpUserData]
    public class DebugLogger
    {
        private static DebugConsole _console;
        private static DebugConsole Console
        {
            get
            {
                if (_console == null)
                    _console = Object.FindObjectOfType<DebugConsole>();
                return _console;
            }
        }

        public static void Log(object message, object context = null)
        {
            var unityObject = context as Object;
            if (unityObject != null)
            {
                Debug.Log(message, unityObject);
            }
            Console.AddEntry(message, context);
        }
    }
}