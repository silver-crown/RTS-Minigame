using UnityEngine;

namespace moveen.example {
    public abstract class StartableMonoBehaviour : MonoBehaviour, Startable {
        public abstract void start();
    }
}