using moveen.descs;
using moveen.utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace moveen.example {
    /// <summary>
    /// 
    /// </summary>
    public class DirControl : OrderedMonoBehaviour {
        [BindWarning]
        [Tooltip("Target GameObject to look at")]
        public Transform target;
        [FormerlySerializedAs("gunBody")]//24.11.17
        [Tooltip("MoveenSkelBase for which target orientation will be set. Best used with GlobalMotor. If not set - local GameObject will be searched for")]
        [BindOrLocalWarning]
        public MoveenSkelBase body;

        //TODO can take base orientation from an other object?   

        [Tooltip("Maximum allowed angle to rotate (degrees)")]
        public float maxAngle = 20;//degrees
        [Tooltip("If target is at larger angle than this, target will cease orientation and will return body to the initial position")]
        public float maxPrepareAngle = 40;//degrees
        [ReadOnly] public float differenceDegrees;
        private Quaternion initialLocalRot;


        public DirControl() {
            participateInFixedUpdate = true;
            participateInUpdate = false;
        }

        public override void OnEnable() {
            base.OnEnable();
            initialLocalRot = transform.localRotation;
            if (body == null) body = transform.GetComponent<MoveenSkelBase>();
        }

        public override void tick(float dt) {
            if (target == null || body == null) return;
            Quaternion bestDirAbs = MUtil.qToAxes(target.position - transform.position, Vector3.up);
            Quaternion baseRotAbs = transform.parent.rotation * initialLocalRot;
            Quaternion difference = baseRotAbs.rotSub(bestDirAbs);
            differenceDegrees = MyMath.abs(MyMath.angleNormalizeSigned(MyMath.acos(difference.w) * 2)) * 180f / MyMath.PI;
            if (differenceDegrees > maxPrepareAngle) bestDirAbs = baseRotAbs;//look forward if the angle is more than maxPrepareAngle
            else bestDirAbs = Quaternion.RotateTowards(baseRotAbs, bestDirAbs, maxAngle);//try to look to the target, but not more than maxAngle

            body.targetRot = bestDirAbs;
        }
    }
}