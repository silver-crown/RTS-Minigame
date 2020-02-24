using UnityEditor;

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
            base.OnInspectorGUI();
            var node = (BbbtNode)target;

            // Basic info
            EditorGUILayout.LabelField("Id", node.Id.ToString());
            //EditorGUILayout.LabelField("Type", node.BaseBehaviour.name);

            // Display the editable properties of the node's behaviour.
            EditorGUILayout.Separator();
            var behaviourEditor = CreateEditor(node.Behaviour);
            behaviourEditor.OnInspectorGUI();
        }
    }
}