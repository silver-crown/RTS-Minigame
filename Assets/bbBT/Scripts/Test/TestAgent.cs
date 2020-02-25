using MoonSharp.Interpreter;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Used for testing the bbBT behaviour tree system.
    /// </summary>
    public class TestAgent : MonoBehaviour
    {
        private void Awake()
        {
            // Load the behaviour tree.
            var script = new Script();
            script.DoFile("test");
            string tree = script.Globals.Get("behaviourTree").String;
            GetComponent<BbbtBehaviourTreeComponent>().SetBehaviourTree(tree);
        }
    }
}