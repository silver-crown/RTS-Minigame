using JsonSubTypes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// A BbbtBehaviourTree scriptable object that can be loaded into and edited in the bbBT editor window.
    /// </summary>
    [CreateAssetMenu(fileName = "New bbBT Behaviour Tree", menuName = "bbBT/Behaviour Tree", order = 0)]
    [JsonConverter(typeof(JsonSubtypes))]
    public class BbbtBehaviourTree : ScriptableObject
    {
        /// <summary>
        /// The data needed to reconstruct the tree in the editor.
        /// </summary>
        [JsonProperty] public BbbtBehaviourTreeEditorSaveData EditorSaveData { get; protected set; }

        /// <summary>
        /// The data needed to functionally reconstruct the behaviour tree.
        /// </summary>
        [JsonProperty] public BbbtBehaviourTreeSaveData SaveData { get; protected set; }

        /// <summary>
        /// The entry point of the behaviour tree.
        /// </summary>
        [JsonProperty] public BbbtBehaviour RootBehaviour { get; protected set; }

        /// <summary>
        /// A list of all the behaviours in the tree.
        /// </summary>
        [JsonProperty] public List<BbbtBehaviour> Behaviours { get; protected set; }


        /// <summary>
        /// Saves the behaviour tree's data to a json file of the same name as the tree.
        /// </summary>
        /// <param name="editorSaveData">The editor save data to be used.</param>
        /// <param name="saveData">The functional save data to be used.</param>
        public void Save(BbbtBehaviourTreeEditorSaveData editorSaveData, BbbtBehaviourTreeSaveData saveData)
        {
            // Save editor data
            string parentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

            if (!Directory.Exists(Path.Combine(parentDirectory, "json")))
            {
                AssetDatabase.CreateFolder(parentDirectory, "json");
            }
            string jsonFolder = AssetDatabase.GUIDToAssetPath(
                AssetDatabase.AssetPathToGUID(Path.Combine(parentDirectory, "json")
            ));

            if (editorSaveData != null)
            {
                EditorSaveData = editorSaveData;
                string editorJson = JsonConvert.SerializeObject(editorSaveData, Formatting.Indented);
                File.WriteAllText(Path.Combine(jsonFolder, name + ".editor.json"), editorJson);
            }

            if (saveData != null)
            {
                SaveData = saveData;
                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
                File.WriteAllText(Path.Combine(jsonFolder, name + ".json"), json);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Loads the behaviour tree save data from the json file of the same name in the json directory.
        /// </summary>
        /// <param name="sourceTree">The tree this is sourced from if it is sourced from another tree.</param>
        public void LoadSaveData(BbbtBehaviourTree sourceTree = null)
        {
            if (sourceTree == null)
            {
                sourceTree = this;
            }
            string fileName = sourceTree.name;
            string parentDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(sourceTree));

            if (Directory.Exists(Path.Combine(parentDirectory, "json")))
            {
                // Load editor save data.
                if (File.Exists(Path.Combine(parentDirectory, "json", fileName + ".editor.json")))
                {
                    string jsonFolderGuid = AssetDatabase.AssetPathToGUID(Path.Combine(parentDirectory, "json"));
                    string path = Path.Combine(
                        AssetDatabase.GUIDToAssetPath(jsonFolderGuid),
                        fileName + ".editor.json"
                    );
                    string json = File.ReadAllText(path);
                    EditorSaveData = JsonConvert.DeserializeObject<BbbtBehaviourTreeEditorSaveData>(json);
                }

                // Load functional save data.
                if (File.Exists(Path.Combine(parentDirectory, "json", fileName + ".json")))
                {
                    string jsonFolderGuid = AssetDatabase.AssetPathToGUID(Path.Combine(parentDirectory, "json"));
                    string path = Path.Combine(
                        AssetDatabase.GUIDToAssetPath(jsonFolderGuid),
                        fileName + ".json"
                    );
                    string json = File.ReadAllText(path);
                    SaveData = JsonConvert.DeserializeObject<BbbtBehaviourTreeSaveData>(json);
                    BuildTree();
                }
            }
        }

        /// <summary>
        /// Builds a functional behaviour tree from the save data.
        /// </summary>
        private void BuildTree()
        {
            RootBehaviour = SaveData.Root;

            // Populate the behaviour list
            if ((RootBehaviour as BbbtRoot).Child != null)
            {
                Behaviours = new List<BbbtBehaviour>();
                Behaviours.Add(RootBehaviour);
                var behavioursToVisit = new Queue<BbbtBehaviour>();
                behavioursToVisit.Enqueue((RootBehaviour as BbbtRoot).Child);
                while (behavioursToVisit.Count != 0)
                {
                    var behaviour = (behavioursToVisit.Dequeue());
                    Behaviours.Add(behaviour);
                    if (behaviour as BbbtCompositeBehaviour != null)
                    {
                        foreach (var child in (behaviour as BbbtCompositeBehaviour).Children)
                        {
                            behavioursToVisit.Enqueue(child);
                        }
                    }
                    else if (behaviour as BbbtDecoratorBehaviour != null)
                    {
                        behavioursToVisit.Enqueue((behaviour as BbbtDecoratorBehaviour).Child);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Behaviour tree " + name + "'s root has no child.");
            }
        }
        
        /// <summary>
        /// Tries to find a beahaviour tree that matches a given query (case insensitive).
        /// </summary>
        /// <param name="query">
        /// The string matching (case insensitive) the name of the behaviour tree to find.
        /// Has to match the name of the behaviour tree exactly (apart from caes) as instantiated in the assets folder.
        /// </param>
        /// <returns>The behaviour tree that maches the query, if found. Null otherwise.</returns>
        public static BbbtBehaviourTree FindBehaviourTreeWithName(string query)
        {
            // Try to find a behaviour tree with name matching the query.
            var guids = AssetDatabase.FindAssets(query + " t:BbbtBehaviourTree");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var behaviourTree = AssetDatabase.LoadAssetAtPath<BbbtBehaviourTree>(path);

                if (behaviourTree.name.ToLower() == query.ToLower())
                {
                    if (behaviourTree != null)
                    {
                        // Found a behaviour tree matching the query.
                        return AssetDatabase.LoadAssetAtPath<BbbtBehaviourTree>(path);
                    }
                }
            }

            // No behaviour tree found with name matching the query.
            return null;
        }
    }
}