using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a "Create>Ink" option the the Assets menu.
/// </summary>
public class InkMenuItem
{
    /// <summary>
    /// Creates a .ink in the assets folder.
    /// </summary>
    [MenuItem("Assets/Create/ink", priority = 82)]
    public static void CreateInkFile()
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
        string fileName = "New Ink.ink";
        int fileNameSuffixIndex = 1;
        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            fileName = "New Ink" + fileNameSuffixIndex + ".ink";
            fileNameSuffixIndex++;
        }
        string filePath = Path.Combine(folderPath, fileName);

        // Add the file.
        File.Create(filePath).Close();
        AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceSynchronousImport);
    }
}
