using System.IO;

namespace Bbbt
{
    /// <summary>
    /// Exports BbbtBehaviourTrees to a format usable by rts-minigame
    /// </summary>
    public static class BbbtExporter
    {
        /// <summary>
        /// Export a BbbtBehaviourTree to a file in a format usable by rts-minigame.
        /// </summary>
        /// <param name="tree">The tree to be exported.</param>
        /// <param name="folder">The folder to which the tree should be exported.</param>
        public static void Export(BbbtBehaviourTree tree, string folder)
        {
            // Create the folder if it doesn't exist.
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}