using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Custom editor for editing individual behaviour tree nodes.
    /// </summary>
    [CustomEditor(typeof(BbbtNode))]
    public class BbbtNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            var node = (BbbtNode)target;

            // Basic info
            EditorGUILayout.LabelField("Id", node.Id.ToString());
            //EditorGUILayout.LabelField("Type", node.BaseBehaviour.name);

            // Display the editable properties of the node's behaviour.
            EditorGUILayout.Separator();
            var behaviourEditor = CreateEditor(node.Behaviour);
            behaviourEditor.OnInspectorGUI();

            // Show status of node's behaviour
            if (Application.isPlaying)
            {
                EditorGUILayout.EnumFlagsField(node.Behaviour.Status);
            }

            // Check if anything changed, if so tell the bbBT editor window that a change occured in the tab
            // that the node belongs to.
            if (EditorGUI.EndChangeCheck())
            {
                var window = EditorWindow.GetWindow<BbbtWindow>(null, false);
                window.SetUnsavedChangesTabTitle(node.Tab);
            }
        }
    }
}