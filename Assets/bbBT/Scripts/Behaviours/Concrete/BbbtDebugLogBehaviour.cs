using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Marks the entry point of a behaviour tree. Inherits from BbbtBehaviour for convenience but isn't actually
    /// included in a behaviour tree, and is used in the editor to point to the real root node.
    /// </summary>
    [CreateAssetMenu(fileName = "Debug Logger", menuName = "bbBT/Behaviour/Leaf/Debug Logger", order = 0)]
    public class BbbtDebugLogBehaviour : BbbtLeafBehaviour
    {
        /// <summary>
        /// The message to print to log to the Unity console.
        /// </summary>
        [SerializeField] private string _message = "";


        /// <summary>
        /// BbbtRoot doesn't have any initialisation logic.
        /// </summary>
        protected override void OnInitialize()
        {
        }

        /// <summary>
        /// BbbtRoot doesn't have any termination logic.
        /// </summary>
        protected override void OnTerminate(BbbtBehaviorStatus status)
        {
        }

        /// <summary>
        /// Prints the message the debug logger is supposed to print.
        /// </summary>
        protected override BbbtBehaviorStatus UpdateBehavior()
        {
            Debug.Log(_message);
            return BbbtBehaviorStatus.Success;
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>The generated save data.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            return new BbbtDebugLogBehaviourSaveData(_message);
        }

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public override void LoadSaveData(BbbtBehaviourSaveData saveData)
        {
            var castSaveData = saveData as BbbtDebugLogBehaviourSaveData;
            if (castSaveData != null)
            {
                _message = castSaveData.Message;
            }
            else
            {
                Debug.LogError("Save data passed to BbbtDebugLogBehaviour was not BbbtDebugLogBehaviourSaveData.");
            }
        }
    }
}