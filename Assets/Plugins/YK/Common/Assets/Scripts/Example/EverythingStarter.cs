using System;
using moveen.utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace moveen.example {
    /// <summary>
    /// Starts all Startable. Very handy to start different effects by event without scripting. For example - when step or fire is made.
    /// </summary>
    public class EverythingStarter : StartableMonoBehaviour {
        [NonSerialized] public ParticleSystem[] particleSystems;
        [NonSerialized] public Startable[] Startables;

        [Tooltip("If false - looks for startable in target GameObject only. If enabled - in its hierarchy too")]
        public bool lookInChildren;
        [FormerlySerializedAs("effects")]//24.11.17
        [Tooltip("Target GameObject. If null - considered this GameObject")]
        public Transform target;

        private void Awake() {
            Transform lookIn = target == null ? transform : target;
            if (lookInChildren) {
                particleSystems = lookIn.GetComponentsInChildren<ParticleSystem>();
                Startables = lookIn.GetComponentsInChildren<Startable>();
            } else {
                particleSystems = lookIn.GetComponents<ParticleSystem>();
                Startables = lookIn.GetComponents<Startable>();
            }
        }

        public override void start() {
            for (int i = 0; i < particleSystems.Length; i++) {
                particleSystems[i].Play();
            }
            for (int i = 0; i < Startables.Length; i++) {
                Startable startable = Startables[i];
                if (startable != this && ((MonoBehaviour)startable).isActiveAndEnabled) startable.start();
            }
        }
    }
}