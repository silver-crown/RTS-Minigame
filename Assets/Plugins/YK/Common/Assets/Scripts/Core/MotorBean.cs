using moveen.utils;
using UnityEngine;

namespace moveen.core {
    [System.Serializable]
    public class MotorBean {
        [Tooltip("Coefficient: distance to wanted speed. (It is sometimes called \"Spring\")")]
        public float distanceToSpeed = 5;
        [Tooltip("Maximum wanted speed. Motor will try to slow down if speed is too high")]
        public float maxSpeed = 100;
        [Tooltip("Coefficient: wanted speed to acceleration.")]
        public float speedDifToAccel = 20;
        [Tooltip("Maximum applicable acceleration. Not the force. Corresponds to the power of the motor, but not depends on the mass of the connected body. So you can scale mass without fixing the motor, but you do want to scale this parameter if you want the motor to seem weaker or stronger")]
        public float maxAccel = 100;
        
        //maxAccelMultiplier - usually means count of legs participating in work
        public float getAccel(float targetPos, float curPos, float curSpeed, float maxAccelMultiplier) {
            return MyMath.clamp((MyMath.clamp((targetPos - curPos) * distanceToSpeed, maxSpeed) - curSpeed) * speedDifToAccel, maxAccel * maxAccelMultiplier);
        }

        public float getAccel(float targetPos, float curPos, float curSpeed, float externalAccel, float maxAccelMultiplier) {
            return MyMath.clamp((MyMath.clamp((targetPos - curPos) * distanceToSpeed, maxSpeed) - curSpeed) * speedDifToAccel + externalAccel, maxAccel * maxAccelMultiplier);
        }

        public Vector3 getAccel(Vector3 targetPos, Vector3 curPos, Vector3 curSpeed, float maxAccelMultiplier) {
            return targetPos
                .sub(curPos)
                .mul(distanceToSpeed)
                .limit(maxSpeed)
                .sub(curSpeed)
                .mul(speedDifToAccel)
                .limit(maxAccel * maxAccelMultiplier);
        }

        public void copyFrom(MotorBean other) {
            distanceToSpeed = other.distanceToSpeed;
            maxSpeed = other.maxSpeed;
            speedDifToAccel = other.speedDifToAccel;
            maxAccel = other.maxAccel;
        }

    }
    
}
