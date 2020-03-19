using MoonSharp.Interpreter;
using RTS.UI.Debugging;
using UnityEngine;

/// <summary>
/// Used for displaying creating and logging debug messages in-game.
/// </summary>
[MoonSharpUserData]
public class InGameDebug
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
        else
        {
            Debug.Log(message);
        }
        DebugConsole.StaticAddEntry(message, context);
    }
}