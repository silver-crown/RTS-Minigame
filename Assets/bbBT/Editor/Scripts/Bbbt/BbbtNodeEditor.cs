using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Custom editor for editing behaviour tree nodes.
    /// </summary>
    [CustomEditor(typeof(BbbtNode))]
    public class BbbtNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            var node = (BbbtNode)target;

            // Basic info
            EditorGUILayout.LabelField("Id", node.Id.ToString());
            EditorGUILayout.LabelField("Type", node.Type.ToString());

            // Allow attaching action if leaf node.
            if (node.Type == BbbtNodeType.Leaf)
            {
                node.AttachedAction = EditorGUILayout.ObjectField(
                    "Attached Behaviour Tree",
                    node.AttachedAction,
                    typeof(BbbtBehaviourTree),
                    false
                ) as BbbtBehaviourTree;
            }
        }
    }
}