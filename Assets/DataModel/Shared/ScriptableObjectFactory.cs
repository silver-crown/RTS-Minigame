using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using System.Reflection;

namespace HGC
{
    // Info for assemblies - https://docs.unity3d.com/2018.3/Documentation/Manual/ScriptCompilationAssemblyDefinitionFiles.html
    public static class ScriptableObjectFactory
    {
        //TODO: this is a hack
        public const string ProjectName = "VR";

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Scriptable Object"), UnityEditor.MenuItem("HGC/Tools/Scriptable Object/Create")]
        public static void CreateInWindow()
        {
            var assemblies = GetAssemblies();

            // Get all classes derived from ScriptableObject in the unity assemblies
            var allScriptableObjects = (from assembly in assemblies
                                        from type in assembly.GetTypes()
                                        where type.IsSubclassOf(typeof(ScriptableObject))
                                        select type).ToArray();

            // Show the selection window.
            ScriptableObjectWindow.Init(allScriptableObjects);
        }

        public static void CreateAsset<T>() where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject)), "");
            }

            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

            UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = asset;
        }

        public static void CreateAsset<T>(string name) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject)), "");
            }
            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".asset");
            UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = asset;
        }

        public static T Create<T>() where T : ScriptableObject
        {
            string typeName = TypeUtil.GetUnqualifiedTypeName(typeof(T));

            T scriptableObject;

            scriptableObject = ScriptableObject.CreateInstance<T>();
            UnityEditor.AssetDatabase.CreateAsset(scriptableObject, "Assets/Resources/" + ProjectName + "/" + typeName + ".asset");
            UnityEditor.AssetDatabase.Refresh();

            return scriptableObject;
        }

        /// <summary>
        /// Returns the assembly that contains the script code for this project
        /// </summary>
        private static IEnumerable<Assembly> GetAssemblies()
        {    
            // add additional assemblies here for each miniproject

            yield return Assembly.Load(new AssemblyName("Assembly-CSharp"));
            //yield return Assembly.Load(new AssemblyName("Assembly-CSharp-firstpass"));
        }
#endif

        private static Dictionary<string, ScriptableObject> _loadedScriptableObjects;
        private static Dictionary<string, ScriptableObject> LoadedScriptableObjects
        {
            get
            {
                if(_loadedScriptableObjects == null)
                    _loadedScriptableObjects = new Dictionary<string, ScriptableObject>();

                return _loadedScriptableObjects;
            }
        }

        public static T Load<T>(string resourcePath)
            where T : ScriptableObject
        {
            return Load<T>(resourcePath, false);
        }

        public static T Load<T>(string resourcePath, bool errorIfNull)
            where T : ScriptableObject
        {
            string name = string.Empty;

            if(!string.IsNullOrEmpty(resourcePath))
            {
                name = resourcePath;
            }
            else
            {
                name = TypeUtil.GetUnqualifiedTypeName(typeof(T));
            }

            if (!LoadedScriptableObjects.ContainsKey(name) || LoadedScriptableObjects[name] == null)
            {
                T scriptableObject = Resources.Load<T>(name);

                if (scriptableObject != null)
                {
                    LoadedScriptableObjects.Add(name, scriptableObject);
                }
                else
                {
                    if(errorIfNull)
                    Debug.LogError("Scriptable object of type '" + typeof(T).ToString() + "' does not exist locally at '" + name + "'");
                }

                return scriptableObject;
            }
            else
            {
                return (T)LoadedScriptableObjects[name];
            }
        }

        public static T Load<T>(bool errorIfNull)
            where T : ScriptableObject
        {
            return Load<T>(errorIfNull);
        }

        public static T LoadOrCreate<T>() where T : ScriptableObject
        {
            T scriptableObject = Load<T>(false);

            if (scriptableObject == null)
            {
#if UNITY_EDITOR
                scriptableObject = Create<T>();
                LoadedScriptableObjects.AddOrUpdate(TypeUtil.GetUnqualifiedTypeName(typeof(T)), scriptableObject);
#else
                Debug.LogError("Scriptable object of type '" + typeof(T).ToString() + "' does not exist locally cannot create it outside of the editor.");
#endif
            }

            return scriptableObject;
        }
    }
}