using UnityEngine;

namespace moveen.example {
    public class AnimationTrigger : StartableMonoBehaviour {

        public Animator animator;
        public string triggerName;

        private void OnEnable() {
            if (animator == null) animator = GetComponent<Animator>();
        }

        public override void start() {
            animator.SetTrigger(triggerName);
        }
    }
}