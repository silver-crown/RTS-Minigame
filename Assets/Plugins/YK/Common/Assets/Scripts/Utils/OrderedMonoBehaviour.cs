using System;
using System.Collections;
using moveen.descs;
using UnityEngine;

namespace moveen.utils {
    public abstract class OrderedMonoBehaviour : MonoBehaviour, IOrderableTick {
        public int executionOrder;
        public bool participateInUpdate = true;
        public bool participateInFixedUpdate = false;

        [NonSerialized] public CounterStack counterStack = new CounterStack(50);
        
        public int getOrder() {
            return executionOrder;
        }

        public abstract void tick(float dt);

        public virtual void fixedTick(float dt) {
            tick(dt);
        }

        public bool doParticipateInsUpdate() {
            return participateInUpdate;
        }

        public bool participateInPhysicsUpdate() {
            return participateInFixedUpdate;
        }

        public virtual void Update() {
            OrderedTick.onUnityUpdate();
        }

        public void LateUpdate() {
            OrderedTick.onUnityLateUpdate();
        }

        
        public static bool coroutineStarted;

        [NonSerialized] private bool coroutineOwner;

        public void FixedUpdate() {
            //commented because we want physics to update even when there are no physics participants
            //  can refactor if find a bad example
            //if (participateInFixedUpdate) {
                if (!coroutineStarted) StartCoroutine(startAfterPhysics());
                OrderedTick.onUnityFixedUpdate();
            //}
        }

        private IEnumerator startAfterPhysics() {
            coroutineStarted = true;
            coroutineOwner = true;
            while (true) {
                yield return new WaitForFixedUpdate();
                //commented because we want physics to update even when there are no physics participants
                //  can refactor if find a bad example
                //if (participateInFixedUpdate) {
                    OrderedTick.onUnityLateFixedUpdate();
                //}
            }
        }

        public virtual void OnEnable() {
            OrderedTick.addComponent(this);
        }
        
        public virtual void OnDisable() {
            OrderedTick.deleteComponent(this);
//            OrderedTick.deleteComponentFast(this);
            if (coroutineOwner) {//because its coroutines will stop working if GameObject is disabled (though they still work if only script is disabled, but we don't distinguish) 
                coroutineStarted = false;
                coroutineOwner = false;
            }
        }

        [NonSerialized] private float lastExecutionOrder;
        public virtual void OnValidate() {
            if (lastExecutionOrder != executionOrder) {
                lastExecutionOrder = executionOrder;
                OrderedTick.setUnsorted();
            }

        }
    }
}