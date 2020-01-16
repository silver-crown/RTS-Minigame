using System;
using moveen.core;
using moveen.descs;
using moveen.utils;
using UnityEngine;

namespace moveen.example {
    /// <summary>
    /// This component applies acceleration to the Rigidbody in order to reach target pos/rot. It contains its own speed representation so can be used without Rigidbody too.
    /// </summary>
    public class GlobalMotor : MoveenSkelBase {
        [Tooltip("Enable position following")]
        public bool move = true;
        [Tooltip("Movement dynamics")]
        public MotorBean posMotor;
        [NonSerialized] public Vector3 speed;

        [Tooltip("Enable rotation following")]
        public bool rotate = true;
        [Tooltip("Rotation dynamics")]
        public MotorBean rotationMotor;
        [NonSerialized] public Vector3 angleSpeed;

        [NonSerialized] public Rigidbody rb;

        //TODO can turn off local displacement
        private Vector3 initialLocalPos;//TODO more common

        public GlobalMotor() {
            participateInUpdate = false;
            participateInFixedUpdate = true;
        }

        public override void OnEnable() {
            base.OnEnable();
            rb = transform.GetComponent<Rigidbody>();
            initialLocalPos = transform.localPosition;
        }

        public override void fixedTick(float dt) {
            base.fixedTick(dt);

            if (target == null) {
                if (transform.parent == null) targetPos = initialLocalPos;
                else targetPos = transform.parent.TransformPoint(initialLocalPos);
            }

            if (!Application.isPlaying) return;
            if (rotate) {
                Quaternion curRotAbs = transform.rotation;
                if (rb == null) {
                    Vector3 rotAccel = Stepper5.torqueFromQuaternions(rotationMotor, curRotAbs, targetRot, angleSpeed, 1);
                    angleSpeed += rotAccel * dt;
                    Quaternion rotForce = MUtil.toAngleAxis(angleSpeed.length() * Time.deltaTime, angleSpeed.normalized);
                    transform.rotation = rotForce * curRotAbs;
                } else {
                    rb.AddTorque(Stepper5.torqueFromQuaternions(rotationMotor, curRotAbs, targetRot, rb.angularVelocity, 1), ForceMode.Acceleration);
                }
            }

            if (move) {
                Vector3 curPosAbs = transform.position;
                if (rb == null) {
                    Vector3 accel = posMotor.getAccel(targetPos, curPosAbs, speed, 1);
                    speed += accel * dt;
                    transform.position = curPosAbs + speed * dt;
                } else {
                    rb.AddForce(posMotor.getAccel(targetPos, curPosAbs, rb.velocity, 1), ForceMode.Acceleration);
                }
            }
        }

    }
}