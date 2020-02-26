using UnityEditor;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Custom editor for BbbtBehaviourTreeCallerBehaviours
    /// </summary>
    [CustomEditor(typeof(BbbtBehaviourTreeCallerBehaviour))]
    public class BbbtBehaviourTreeCallerBehaviourEditor : Editor
    {
        /// <summary>
        /// Called when a BbbtBehaviourTreeCallerBehaviour is shown in the inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var behaviour = (BbbtBehaviourTreeCallerBehaviour)target;

            // Show the tree in the bbBT editor window.
            if (Application.isPlaying && GUILayout.Button("Open Behaviour Tree") && behaviour.BehaviourTree != null)
            {
                var window = EditorWindow.GetWindow<BbbtWindow>();
                window.LoadTree(behaviour.BehaviourTree);
                window.Show();
            }
        }
    }
}