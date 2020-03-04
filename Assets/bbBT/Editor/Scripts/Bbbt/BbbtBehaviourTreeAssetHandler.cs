using Bbbt.Constants;
using System;
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
        private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
        {
            if (AssetDatabase.GetMainAssetTypeAtPath(assetPath) == typeof(BbbtBehaviourTree))
            {
                // Delete json files.
                if (Directory.Exists(BbbtConstants.JsonDirectory))
                {
                    string name = Path.GetFileNameWithoutExtension(assetPath);
                    string jsonFilePath = Path.Combine(BbbtConstants.JsonDirectory, name + ".json");
                    if (File.Exists(jsonFilePath))
                    {
                        File.Delete(jsonFilePath);
                    }
                    string jsonFileMetaPath = jsonFilePath + ".meta";
                    if (File.Exists(jsonFileMetaPath))
                    {
                        File.Delete(jsonFileMetaPath);
                    }
                    string jsonEditorFilePath = Path.Combine(BbbtConstants.JsonDirectory, name + ".editor.json");
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

        /*
        /// <summary>
        /// Called when an asset will move.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns>AssetMoveResult.DidNotMove.</returns>
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            if (AssetDatabase.GetMainAssetTypeAtPath(sourcePath) == typeof(BbbtBehaviourTree))
            {
                string parentDirectory = Path.GetDirectoryName(sourcePath);
                string name = Path.GetFileNameWithoutExtension(sourcePath);
                string destinationParentDirectory = Path.GetDirectoryName(destinationPath);
                string sourceJsonDirectory = AssetDatabase.GUIDToAssetPath(
                    AssetDatabase.AssetPathToGUID(Path.Combine(parentDirectory, "json")
                ));
                if (Directory.Exists(sourceJsonDirectory))
                {
                    string jsonFilePath = Path.Combine(sourceJsonDirectory, name + ".json");
                    string jsonFileMetaPath = jsonFilePath + ".meta";
                    string jsonEditorFilePath = Path.Combine(sourceJsonDirectory, name + ".editor.json");
                    string jsonEditorFileMetaPath = jsonEditorFilePath + ".meta";

                    // Create a json directory in the destination path.
                    string destinationJsonDirectory = Path.Combine(destinationParentDirectory, "json");

                    // Moves a file from the source json directory to the target json directory.
                    void MoveFile(string path)
                    {
                        if (File.Exists(path))
                        {
                            if (!Directory.Exists(destinationJsonDirectory))
                            {
                                Directory.CreateDirectory(destinationJsonDirectory);
                            }
                            string fileName = Path.GetFileName(path);
                            File.Move(path, Path.Combine(destinationJsonDirectory, fileName));
                        }
                    }

                    // Move json files.
                    MoveFile(jsonFilePath);
                    MoveFile(jsonFileMetaPath);
                    MoveFile(jsonEditorFilePath);
                    MoveFile(jsonEditorFileMetaPath);


                    // Delete the old json directory if it is empty.
                    if (Directory.GetFiles(sourceJsonDirectory).Length == 0)
                    {
                        Directory.Delete(sourceJsonDirectory);
                        if (File.Exists(sourceJsonDirectory + ".meta"))
                        {
                            File.Delete(sourceJsonDirectory + ".meta");
                        }
                    }
                }
                AssetDatabase.Refresh();
            }
            return AssetMoveResult.DidNotMove;
        }
        */
    }
}