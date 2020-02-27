using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Behaviour which selects the leftmost valid child for execution.
    /// </summary>
    [CreateAssetMenu(fileName = "Selector", menuName =  "bbBT/Behaviour/Composite/Selector", order = 0)]
    public class BbbtSelector : BbbtCompositeBehaviour
    {
        /// <summary>
        /// BbbtSelector doesn't use any initialisation logic.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        protected override void OnInitialize(GameObject gameObject)
        {
        }

        /// <summary>
        /// BbbtSelector doesn't use any termination logic.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        /// <param name="status">The status of the behaviour upon termination.</param>
        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        /// <summary>
        /// Executes the first runnable child behaviour.
        /// </summary>
        /// <param name="gameObject">The game object that owns the behaviour.</param>
        /// <returns>The status of the child that was run, or Invalid if none was found.</returns>
        protected override BbbtBehaviourStatus UpdateBehavior(GameObject gameObject)
        {
            // Iterate through the children and return the status of the first child that
            // returned Running or Success.
            foreach (var child in Children)
            {
                var status = child.Tick(gameObject);
                if (status == BbbtBehaviourStatus.Running || status == BbbtBehaviourStatus.Success)
                {
                    // Found a child that
                    return status;
                }
            }
            // No child could be run.
            return BbbtBehaviourStatus.Invalid;
        }

        /// <summary>
        /// Converts the behaviour to save data.
        /// </summary>
        /// <returns>Null.</returns>
        public override BbbtBehaviourSaveData ToSaveData()
        {
            if (Children != null)
            {
                var childSaveData = new BbbtBehaviourSaveData[Children.Count];
                for (int i = 0; i < Children.Count; i++)
                {
                    childSaveData[i] = Children[i].ToSaveData();
                }
                return new BbbtSelectorSaveData(NodeId, childSaveData);
            }
            else
            {
                return new BbbtSelectorSaveData(NodeId, null);
            }
        }

        /// <summary>
        /// Sets up the behaviour from save data.
        /// </summary>
        /// <param name="saveData">The save data to use for setting up the behaviour.</param>
        public override void LoadSaveData(BbbtBehaviourSaveData saveData)
        {
            base.LoadSaveData(saveData);
            Children = null;
            var castSaveData = saveData as BbbtSelectorSaveData;
            if (castSaveData != null)
            {
                if (castSaveData.ChildSaveData != null)
                {
                    foreach (var childSaveData in castSaveData.ChildSaveData)
                    {
                        AddChild(childSaveData.Deserialize());
                    }
                }
            }
            else
            {
                Debug.LogError("Save data passed to BbbtSelector was not BbbtSelectorSaveData.");
            }
        }
    }
}