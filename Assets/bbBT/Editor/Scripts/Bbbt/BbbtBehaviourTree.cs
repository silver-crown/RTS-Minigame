using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// A BbbtBehaviourTree scriptable object that can be loaded into and edited in the bbBT editor window.
    /// </summary>
    [CreateAssetMenu(fileName = "New bbBT Behaviour Tree", menuName = "bbBT Behaviour Tree", order = 0)]
    public class BbbtBehaviourTree : ScriptableObject
    {
        /// <summary>
        /// The data associated with the behaviour tree.
        /// </summary>
        public BbbtBehaviourTreeSaveData Data { get; protected set; }


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
        /// Saves the behaviour tree to a json file of the same name as the tree.
        /// </summary>
        /// <param name="saveData">The save data to be used.</param>
        public void SaveData(BbbtBehaviourTreeSaveData saveData)
        {
            Data = saveData;
            string parentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            string json = JsonUtility.ToJson(saveData);

            if (!Directory.Exists(Path.Combine(parentDirectory, "json")))
            {
                AssetDatabase.CreateFolder(parentDirectory, "json");
            }
            string jsonFolderGuid = AssetDatabase.AssetPathToGUID(Path.Combine(parentDirectory, "json"));

            File.WriteAllText(Path.Combine(AssetDatabase.GUIDToAssetPath(jsonFolderGuid), name + ".json"), json);
        }

        /// <summary>
        /// Loads the behaviour tree save data from the json file of the same name in the json directory.
        /// </summary>
        public void LoadSaveData()
        {
            string parentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            if (Directory.Exists(Path.Combine(parentDirectory, "json")))
            {
                if (File.Exists(Path.Combine(parentDirectory, "json", name + ".json")))
                {
                    string jsonFolderGuid = AssetDatabase.AssetPathToGUID(Path.Combine(parentDirectory, "json"));
                    string json = File.ReadAllText(Path.Combine(
                        AssetDatabase.GUIDToAssetPath(jsonFolderGuid),
                        name + ".json"
                    ));
                    Data = JsonUtility.FromJson<BbbtBehaviourTreeSaveData>(json);
                }
            }
        }
    }
}