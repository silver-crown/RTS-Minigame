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
        [HideInInspector] public BbbtBehaviourTree Tree = null;

        /// <summary>
        /// The entry point of the behaviour tree.
        /// </summary>
        private BbbtBehaviour _rootNode = null;


        /// <summary>
        /// Called every frame.
        /// </summary>
        private void Update()
        {
            if (_rootNode != null)
            {
                if (Time.frameCount % 5 == 0)
                {
                    _rootNode.Tick(gameObject);
                }
            }
        }

        /// <summary>
        /// Awake is called before Start.
        /// </summary>
        /// <param name="treeName">The name of the tree to use.</param>
        public void SetBehaviourTree(string treeName)
        {
            if (treeName == null)
            {
                Debug.LogError(GetType().Name + ".SetBehaviourTree(): treeName was null. Aborting.", this);
                return;
            }

            // Build the tree.
            var sourceTree = BbbtBehaviourTree.FindBehaviourTreeWithName(treeName);
            Tree = Instantiate(sourceTree);
            Tree.name = sourceTree.name + " (" + name + ")";
            Tree.LoadSaveData(sourceTree);
            _rootNode = (Tree.RootBehaviour as BbbtRoot).Child;
        }
    }
}