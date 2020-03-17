using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI.Debugging
{
    /// <summary>
    /// And entry in the in-game DebugConsole.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class DebugConsoleEntry : MonoBehaviour
    {
        private Text _text;
        public string Message { get; set; }
        public object Context { get; set; }

        private void Awake()
        {
            _text = GetComponent<Text>();
            if (_text == null)
            {
                Debug.LogError(
                    GetType().Name + ": _text was null." +
                    "Make sure the DebugConsoleEntry prefab has a text component.", this);
            }
        }

        private void Start()
        {
            if (Message == null)
            {
                Debug.LogError(
                    GetType().Name + ": Message was null. Set Message immediately after instantiating.",
                    this);
                return;
            }
            _text.text = Message;
            Canvas.ForceUpdateCanvases();
        }
    }
}