using System.Timers;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// MonoBehaviour component for using bbBT behaviour trees with GameObjects.
    /// </summary>
    public class BbbtBehaviourTreeComponent : MonoBehaviour
    {
        /// <summary>
        /// The unique tree belonging to this component.
        /// </summary>
        public BbbtBehaviourTree Tree = null;

        /// <summary>
        /// The entry point of the behaviour tree.
        /// </summary>
        private BbbtBehaviour _rootNode = null;

        /// <summary>
        /// Timer used to keep the behaviour tree ticking.
        /// </summary>
        private Timer _timer = null;


        /// <summary>
        /// Awake is called before Start.
        /// </summary>
        /// <param name="treeName">The name of the tree to use.</param>
        public void SetBehaviourTree(string treeName)
        {
            // Start the timer.
            _timer = new Timer(200);
            _timer.Elapsed += Tick;
            _timer.AutoReset = true;
            _timer.Enabled = true;

            // Build the tree.
            var sourceTree = BbbtBehaviourTree.FindBehaviourTreeWithName(treeName);
            Tree = Instantiate(sourceTree);
            Tree.LoadSaveData(sourceTree);
            Tree.name = sourceTree.name + " (" + name + ")";
            _rootNode = (Tree.RootBehaviour as BbbtRoot).Child;
        }

        /// <summary>
        /// Ticks the behaviour tree.
        /// </summary>
        private void Tick(object source, ElapsedEventArgs e)
        {
            _rootNode.Tick();
        }

        /// <summary>
        /// Called when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (_timer != null)
            {
                // Stop timer upon destroying the tree so it doesn't keep ticking.
                _timer.Stop();
                _timer.Dispose();
            }
        }
    }
}