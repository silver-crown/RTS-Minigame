using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Bbbt
{
    [InitializeOnLoad]
    public class BbbtBehaviourTreeAssetHandler : UnityEditor.AssetModificationProcessor
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

        /// <summary>
        /// Called when the asset will be deleted.
        /// </summary>
        /// <param name="assetPath">The path of the asset.</param>
        /// <param name="options"></param>
        /// <returns>AssetDeleteResult.DidNotDelete.</returns>
        public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(BbbtBehaviourTree))
            {
                // Delete json files.
                string parentDirectory = Path.GetDirectoryName(assetPath);
                if (Directory.Exists(Path.Combine(parentDirectory, "json")))
                {
                    string name = Path.GetFileNameWithoutExtension(assetPath);
                    string jsonFolder = AssetDatabase.GUIDToAssetPath(
                        AssetDatabase.AssetPathToGUID(Path.Combine(parentDirectory, "json")
                    ));
                    string jsonFilePath = Path.Combine(jsonFolder, name + ".json");
                    if (File.Exists(jsonFilePath))
                    {
                        File.Delete(jsonFilePath);
                    }
                    string jsonFileMetaPath = jsonFilePath + ".meta";
                    if (File.Exists(jsonFileMetaPath))
                    {
                        File.Delete(jsonFileMetaPath);
                    }
                    string jsonEditorFilePath = Path.Combine(jsonFolder, name + ".editor.json");
                    if (File.Exists(jsonEditorFilePath))
                    {
                        File.Delete(jsonEditorFilePath);
                    }
                    string jsonEditorFileMetaPath = jsonEditorFilePath + ".meta";
                    if (File.Exists(jsonEditorFileMetaPath))
                    {
                        File.Delete(jsonEditorFileMetaPath);
                    }
                    AssetDatabase.Refresh();
                }
            }
            return AssetDeleteResult.DidNotDelete;
        }
    }
}