using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Leaf node which prints a message to the Unity console.
    /// </summary>
    [CreateAssetMenu(fileName = "Debug Logger", menuName = "bbBT/Behaviour/Leaf/Debug Logger", order = 0)]
    public class BbbtDebugLogBehaviour : BbbtLeafBehaviour
    {
        /// <summary>
        /// The message to print to log to the Unity console.
        /// </summary>
        [SerializeField] private string _message = "";

        /// <summary>
        /// The type of message to log.
        /// </summary>
        [SerializeField] private LogType _logType = LogType.Log;


        /// <summary>
        /// BbbtRoot doesn't have any initialisation logic.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override void OnInitialize(GameObject gameObject)
        {
        }

        /// <summary>
        /// BbbtRoot doesn't have any termination logic.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        /// <summary>
        /// Prints the message the debug logger is supposed to print.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override BbbtBehaviourStatus UpdateBehavior(GameObject gameObject)
        {
            switch (_logType)
            {
                case LogType.Error:
                    Debug.LogError(_message);
                    break;
                case LogType.Assert:
                    Debug.LogAssertion(_message);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(_message);
                    break;
                case LogType.Log:
                    Debug.Log(_message);
                    break;
                case LogType.Exception:
                    Debug.LogException(new System.Exception(_message));
                    break;
            }
            return BbbtBehaviourStatus.Success;
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>The generated save data.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            return new BbbtDebugLogBehaviourSaveData(NodeId, _message, _logType);
        }

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public override void LoadSaveData(BbbtBehaviourSaveData saveData)
        {
            base.LoadSaveData(saveData);
            var castSaveData = saveData as BbbtDebugLogBehaviourSaveData;
            if (castSaveData != null)
            {
                _message = castSaveData.Message;
                _logType = castSaveData.LogType;
            }
            else
            {
                Debug.LogError("Save data passed to BbbtDebugLogBehaviour was not BbbtDebugLogBehaviourSaveData.");
            }
        }
    }
}