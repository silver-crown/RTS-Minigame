using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Custom editor for BbbtBehaviourTreeComponents
    /// </summary>
    [CustomEditor(typeof(BbbtBehaviourTreeComponent))]
    public class BbbtBehaviourTreeComponentEditor : Editor
    {
        /// <summary>
        /// Called when a BbbtBehaviourTreeComponent is shown in the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var component = (BbbtBehaviourTreeComponent)target;
            var tree = component.Tree;

            // Show the tree in the bbBT editor window.
            if (Application.isPlaying && GUILayout.Button("Open Behaviour Tree") && tree != null)
            {
                var window = EditorWindow.GetWindow<BbbtWindow>();
                window.LoadTree(tree);
                window.Show();
            }
        }
    }
}