using Newtonsoft.Json;

namespace Bbbt
{
    /// <summary>
    /// Data needed to serialise and deserialise a behaviour tree.
    /// </summary>
    public class BbbtBehaviourTreeSaveData
    {
        /// <summary>
        /// The data needed to load the root behaviour.
        /// </summary>
        //public BbbtRootSaveData RootSaveData { get; protected set; }


        /// <summary>
        /// The root of the behaviour tree.
        /// </summary>
        public BbbtRoot Root { get; protected set; }

        /// <summary>
        /// Constructs a new BbbtBehaviourTreeSaveData object.
        /// </summary>
        /// <param name="rootSaveData">The root behaviour's save data.</param>
        /// <param name="root">The root of the behaviour tree.</param>
        public BbbtBehaviourTreeSaveData(/*BbbtRootSaveData rootSaveData*/ BbbtRoot root)
        {
            //RootSaveData = rootSaveData;
            Root = root;
        }
    }
}