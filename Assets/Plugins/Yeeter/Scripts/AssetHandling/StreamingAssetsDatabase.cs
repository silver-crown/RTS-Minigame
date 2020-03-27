using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Tyd;
using UnityEngine;
using UnityEngine.Scripting;

namespace Yeeter
{
    [Preserve, MoonSharpUserData, MoonSharpAlias("Assets")]
    public class StreamingAssetsDatabase
    {
        private static readonly string _defaultCompanyName = "Official";

        private static Module[] _modules;
        private static Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
        private static Dictionary<string, TydNode> _defs = new Dictionary<string, TydNode>();
        private static Dictionary<string, string> _settings = new Dictionary<string, string>();
        private static Dictionary<string, TydNode> _settingsNodes = new Dictionary<string, TydNode>();
        private static Dictionary<string, string> _settingsToBeChanged = new Dictionary<string, string>();
        private static Dictionary<string, Action<string>> _onSettingChanged = new Dictionary<string, Action<string>>();

        public static Module[] ActiveModules { get; private set; }
        public static Action OnSoundsDoneLoading { get; set; }

        // Working variables.
        private static Dictionary<string, Module> _packageIdToModule;

        public static Texture2D GetTexture(string key)
        {
            if (_textures.ContainsKey(key))
            {
                return _textures[key];
            }
            else
            {
                InGameDebug.Log("Missing texture: '" + key + "'.");
                if (_textures.ContainsKey("Misc.Missing"))
                {
                    return _textures["Misc.Missing"];
                }
                else
                {
                    return Resources.Load<Texture2D>("Textures/Missing");
                }
            }
        }
        public static string[] GetTextures(string key)
        {
            var result = _textures.Keys.Where(k =>
            {
                if (!k.StartsWith(key)) return false;
                var parts = k.Split(new char[] { '.' });
                k = k.Remove(k.Length - parts[parts.Length - 1].Length - 1);
                return k == key;
            }).ToArray();
            return result;
        }
        public static AudioClip GetSound(string key)
        {
            if (_audioClips.ContainsKey(key))
            {
                return _audioClips[key];
            }
            else
            {
                return null;
            }
        }
        public static TydNode GetDef(string key)
        {
            if (_defs.ContainsKey(key))
            {
                return _defs[key];
            }
            else
            {
                InGameDebug.Log("Missing def: '" + key + "'.");
                return null;
            }
        }
        public static string GetString(string key)
        {
            if (_defs.ContainsKey(key))
            {
                var tydString = _defs[key] as TydString;
                if (tydString != null)
                {
                    return tydString.Value;
                }
                else
                {
                    InGameDebug.Log("Node " + key + " is not a TydString.");
                    return null;
                }
            }
            else
            {
                InGameDebug.Log("Missing def: '" + key + "'.");
                return null;
            }
        }
        public static List<string> GetStrings(string key)
        {
            var values = new List<string>();
            var node = _defs[key] as TydCollection;
            node.Nodes.ForEach(n => values.Add((n as TydString).Value));
            return values;
        }
        public static double GetNumber(string key)
        {
            if (_defs.ContainsKey(key))
            {
                var tydString = _defs[key] as TydString;
                if (tydString != null)
                {
                    double result = 0;
                    if (double.TryParse(tydString.Value, out result))
                    {
                        return result;
                    }
                    else
                    {
                        InGameDebug.Log("Couldn't cast " + key + " to a double.");
                        return 0;
                    }
                }
                else
                {
                    InGameDebug.Log("Node " + key + " is not a TydString.");
                    return 0;
                }
            }
            else
            {
                InGameDebug.Log("Missing def: '" + key + "'.");
                return 0;
            }
        }
        public static string GetSetting(string key)
        {
            if (!key.StartsWith("Settings"))
            {
                key = "Settings." + key;
            }
            if (!_settings.ContainsKey(key))
            {
                InGameDebug.Log("Settings " + key + " not found.");
            }
            return _settings[key];
        }
        public static void AddSettingToBeChanged(string key, string value)
        {
            _settingsToBeChanged[key] = value;
            ApplySettingChanges();
        }
        public static void ApplySettingChanges()
        {
            _settingsToBeChanged.Keys.ToList().ForEach(key =>
            {
                _settings[key] = _settingsToBeChanged[key];
                _settingsToBeChanged.Remove(key);
                if (_onSettingChanged.ContainsKey(key)) _onSettingChanged[key]?.Invoke(_settings[key]);
            });
        }
        private static void WriteSettingsToFile()
        {
            // Build a TydDocument from all settings nodes and write.
            foreach (var key in _settings.Keys)
            {
                var tydString = _settingsNodes[key] as TydString;
                if (tydString != null)
                {
                    tydString.Value = _settings[key];
                }
            }
            var doc = new TydDocument(_settingsNodes.Values);
            TydFile.FromDocument(doc).Save(Application.streamingAssetsPath + "/Settings/Settings.tyd");
        }

        public static void LoadModules()
        {
            string dataFolder = Application.streamingAssetsPath + "/Data";
            string modsFolder = Application.streamingAssetsPath + "/Mods";

            var mods = new List<Module>();
            _packageIdToModule = new Dictionary<string, Module>();

            // Read all mods.
            InGameDebug.Log("Loading mods...");
            Module LoadModule(string path)
            {
                if (Directory.Exists(path + "/About"))
                {
                    if (File.Exists(path + "/About/About.xml"))
                    {
                        return XmlLoader.Load<Module>(path + "/About/About.xml");
                    }
                    else
                    {
                        InGameDebug.Log("\t\tNo About.xml");
                        return null;
                    }
                }
                else
                {
                    Debug.Log("\t\tNo About folder.");
                    return null;
                }
            }
            bool AddMod(string path)
            {
                Debug.Log("\t\tTrying to add mod at '" + path + "'...");
                var mod = LoadModule(path);
                if (mod != null)
                {
                    mods.Add(mod);
                    _packageIdToModule.Add(mod.Id, mod);
                    mod.Path = path;
                    InGameDebug.Log("\t\t'" + mod.Id + "' loaded.");
                    return true;
                }
                else
                {
                    InGameDebug.Log("\t\t<color=orange>Failed to load mod at '" + path + "'.</color>");
                    return false;
                }
            }
            if (!Directory.Exists(dataFolder))
            {
                InGameDebug.Log("No Data folder in StreamingAssets. Created one for you ;).");
                Directory.CreateDirectory(dataFolder);
            }
            bool anyDataModule = false;
            foreach (string folder in Directory.GetDirectories(dataFolder))
            {
                InGameDebug.Log("\tData folder found: '" + folder + "'.");
                bool success = AddMod(folder);
                if (success) anyDataModule = true;
            }
            if (!anyDataModule)
            {
                // Create Core module if there are no modules in Data.
                Directory.CreateDirectory(Path.Combine(dataFolder, "Core"));
                var core = new Module();
                core.Id = _defaultCompanyName + ".Core";
                core.Name = "Core";
                Directory.CreateDirectory(Path.Combine(dataFolder, "Core", "About"));
                var aboutFile = File.Create(Path.Combine(dataFolder, "Core", "About", "About.xml"));
                new XmlSerializer(typeof(Module)).Serialize(aboutFile, core);
                aboutFile.Close();

                InGameDebug.Log("\t<color=orange>No module found in Data. Added Core module.</color>");
            }
            if (!Directory.Exists(modsFolder))
            {
                InGameDebug.Log("No Mods folder in StreamingAssets. Created one for you ;).");
                Directory.CreateDirectory(modsFolder);
                File.WriteAllText(modsFolder + "/Place Mods Here.txt", "");
            }
            foreach (string folder in Directory.GetDirectories(modsFolder))
            {
                InGameDebug.Log("\tMod folder found: '" + folder + "'.");
                AddMod(folder);
            }
            _modules = mods.ToArray();
            InGameDebug.Log("Mods loaded.");
        }
        public static void LoadActiveModules()
        {
            // Load contents of mods in load order
            InGameDebug.Log("Loading active mods...");
            var loadOrderFile = Application.streamingAssetsPath + "/Settings/LoadOrder.txt";
            if (!File.Exists(loadOrderFile))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(loadOrderFile));
                File.WriteAllText(loadOrderFile, _defaultCompanyName + ".Core");
            }
            var loadOrder = File.ReadAllLines(loadOrderFile);
            var activeModules = new List<Module>();
            foreach (string line in loadOrder)
            {
                if (_packageIdToModule.ContainsKey(line.Trim()))
                {
                    InGameDebug.Log("\t" + line);
                    activeModules.Add(_packageIdToModule[line]);
                }
                else
                {
                    InGameDebug.Log(
                        "\t" + line + " was present in " +
                        "StreamingAssets/Settings/LoadOrder.txt but is not installed or its " +
                        "About.xml is invalid."
                    );
                }
            }
            InGameDebug.Log("Active mods loaded.");
            if (activeModules.Count == 0)
            {
                InGameDebug.Log("No active mods?");
            }
            ActiveModules = activeModules.ToArray();
        }
        public static void LoadTexturesFromActiveModules()
        {
            InGameDebug.Log("Loading textures...");
            if (ActiveModules == null || ActiveModules.Length == 0)
            {
                InGameDebug.Log(
                    "StreamingAssetsDatabase.LoadTexturesFromActiveModules(): " +
                    "No active modules. Did you forget to call LoadActiveModules()?");
                return;
            }

            _textures = new Dictionary<string, Texture2D>();
            foreach (var mod in ActiveModules)
            {
                var folders = new List<string>() { mod.Path + "/Textures" };
                if (!Directory.Exists(folders[0]))
                {
                    InGameDebug.Log("\t" + mod.Id + ": No Textures folder.");
                    continue;
                }
                folders.AddRange(GetAllSubDirectories(mod.Path + "/Textures"));
                foreach (var folder in folders)
                {
                    InGameDebug.Log("\tSearching '" + folder + "'...");
                    foreach (var file in Directory.GetFiles(folder))
                    {
                        InGameDebug.Log("\tFile found: '" + file + "'.");
                        if (Path.GetExtension(file) == ".png")
                        {
                            var fileData = File.ReadAllBytes(file);
                            var texture = new Texture2D(2, 2, TextureFormat.ARGB32, false, true);
                            texture.filterMode = FilterMode.Point;
                            texture.LoadImage(fileData);
                            var key =
                                file
                                .Replace(".png", "")
                                .Remove(0, mod.Path.Length + "/Textures".Length + 1)
                                .Replace('\\', '.')
                                .Replace('/', '.');
                            InGameDebug.Log("\t\t<color=green>Texture loaded: '" + key + "' (from " + mod.Id + ").</color>");
                            _textures[key] = texture;
                        }
                        else
                        {
                            InGameDebug.Log("\t\tInvalid extension: '" + Path.GetExtension(file) + "'. Skipping.");
                        }
                    }
                }
            }
            InGameDebug.Log("Textures loaded.");
        }
        public static void LoadScriptsFromActiveModules()
        {
            InGameDebug.Log("Loading scripts...");
            if (ActiveModules == null || ActiveModules.Length == 0)
            {
                InGameDebug.Log(
                    "StreamingAssetsDatabase.LoadScriptsFromActiveModules(): " +
                    "No active modules. Did you forget to call LoadActiveModules()?");
                return;
            }
            Script.DefaultOptions.ScriptLoader = new YeeterScriptLoader();
            foreach (var mod in ActiveModules)
            {
                InGameDebug.Log("\t" + mod.Id);
                var folders = new List<string>() { mod.Path + "/Scripts" };
                if (!Directory.Exists(folders[0]))
                {
                    InGameDebug.Log("\t" + mod.Id + ": No Scripts folder.");
                    continue;
                }
                folders.AddRange(GetAllSubDirectories(mod.Path + "/Scripts"));
                foreach (var folder in folders)
                {
                    InGameDebug.Log("\tSearching '" + folder + "'...");
                    foreach (var file in Directory.GetFiles(folder))
                    {
                        InGameDebug.Log("\tFile found: '" + file + "'.");
                        if (Path.GetExtension(file) == ".lua")
                        {
                            var pathRelativeToScriptsFolder =
                                file
                                .Replace(".lua", "")
                                .Remove(0, mod.Path.Length + "/Scripts".Length + 1);
                            var key =
                                pathRelativeToScriptsFolder
                                .Replace('\\', '.')
                                .Replace('/', '.');
                            InGameDebug.Log(
                                "\t\t<color=green>Script loaded: '" + key + "' (from " + mod.Id + ").</color>");
                            YeeterScriptLoader.AssetDatabaseToPath[key] = file;
                        }
                        else
                        {
                            InGameDebug.Log("\t\tInvalid extension: '" + Path.GetExtension(file) + "'. Skipping.");
                        }
                    }
                }
            }
            InGameDebug.Log("Scripts loaded.");
        }
        public static void LoadScriptsFromDirectory(string directory)
        {
            InGameDebug.Log("Loading scripts...");
            Script.DefaultOptions.ScriptLoader = new YeeterScriptLoader();
            var folders = new List<string>() { directory };
            folders.AddRange(GetAllSubDirectories(directory));
            foreach (var folder in folders)
            {
                InGameDebug.Log("\tSearching '" + folder + "'...");
                foreach (var file in Directory.GetFiles(folder))
                {
                    InGameDebug.Log("\tFile found: '" + file + "'.");
                    if (Path.GetExtension(file) == ".lua")
                    {
                        var pathRelativeToScriptsFolder =
                            file
                            .Replace(".lua", "")
                            .Remove(0, directory.Length + 1);
                        var key =
                            pathRelativeToScriptsFolder
                            .Replace('\\', '.')
                            .Replace('/', '.');
                        InGameDebug.Log(
                            "\t\t<color=green>Script loaded: '" + key + "' (no module).</color>");
                        YeeterScriptLoader.AssetDatabaseToPath[key] = file;
                    }
                    else
                    {
                        InGameDebug.Log("\t\tInvalid extension: '" + Path.GetExtension(file) + "'. Skipping.");
                    }
                }
            }
            InGameDebug.Log("Scripts loaded.");
        }
        public static bool LoadSoundsFromActiveModules()
        {
            InGameDebug.Log("Loading sounds...");
            if (ActiveModules == null || ActiveModules.Length == 0)
            {
                InGameDebug.Log(
                    "StreamingAssetsDatabase.LoadSoundsFromActiveModules(): " +
                    "No active modules. Did you forget to call LoadActiveModules()?");
                return false;
            }

            var audioFiles = new Dictionary<string, AudioLoader.AudioMetaData>();
            foreach (var mod in ActiveModules)
            {
                InGameDebug.Log("\t" + mod.Id);
                var folders = new List<string>() { mod.Path + "/Sounds" };
                if (!Directory.Exists(folders[0]))
                {
                    InGameDebug.Log("\t" + mod.Id + ": No Sounds folder.");
                    continue;
                }
                folders.AddRange(GetAllSubDirectories(mod.Path + "/Sounds"));
                foreach (var folder in folders)
                {
                    InGameDebug.Log("\tSearching '" + folder + "'...");
                    foreach (var file in Directory.GetFiles(folder))
                    {
                        InGameDebug.Log("\tFile found: '" + file + "'.");
                        if (Path.GetExtension(file) == ".ogg" || Path.GetExtension(file) == ".wav")
                        {
                            var pathRelativeToSoundsFolder =
                                file
                                .Replace(".wav", "")
                                .Replace(".ogg", "")
                                .Remove(0, mod.Path.Length + "/Sounds".Length + 1);
                            var key =
                                pathRelativeToSoundsFolder
                                .Replace('\\', '.')
                                .Replace('/', '.');
                            InGameDebug.Log("<color=green>\t\tValid file. Set to be loaded.</color>");
                            if (audioFiles.ContainsKey(key))
                            {
                                InGameDebug.Log(
                                    "<color=green>\t\tOverrides file from " + audioFiles[key].Mod.Id + "</color>"
                                );
                            }
                            audioFiles[key] = new AudioLoader.AudioMetaData(mod, file, key);
                        }
                        else
                        {
                            InGameDebug.Log("\t\tInvalid extension: '" + Path.GetExtension(file) + "'. Skipping.");
                        }
                    }
                }
            }

            if (audioFiles.Count > 0)
            {
                // Start streaming in audio.
                var loader = UnityEngine.Object.Instantiate(new GameObject()).AddComponent<AudioLoader>();
                loader.LoadFiles(audioFiles.Values.ToList());
                loader.OnLoadingDone += () =>
                {
                    _audioClips = new Dictionary<string, AudioClip>();
                    foreach (var pair in loader.Clips)
                    {
                        var file = pair.Key.Path;
                        var mod = pair.Key.Mod;
                        var clip = pair.Value;
                        var pathRelativeToScriptsFolder =
                            file
                            .Replace(".wav", "")
                            .Replace(".ogg", "")
                            .Remove(0, mod.Path.Length + "/Sounds".Length + 1);
                        var key =
                            pathRelativeToScriptsFolder
                            .Replace('\\', '.')
                            .Replace('/', '.');
                        InGameDebug.Log("\t\t<color=green>Sound loaded: '" + key + "' (from " + mod.Id + ").</color>");
                        _audioClips[key] = clip;
                    }
                    InGameDebug.Log("Sounds loaded.");
                    OnSoundsDoneLoading?.Invoke();
                };
            }
            else
            {
                // No audio to load.
                return false;
            }
            return true;
        }
        public static void LoadDefsFromActiveModules()
        {
            // Visits a node to add it to the defs dictionary.
            void AddNodeToAssetDatabase(string parentKey, TydNode node, int index = 0)
            {
                if (node == null) return;

                // Add self.
                string nodeKey = parentKey;
                // TydDocuments should have the same key as their folder. Otherwise we get
                // "Some.Folder.File.0.NodeInFile" instead of "Some.Folder.File.NodeInFile"
                if (node as TydDocument == null)
                {
                    nodeKey += ".";
                    nodeKey += node.Name != null ? node.Name : index.ToString();
                }
                _defs[nodeKey] = node;
                InGameDebug.Log("<color=green>\t\t\tLoaded def: " + nodeKey + "</color>");

                // Visit child nodes if this is a collection.
                var table = node as TydTable;
                if (table != null)
                {
                    for (int i = 0; i < table.Nodes.Count; i++)
                    {
                        AddNodeToAssetDatabase(nodeKey, table.Nodes[i], i);
                    }
                    return;
                }
                var list = node as TydList;
                if (list != null)
                {
                    for (int i = 0; i < list.Nodes.Count; i++)
                    {
                        AddNodeToAssetDatabase(nodeKey, list.Nodes[i], i);
                    }
                    return;
                }
            }

            // Adds a node to the Tyd inheritance list so we can do inheritance.
            void AddNodeToInheritance(TydNode node)
            {
                if (node == null) return;

                // Add to inheritance and visit child nodes if this is a collection.

                var collection = node as TydCollection;
                if (collection != null)
                {
                    Inheritance.Register(collection);
                    for (int i = 0; i < collection.Nodes.Count; i++)
                    {
                        AddNodeToInheritance(collection[i]);
                    }
                }
            }

            if (ActiveModules == null || ActiveModules.Length == 0)
            {
                InGameDebug.Log(
                    "StreamingAssetsDatabase.LoadDefsFromActiveModules(): " +
                    "No active modules. Did you forget to call LoadActiveModules()?");
                return;
            }

            // Merge files.
            var mergedFiles = new Dictionary<string, string>();
            foreach (var mod in ActiveModules)
            {
                InGameDebug.Log("\t" + mod.Id);
                var folders = new List<string>() { mod.Path + "/Defs" };
                if (!Directory.Exists(folders[0]))
                {
                    InGameDebug.Log("\t\t" + mod.Id + ":No Defs folder.");
                    continue;
                }
                folders.AddRange(GetAllSubDirectories(mod.Path + "/Defs"));
                foreach (var folder in folders)
                {
                    InGameDebug.Log("\t\tSearching '" + folder + "'...");
                    foreach (var file in Directory.GetFiles(folder))
                    {
                        InGameDebug.Log("\t\tFile found: '" + file + "'.");
                        if (Path.GetExtension(file) == Constants.TydFileExtension)
                        {
                            var pathRelativeToDefsFolder =
                                file
                                .Replace(Constants.TydFileExtension, "")
                                .Remove(0, mod.Path.Length + "/Defs".Length + 1);
                            var key =
                                pathRelativeToDefsFolder
                                .Replace('\\', '.')
                                .Replace('/', '.');
                            InGameDebug.Log("<color=green>\t\tValid def file: '" + key + "' Adding nodes.</color>");

                            if (!mergedFiles.ContainsKey(key))
                            {
                                mergedFiles[key] = "";
                            }
                            mergedFiles[key] += File.ReadAllText(file) + "\n";
                        }
                        else
                        {
                            InGameDebug.Log("\t\tInvalid extension: '" + Path.GetExtension(file) + "'. Skipping.");
                        }
                    }
                }
            }
            // Add all nodes to inheritance.
            var docs = new Dictionary<string, TydDocument>();
            Inheritance.Initialize();
            foreach (var pair in mergedFiles)
            {
                var nodes = TydFromText.Parse(pair.Value).ToList();
                var doc = new TydDocument(nodes);
                AddNodeToInheritance(doc);
                docs[pair.Key] = doc;
            }
            Inheritance.ResolveAll();

            // Now that we've applied inheritance we can load the node's defs and have stuff work.
            foreach (var pair in docs)
            {
                AddNodeToAssetDatabase(pair.Key, pair.Value);
            }

            InGameDebug.Log("Defs loaded.");
        }
        public static void LoadDefsFromDirectory(string directory)
        {
            // Visits a node to add it to the defs dictionary.
            void AddNodeToAssetDatabase(string parentKey, TydNode node, int index = 0)
            {
                if (node == null) return;

                // Add self.
                string nodeKey = parentKey;
                // TydDocuments should have the same key as their folder. Otherwise we get
                // "Some.Folder.File.0.NodeInFile" instead of "Some.Folder.File.NodeInFile"
                if (node as TydDocument == null)
                {
                    nodeKey += ".";
                    nodeKey += node.Name != null ? node.Name : index.ToString();
                }
                _defs[nodeKey] = node;
                InGameDebug.Log("<color=green>\t\t\tLoaded def: " + nodeKey + "</color>");

                // Visit child nodes if this is a collection.
                var table = node as TydTable;
                if (table != null)
                {
                    for (int i = 0; i < table.Nodes.Count; i++)
                    {
                        AddNodeToAssetDatabase(nodeKey, table.Nodes[i], i);
                    }
                    return;
                }
                var list = node as TydList;
                if (list != null)
                {
                    for (int i = 0; i < list.Nodes.Count; i++)
                    {
                        AddNodeToAssetDatabase(nodeKey, list.Nodes[i], i);
                    }
                    return;
                }
            }

            // Adds a node to the Tyd inheritance list so we can do inheritance.
            void AddNodeToInheritance(TydNode node)
            {
                if (node == null) return;

                // Add to inheritance and visit child nodes if this is a collection.

                var collection = node as TydCollection;
                if (collection != null)
                {
                    Inheritance.Register(collection);
                    for (int i = 0; i < collection.Nodes.Count; i++)
                    {
                        AddNodeToInheritance(collection[i]);
                    }
                }
            }

            // Merge files.
            var mergedFiles = new Dictionary<string, string>();
            var folders = new List<string>() { directory };
            folders.AddRange(GetAllSubDirectories(directory));
            foreach (var folder in folders)
            {
                InGameDebug.Log("\t\tSearching '" + folder + "'...");
                foreach (var file in Directory.GetFiles(folder))
                {
                    InGameDebug.Log("\t\tFile found: '" + file + "'.");
                    if (Path.GetExtension(file) == Constants.TydFileExtension)
                    {
                        var pathRelativeToDefsFolder =
                            file
                            .Replace(Constants.TydFileExtension, "")
                            .Remove(0, directory.Length + 1);
                        var key =
                            pathRelativeToDefsFolder
                            .Replace('\\', '.')
                            .Replace('/', '.');
                        InGameDebug.Log("<color=green>\t\tValid def file: '" + key + "' Adding nodes.</color>");

                        if (!mergedFiles.ContainsKey(key))
                        {
                            mergedFiles[key] = "";
                        }
                        mergedFiles[key] += File.ReadAllText(file) + "\n";
                    }
                    else
                    {
                        InGameDebug.Log("\t\tInvalid extension: '" + Path.GetExtension(file) + "'. Skipping.");
                    }
                }
            }
            // Add all nodes to inheritance.
            var docs = new Dictionary<string, TydDocument>();
            Inheritance.Initialize();
            foreach (var pair in mergedFiles)
            {
                var nodes = TydFromText.Parse(pair.Value).ToList();
                var doc = new TydDocument(nodes);
                AddNodeToInheritance(doc);
                docs[pair.Key] = doc;
            }
            Inheritance.ResolveAll();

            // Now that we've applied inheritance we can load the node's defs and have stuff work.
            foreach (var pair in docs)
            {
                AddNodeToAssetDatabase(pair.Key, pair.Value);
            }

            InGameDebug.Log("Defs loaded.");
        }
        public static void LoadSettings()
        {
            // Visits a node to add it to the defs dictionary.
            void AddSetting(string parentKey, TydNode node, int index = 0)
            {
                if (node == null) return;

                // Add self.
                string nodeKey = parentKey;
                if (node as TydDocument == null)
                {
                    nodeKey += ".";
                    nodeKey += node.Name != null ? node.Name : index.ToString();
                    _settingsNodes[nodeKey] = node;
                }
                InGameDebug.Log("<color=green>\t\t\tLoaded setting: " + nodeKey + "</color>");
                // Also add this to settings if it is an end point.
                var tydString = node as TydString;
                if (tydString != null)
                {
                    _settings.Add(nodeKey, tydString.Value);
                }

                // Visit child nodes if this is a collection.
                var table = node as TydTable;
                if (table != null)
                {
                    for (int i = 0; i < table.Nodes.Count; i++)
                    {
                        AddSetting(nodeKey, table.Nodes[i], i);
                    }
                    return;
                }
                var list = node as TydList;
                if (list != null)
                {
                    for (int i = 0; i < list.Nodes.Count; i++)
                    {
                        AddSetting(nodeKey, list.Nodes[i], i);
                    }
                    return;
                }
            }
            InGameDebug.Log("Loading settings...");
            var file = Application.streamingAssetsPath + "/Settings/Settings.tyd";
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "musicVolume 0.5\neffectsVolume 0.5");
            }
            var tydFile = TydFile.FromFile(file);
            AddSetting("Settings", tydFile.DocumentNode);
            InGameDebug.Log("Settings loaded.");
        }

        public static void AddOnSettingChangedListener(string key, Action<string> action)
        {
            if (!key.StartsWith("Settings"))
            {
                key = "Settings." + key;
            }
            Debug.Log(key);
            if (!_onSettingChanged.ContainsKey(key)) _onSettingChanged.Add(key, action);
            else _onSettingChanged[key] += action;
        }
        public static void AddOnSettingChangedListener(string key, DynValue action)
        {
            if (!key.StartsWith("Settings"))
            {
                key = "Settings." + key;
            }
            Debug.Log(key);
            if (!_onSettingChanged.ContainsKey(key))
            {
                _onSettingChanged.Add(key, v => LuaManager.Call(action, DynValue.NewString(v)));
            }
            else
            {
                _onSettingChanged[key] += v => LuaManager.Call(action, DynValue.NewString(v));
            }
        }

        private static List<string> GetAllSubDirectories(string path)
        {
            var subfolders = Directory.GetDirectories(path).ToList();
            var addedFolders = new List<string>();
            foreach (var subfolder in subfolders)
            {
                var subsubfolders = GetAllSubDirectories(subfolder);
                if (subsubfolders != null && subsubfolders.Count > 0)
                {
                    foreach (var subsubfolder in subsubfolders)
                    {
                        addedFolders.Add(subsubfolder);
                    }
                }
            }
            foreach (var folder in addedFolders)
            {
                subfolders.Add(folder);
            }
            return subfolders;
        }
    }
}