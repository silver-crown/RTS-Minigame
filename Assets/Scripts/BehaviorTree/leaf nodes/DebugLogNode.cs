using UnityEngine;

/// <summary>
/// Behaviour tree node which logs a message in the unity console.
/// </summary>
public class DebugLogNode : BbbtBehaviour
{
    /// <summary>
    /// The message to print to the console.
    /// </summary>
    [SerializeField] private string _message = "";

    /// <summary>
    /// Prints the message to the console.
    /// </summary>
    /// <returns>Always returns a success.</returns>
    protected override Status UpdateBehavior()
    {
        Debug.Log(_message);
        return Status.SUCCESS;
    }
}
