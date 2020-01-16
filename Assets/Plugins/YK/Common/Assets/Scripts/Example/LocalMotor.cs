using System;
using moveen.core;
using UnityEngine;

namespace moveen.example {
    /// <summary>
    /// This component tries to reach target pos/rot in local space. It can be useful for local, physics-independent animation - barrel recoil, revolver, etc.
    /// TODO implement rotation
    /// </summary>
    public class LocalMotor : MonoBehaviour {

        [Tooltip("Enable position following")]
        public bool move = true;
        [Tooltip("Movement dynamics")]
        public MotorBean posMotor;
        [NonSerialized] public Vector3 localSpeed;
        [NonSerialized] private Vector3 initialLocalPos;//TODO more common

        [Tooltip("Enable rotation following")]
        public bool rotate = true;
        [Tooltip("Rotation dynamics (TODO implement)")]
        public MotorBean rotationMotor;
        [NonSerialized] private Quaternion initialLocalRot;//TODO more common

        public void OnEnable() {
            initialLocalPos = transform.localPosition;
            initialLocalRot = transform.localRotation;
        }

        private void FixedUpdate() {
            float dt = Time.deltaTime;
            if (!Application.isPlaying) return;
            //TODO implement
//            if (rotate) {
//                Quaternion curRotAbs = transform.rotation;
//                Vector3 rotAccel = Stepper5.torqueFromQuaternions(rotationMotor, curRotAbs, targetRot, angleSpeed, 1);
//                angleSpeed += rotAccel * dt;
//                Quaternion rotForce = MUtil.toAngleAxis(angleSpeed.length() * Time.deltaTime, angleSpeed.normalized);
//                transform.rotation = rotForce * curRotAbs;
//            }

            Vector3 localPos = transform.localPosition;
            if (move) {
                Vector3 accel = posMotor.getAccel(initialLocalPos, localPos, localSpeed, 1);
                localSpeed += accel * dt;
                localPos += localSpeed * dt;
                transform.localPosition = localPos;
            }
        }

    }
}