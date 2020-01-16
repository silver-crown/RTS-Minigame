using UnityEngine;
using UnityEngine.Serialization;

namespace moveen.example {
    /// <summary>
    /// Used to just increase current speed of the LocalMotor. Example: recoil after a fire.
    /// </summary>
    public class RecoilLocalMotorStarter : MonoBehaviour, Startable {
        [Tooltip("Speed to add")]
        [FormerlySerializedAs("accel")]//24.11.17
        public Vector3 speedToAdd = Vector3.left;
        [Tooltip("Target GameObject with LocalMotor. If null - this GameObject will be used")]
        [FormerlySerializedAs("motor2")]//24.11.17
        public LocalMotor localMotor;

        private void OnEnable() {
            if (localMotor == null) localMotor = transform.GetComponent<LocalMotor>();
        }

        public void start() {
            if (localMotor == null) return;
            localMotor.localSpeed += speedToAdd;
        }
    }
}