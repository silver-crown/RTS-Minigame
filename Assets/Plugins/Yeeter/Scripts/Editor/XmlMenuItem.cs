using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Adds a "Create>Xml" option the the Assets menu.
/// </summary>
public class XmlMenuItem
{
    /// <summary>
    /// Creates a Xml in the assets folder.
    /// </summary>
    [MenuItem("Assets/Create/Xml", priority = 82)]
    public static void CreateXmlFile()
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
        string fileName = "New Xml.xml";
        int fileNameSuffixIndex = 1;
        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            fileName = "New Xml" + fileNameSuffixIndex + ".xml";
            fileNameSuffixIndex++;
        }
        string filePath = Path.Combine(folderPath, fileName);

        // Add the file.
        File.Create(filePath).Close();
        AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceSynchronousImport);
    }
}
