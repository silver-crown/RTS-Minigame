using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a "Create>txt" option the the Assets menu.
/// </summary>
public class TxtMenuItem
{
    /// <summary>
    /// Creates a txt file in the assets folder.
    /// </summary>
    [MenuItem("Assets/Create/txt", priority = 82)]
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
        string fileName = "New Text File.txt";
        int fileNameSuffixIndex = 1;
        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            fileName = "New Text File" + fileNameSuffixIndex + ".txt";
            fileNameSuffixIndex++;
        }
        string filePath = Path.Combine(folderPath, fileName);

        // Add the lua script.
        File.Create(filePath).Close();
        AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceSynchronousImport);
    }
}
