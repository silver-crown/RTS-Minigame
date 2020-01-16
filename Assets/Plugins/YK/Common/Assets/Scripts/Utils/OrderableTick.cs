using moveen.descs;
using UnityEngine;

namespace moveen.utils {
    public class OrderableTick : IOrderableTick {
        public int executionOrder;
        public bool _participateInUpdate;
        public bool _participateInPhysicsUpdate;
        
        
        public int getOrder() {
            return executionOrder;
        }

        public virtual void tick(float dt) {
        }

        public virtual void fixedTick(float dt) {
        }

        public bool doParticipateInsUpdate() {
            return _participateInUpdate;
        }

        public bool participateInPhysicsUpdate() {
            return _participateInPhysicsUpdate;
        }
    }
}