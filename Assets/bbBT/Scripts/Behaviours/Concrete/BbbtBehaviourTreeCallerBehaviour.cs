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
        /// <summary>
        /// The the behaviour tree to call.
        /// </summary>
        [SerializeField] private BbbtBehaviourTree _behaviourTree = null;

        /// <summary>
        /// The the behaviour tree to call.
        /// </summary>
        public BbbtBehaviourTree BehaviourTree { get => _behaviourTree; }

        /// <summary>
        /// The root of the behaviour tree to call. 
        /// </summary>
        private BbbtBehaviour _root = null;


        /// <summary>
        /// OnInitialize gets called the first time the behaviour is called.
        /// </summary>
        protected override void OnInitialize()
        {
        }

        /// <summary>
        /// Doesn't have any termination logic.
        /// </summary>
        protected override void OnTerminate(BbbtBehaviourStatus status)
        {
        }

        /// <summary>
        /// Ticks the root of the behaviour tree to be called.
        /// </summary>
        /// <returns>The status of the root node in the tree that was run.</returns>
        protected override BbbtBehaviourStatus UpdateBehavior()
        {
            return _root.Tick();
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>The generated save data.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            Debug.Log(_behaviourTree.name);
            return new BbbtBehaviourTreeCallerBehaviourSaveData(NodeId, _behaviourTree.name);
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
                var copiedTree = Instantiate(_behaviourTree);
                copiedTree.LoadSaveData(_behaviourTree);
                copiedTree.name = _behaviourTree.name;
                _root = (copiedTree.RootBehaviour as BbbtRoot).Child;
                if (Application.isPlaying)
                {
                    _behaviourTree = copiedTree;
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
    }
}