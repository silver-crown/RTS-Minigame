using UnityEditor;
using UnityEditor.Callbacks;

namespace Bbbt
{
    public static class BbbtBehaviourTreeAssetHandler
    {
        /// <summary>
        /// Called when an instance of BbbtBehaviourTree is selected in the project hierarchy.
        /// </summary>
        /// <param name="instanceID">The instance id of the opened asset.</param>
        /// <param name="line">TODO: Find out what line is.</param>
        /// <returns>Whether we succesfully open the asset in the bbBT editor window.</returns>
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string assetPath = AssetDatabase.GetAssetPath(instanceID);
            var tree = AssetDatabase.LoadAssetAtPath<BbbtBehaviourTree>(assetPath);

            if (tree != null)
            {
                tree.LoadSaveData();

                var window = EditorWindow.GetWindow<BbbtWindow>();
                window.LoadTree(tree);
                window.Show();
                return true;
            }
            return false;
        }
    }
}