using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a "Create>Lua Script" option the the Assets menu.
/// </summary>
public class LuaMenuItem
{
    /// <summary>
    /// Creates a Lua Script in the assets folder.
    /// </summary>
    [MenuItem("Assets/Create/Lua Script", priority = 82)]
    public static void CreateLuaScript()
    {
        // Get the selected path.
        string folderPath = "Assets";
        foreach (var selectedObject in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
        {
            folderPath = AssetDatabase.GetAssetPath(selectedObject);
            if (File.Exists(folderPath))
            {
                folderPath = Path.GetDirectoryName(folderPath);
            }
            break;
        }
        //folderPath = Path.Combine(folderPath, "New Script.lua");
        string fileName = "New Script.lua";
        int fileNameSuffixIndex = 1;
        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            fileName = "New Script" + fileNameSuffixIndex + ".lua";
            fileNameSuffixIndex++;
        }
        string filePath = Path.Combine(folderPath, fileName);

        // Add the lua script.
        File.Create(filePath);
        AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceSynchronousImport);
    }
}
