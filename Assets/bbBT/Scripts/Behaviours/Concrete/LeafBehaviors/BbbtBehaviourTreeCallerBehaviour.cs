using Newtonsoft.Json;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Leaf node which calls another behaviour tree.
    /// </summary>
    [CreateAssetMenu(
        fileName = "Behaviour Tree Caller",
        menuName = "bbBT/Behaviour/Leaf/Behaviour Tree Caller",
        order = 0)]
    public class BbbtBehaviourTreeCallerBehaviour : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtBehaviourTreeCallerBehaviour";

        /// <summary>
        /// The name of the behaviour tree to call.
        /// </summary>
        [JsonProperty][SerializeField] private string _behaviourTreeName = null;

        /// <summary>
        /// The the behaviour tree to call.
        /// </summary>
        public BbbtBehaviourTree BehaviourTree { get; protected set; }

        /// <summary>
        /// The root of the behaviour tree to call. 
        /// </summary>
        private BbbtBehaviour _root = null;
        
        /// <summary>
        /// OnInitialize gets called the first time the behaviour is called.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override void OnInitialize(GameObject gameObject)
        {

            // so we can test networking remove this ifdef after we have new save system for bbbt
            if (_root == null && _behaviourTreeName != null)
            {
                var originalTree = BbbtBehaviourTree.FindBehaviourTreeWithName(_behaviourTreeName);
                BehaviourTree = Instantiate(originalTree);
                BehaviourTree.LoadSaveData(originalTree);
                BehaviourTree.name = _behaviourTreeName;
                _root = (BehaviourTree.RootBehaviour as BbbtRoot).Child;
                if (Application.isPlaying)
                {
                    BehaviourTree.name = BehaviourTree.name + " (" + gameObject.name + ")";
                }
            }
        }


        /// <summary>
        /// Doesn't have any termination logic.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        /// <summary>
        /// Ticks the root of the behaviour tree to be called.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        /// <returns>The status of the root node in the tree that was run.</returns>
        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            return _root.Tick(gameObject);
        }

        /*
        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>The generated save data.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            if (_behaviourTree != null)
            {
                return new BbbtBehaviourTreeCallerBehaviourSaveData(NodeId, _behaviourTree.name);
            }
            else
            {
                return new BbbtBehaviourTreeCallerBehaviourSaveData(NodeId, "");
            }   
        }

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public override void LoadSaveData(BbbtBehaviourSaveData saveData)
        {
            base.LoadSaveData(saveData);
            var castSaveData = saveData as BbbtBehaviourTreeCallerBehaviourSaveData;
            if (castSaveData != null)
            {
                string behaviourTreeName = castSaveData.BehaviourTreeName;
                _behaviourTree = BbbtBehaviourTree.FindBehaviourTreeWithName(behaviourTreeName);
                if (_behaviourTree != null)
                {
                    var copiedTree = Instantiate(_behaviourTree);
                    copiedTree.LoadSaveData(_behaviourTree);
                    copiedTree.name = _behaviourTree.name;
                    _root = (copiedTree.RootBehaviour as BbbtRoot).Child;
                    if (Application.isPlaying)
                    {
                        _behaviourTree = copiedTree;
                    }
                }
                else if (Application.isPlaying)
                {
                    Debug.LogWarning("Could not find behaviour tree '" + behaviourTreeName + "'.");
                }
            }
            else
            {
                Debug.LogError(
                    "Save data passed to BbbtBehaviourTreeCallerBehaviour was not " +
                    "BbbtBehaviourTreeCallerBehaviourSaveData."
                );
            }
        }
        */
    }
}