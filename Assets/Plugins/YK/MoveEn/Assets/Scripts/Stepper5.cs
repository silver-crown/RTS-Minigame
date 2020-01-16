namespace moveen.core {
    using System;
    using System.Collections.Generic;
    using moveen.descs;
    using moveen.utils;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable] public class Stepper5 {

        [Tooltip("Logical right leg to calculate gait (even for multipeds)")] public int leadingLegRight = 0;
        [Tooltip("Logical left leg to calculate gait (even for multipeds)")] public int leadingLegLeft = 1;
        [Range(0,1)][Tooltip("Step phase. 0.5 - normal step. 0.1 - left right pause. 0.9 - right left pause")] public float phase = 0.5f;
        [Range(0,1)][Tooltip("The leg will try to get more support from behind")] public float lackOfSpeedCompensation = 0.1f;
        public float rewardSelf = 2;
        public float rewardOthers = 2;
        public float rewardPare = 5;
        [HideInInspector] public float runJumpTime = 0;
        [FormerlySerializedAs("forceBipedalEaryStep")] public bool forceBipedalEarlyStep;
        [Tooltip("Reduce foot entanglement for bipedals")] public bool bipedalForbidPlacement;
        [Tooltip("Protects the body from fall through. Must be enabled if no colliders is used")] public bool protectBodyFromFallthrough = true;
        [Tooltip("Ceiling height which will not be seen as a floor, through which it fell. Don't make it too small, as it is critical on steep slopes")] public float protectBodyFromFallthroughMaxHeight = 1;
        [Header("Body movement")][Range(0.5f,1.5f)][Tooltip("0.5 - lower body between lands, 1 - no lowering, 1.5 - higher between lands (unnatural)")] public float downOnStep = 0.7f;
        public MotorBean horizontalMotor;
        [FormerlySerializedAs("casualVerticalMotor")] public MotorBean verticalMotor;
        public MotorBean rotationMotor;
        [Header("Center Of Gravity simulation (important for certain gait)")][Tooltip("Center Of Gravity")] public float cogUpDown;
        [Range(-0.5f,0.5f)][Tooltip("Rotate around Center Of Gravity")] public float cogAngle = 0.2f;
        public float cogRotationMultiplier = 1;
        [FormerlySerializedAs("cogForce")][Tooltip("Push acceleration to compensate Center Of Gravity")] public float cogAccel = 10;
        [NonSerialized] public Vector3 calculatedCOG;
        [NonSerialized] public Vector3 calculatedCOGSpeed;
        [Header("Body helps or opposes legs position")][Range(-1,1)][Tooltip("Rotation for the body to help steps length or oppose (-1 - clumsy, +1 - agile)")] public float bodyLenHelp;
        [Tooltip("Body helps the length in movement only")] public bool bodyLenHelpAtSpeedOnly = true;
        [Tooltip("Speed at which maximum rotation is achieved")] public float bodyLenHelpMaxSpeed = 1;
        [Header("Hip")][Range(0,0.5f)][Tooltip("Hip flexibility relative to the body")] public float hipFlexibility;
        [HideInInspector] public Quaternion wantedHipRot;
        Quaternion slowLocalHipRot;
        [Tooltip("Hip position relative to the body (center of its rotation)")] public Vector3 hipPosRel = new Vector3(0, -0.5f, 0);
        [HideInInspector] public Vector3 hipPosAbs;
        [HideInInspector] public Quaternion hipRotAbs = Quaternion.identity;
        [NonSerialized] public bool doTickHip;
        [Header("_system")] public bool collectSteppingHistory;
        public bool showPhaseDials;
        [HideInInspector] public Quaternion projectedRot;
        [HideInInspector] public Vector3 realSpeed;
        [HideInInspector] public Vector3 g = new Vector3(0, -9.81f, 0);
        [HideInInspector] public Vector3 realBodyAngularSpeed;
        [HideInInspector][InstrumentalInfo] public Vector3 resultAcceleration;
        [HideInInspector][InstrumentalInfo] public Vector3 resultRotAcceleration;
        [HideInInspector] public Vector3 realBodyPos;
        [HideInInspector] public Quaternion realBodyRot = Quaternion.identity;
        [HideInInspector] public Vector3 projPos;
        [HideInInspector][InstrumentalInfo] public Vector3 inputWantedPos;
        [HideInInspector] public Quaternion inputWantedRot;
        [NonSerialized] public ISurfaceDetector surfaceDetector = new SurfaceDetectorStub();
        [NonSerialized] public List<MoveenSkelBase> legSkel = MUtil2.al<MoveenSkelBase>();
        [NonSerialized] public List<Step2> steps = MUtil2.al<Step2>();
        [NonSerialized] public Vector3 up = new Vector3(0, 1, 0);
        [HideInInspector][InstrumentalInfo] public Vector3 imCenter;
        [HideInInspector][InstrumentalInfo] public Vector3 imCenterSpeed;
        [HideInInspector][InstrumentalInfo] public Vector3 imCenterAngularSpeed;
        [HideInInspector][InstrumentalInfo] public Vector3 imBody;
        [HideInInspector][InstrumentalInfo] public Vector3 imBodySpeed;
        [HideInInspector][InstrumentalInfo] public Vector3 imActualCenterSpeed;
        [HideInInspector][InstrumentalInfo] public Vector3 speedLack;
        [HideInInspector][InstrumentalInfo] public Vector3 virtualForLegs;
        [HideInInspector][InstrumentalInfo] public float midLen;

        public Stepper5() {
            MUtil.logEvent(this, "constructor");
        }
        public virtual void setWantedPos(float dt, Vector3 wantedPos, Quaternion wantedRot) {
            this.inputWantedPos = wantedPos;
            this.inputWantedRot = wantedRot;
            this.projPos = this.project(this.realBodyPos);
            this.projectedRot = MUtil.qToAxes(ExtensionMethods.getXForVerticalAxis(ExtensionMethods.rotate(wantedRot, new Vector3(1, 0, 0)), this.up), this.up);
        }
        public virtual void tick(float dt) {
            for (int i = 0; (i < this.steps.Count); (i)++)  {
                Step2 step = this.steps[i];
                step.collectSteppingHistory = this.collectSteppingHistory;
                if (this.collectSteppingHistory)  {
                    step.paramHistory.next();
                }
            }
            this.tickHip(dt);
            this.calcAbs(dt);
            float p0 = this.steps[0].timedProgress;
            float p1 = this.steps[1].timedProgress;
            float fr = MyMath.fract((p1 - p0));
            if ((fr > this.phase))  {
                this.steps[0].beFaster = 0.5f;
                this.steps[1].beFaster = 0;
            } else  {
                if ((fr < this.phase))  {
                    this.steps[0].beFaster = 0;
                    this.steps[1].beFaster = 0.5f;
                }
            }
            this.steps[0].legSpeed *= (1 + this.steps[0].beFaster);
            this.steps[1].legSpeed *= (1 + this.steps[1].beFaster);
            this.tickSteps(dt);
        }
/*GENERATED*/        [Optimize]
/*GENERATED*/        void tickHip(float dt) {
/*GENERATED*/            this.midLen = ((this.steps[this.leadingLegLeft].maxLen + this.steps[this.leadingLegRight].maxLen) / 2);
/*GENERATED*/            Step2 right = this.steps[this.leadingLegRight];
/*GENERATED*/            Step2 left = this.steps[this.leadingLegLeft];
/*GENERATED*/            this.wantedHipRot = this.projectedRot;
/*GENERATED*/            Quaternion wantedRot = this.inputWantedRot;
/*GENERATED*/            int dockedCount = 0;
/*GENERATED*/            for (int i = 0; (i < this.steps.Count); i = (i + 1))  {
/*GENERATED*/                bool _920 = this.steps[i].dockedState;
/*GENERATED*/                if (_920)  {
/*GENERATED*/                    dockedCount = (dockedCount + 1);
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/            for (int i = 0; (i < this.steps.Count); i = (i + 1))  {
/*GENERATED*/                this.steps[i].canGoAir = (dockedCount == 0);
/*GENERATED*/            }
/*GENERATED*/            float additionalSpeed_x = 0;
/*GENERATED*/            float additionalSpeed_y = 0;
/*GENERATED*/            float additionalSpeed_z = 0;
/*GENERATED*/            float i1677_y = this.cogUpDown;
/*GENERATED*/            Quaternion i938_THIS = this.realBodyRot;
/*GENERATED*/            float i942_num1 = (i938_THIS.x * 2);
/*GENERATED*/            float i944_num3 = (i938_THIS.z * 2);
/*GENERATED*/            Vector3 localCOG = new Vector3();
/*GENERATED*/            localCOG.x = (((i938_THIS.x * (i938_THIS.y * 2)) + -(i938_THIS.w * i944_num3)) * i1677_y);
/*GENERATED*/            localCOG.y = ((1 + -((i938_THIS.x * i942_num1) + (i938_THIS.z * i944_num3))) * i1677_y);
/*GENERATED*/            localCOG.z = (((i938_THIS.y * i944_num3) + (i938_THIS.w * i942_num1)) * i1677_y);
/*GENERATED*/            Vector3 oldCalculatedCOG = this.calculatedCOG;
/*GENERATED*/            Vector3 i956_b = this.realBodyPos;
/*GENERATED*/            this.calculatedCOG.x = (localCOG.x + i956_b.x);
/*GENERATED*/            this.calculatedCOG.y = (localCOG.y + i956_b.y);
/*GENERATED*/            this.calculatedCOG.z = (localCOG.z + i956_b.z);
/*GENERATED*/            Vector3 i957_a = this.calculatedCOG;
/*GENERATED*/            Vector3 i965_a = this.calculatedCOGSpeed;
/*GENERATED*/            this.calculatedCOGSpeed.x = (((((i957_a.x + -oldCalculatedCOG.x) / dt) + -i965_a.x) * 0.1f) + i965_a.x);
/*GENERATED*/            this.calculatedCOGSpeed.y = (((((i957_a.y + -oldCalculatedCOG.y) / dt) + -i965_a.y) * 0.1f) + i965_a.y);
/*GENERATED*/            this.calculatedCOGSpeed.z = (((((i957_a.z + -oldCalculatedCOG.z) / dt) + -i965_a.z) * 0.1f) + i965_a.z);
/*GENERATED*/            for (int i = 0; (i < this.steps.Count); i = (i + 1))  {
/*GENERATED*/                Step2 step = this.steps[i];
/*GENERATED*/                Vector3 i1126_a = step.comfortPosRel;
/*GENERATED*/                Vector3 i1127_b = this.up;
/*GENERATED*/                Vector3 rollAxis = new Vector3(
/*GENERATED*/                    (float)(((i1126_a.y * i1127_b.z) + -(i1126_a.z * i1127_b.y))), 
/*GENERATED*/                    (float)(((i1126_a.z * i1127_b.x) + -(i1126_a.x * i1127_b.z))), 
/*GENERATED*/                    (float)(((i1126_a.x * i1127_b.y) + -(i1126_a.y * i1127_b.x))));
/*GENERATED*/                bool _921 = (!step.dockedState && !step.wasTooLong);
/*GENERATED*/                if (_921)  {
/*GENERATED*/                    Quaternion rollQuaternion = Quaternion.AngleAxis((float)(((this.cogAngle / Math.PI) * 180)), rollAxis);
/*GENERATED*/                    Vector3 disp = rotDisp(rollQuaternion, -localCOG).withSetY(0);
/*GENERATED*/                    Quaternion i1132_THIS = this.realBodyRot;
/*GENERATED*/                    float i1136_num1 = (i1132_THIS.x * 2);
/*GENERATED*/                    float i1137_num2 = (i1132_THIS.y * 2);
/*GENERATED*/                    float i1138_num3 = (i1132_THIS.z * 2);
/*GENERATED*/                    float i1139_num4 = (i1132_THIS.x * i1136_num1);
/*GENERATED*/                    float i1140_num5 = (i1132_THIS.y * i1137_num2);
/*GENERATED*/                    float i1141_num6 = (i1132_THIS.z * i1138_num3);
/*GENERATED*/                    float i1142_num7 = (i1132_THIS.x * i1137_num2);
/*GENERATED*/                    float i1143_num8 = (i1132_THIS.x * i1138_num3);
/*GENERATED*/                    float i1144_num9 = (i1132_THIS.y * i1138_num3);
/*GENERATED*/                    float i1145_num10 = (i1132_THIS.w * i1136_num1);
/*GENERATED*/                    float i1146_num11 = (i1132_THIS.w * i1137_num2);
/*GENERATED*/                    float i1147_num12 = (i1132_THIS.w * i1138_num3);
/*GENERATED*/                    float i1150_d = this.cogAccel;
/*GENERATED*/                    additionalSpeed_x = (
/*GENERATED*/                        additionalSpeed_x + 
/*GENERATED*/                        (
/*GENERATED*/                        (
/*GENERATED*/                        ((1 + -(i1140_num5 + i1141_num6)) * disp.x) + 
/*GENERATED*/                        ((i1142_num7 + -i1147_num12) * disp.y) + 
/*GENERATED*/                        ((i1143_num8 + i1146_num11) * disp.z)) * 
/*GENERATED*/                        i1150_d));
/*GENERATED*/                    additionalSpeed_y = (
/*GENERATED*/                        additionalSpeed_y + 
/*GENERATED*/                        (
/*GENERATED*/                        (
/*GENERATED*/                        ((i1142_num7 + i1147_num12) * disp.x) + 
/*GENERATED*/                        ((1 + -(i1139_num4 + i1141_num6)) * disp.y) + 
/*GENERATED*/                        ((i1144_num9 + -i1145_num10) * disp.z)) * 
/*GENERATED*/                        i1150_d));
/*GENERATED*/                    additionalSpeed_z = (
/*GENERATED*/                        additionalSpeed_z + 
/*GENERATED*/                        (
/*GENERATED*/                        (
/*GENERATED*/                        ((i1143_num8 + -i1146_num11) * disp.x) + 
/*GENERATED*/                        ((i1144_num9 + i1145_num10) * disp.y) + 
/*GENERATED*/                        ((1 + -(i1139_num4 + i1140_num5)) * disp.z)) * 
/*GENERATED*/                        i1150_d));
/*GENERATED*/                    bool _922 = (this.cogRotationMultiplier != 1);
/*GENERATED*/                    if (_922)  {
/*GENERATED*/                        Quaternion newVar_885 = Quaternion.AngleAxis((float)((((this.cogAngle * this.cogRotationMultiplier) / Math.PI) * 180)), rollAxis);
/*GENERATED*/                        rollQuaternion = newVar_885;
/*GENERATED*/                    }
/*GENERATED*/                    float i1153_lhs_x_1983 = wantedRot.x;
/*GENERATED*/                    float i1153_lhs_y_1984 = wantedRot.y;
/*GENERATED*/                    float i1153_lhs_z_1985 = wantedRot.z;
/*GENERATED*/                    float i1153_lhs_w_1986 = wantedRot.w;
/*GENERATED*/                    float i1154_rhs_x_1987 = rollQuaternion.x;
/*GENERATED*/                    float i1154_rhs_y_1988 = rollQuaternion.y;
/*GENERATED*/                    float i1154_rhs_z_1989 = rollQuaternion.z;
/*GENERATED*/                    float i1154_rhs_w_1990 = rollQuaternion.w;
/*GENERATED*/                    wantedRot.x = (
/*GENERATED*/                        (i1153_lhs_w_1986 * i1154_rhs_x_1987) + 
/*GENERATED*/                        (i1153_lhs_x_1983 * i1154_rhs_w_1990) + 
/*GENERATED*/                        (i1153_lhs_y_1984 * i1154_rhs_z_1989) + 
/*GENERATED*/                        -(i1153_lhs_z_1985 * i1154_rhs_y_1988));
/*GENERATED*/                    wantedRot.y = (
/*GENERATED*/                        (i1153_lhs_w_1986 * i1154_rhs_y_1988) + 
/*GENERATED*/                        (i1153_lhs_y_1984 * i1154_rhs_w_1990) + 
/*GENERATED*/                        (i1153_lhs_z_1985 * i1154_rhs_x_1987) + 
/*GENERATED*/                        -(i1153_lhs_x_1983 * i1154_rhs_z_1989));
/*GENERATED*/                    wantedRot.z = (
/*GENERATED*/                        (i1153_lhs_w_1986 * i1154_rhs_z_1989) + 
/*GENERATED*/                        (i1153_lhs_z_1985 * i1154_rhs_w_1990) + 
/*GENERATED*/                        (i1153_lhs_x_1983 * i1154_rhs_y_1988) + 
/*GENERATED*/                        -(i1153_lhs_y_1984 * i1154_rhs_x_1987));
/*GENERATED*/                    wantedRot.w = (
/*GENERATED*/                        (i1153_lhs_w_1986 * i1154_rhs_w_1990) + 
/*GENERATED*/                        -(i1153_lhs_x_1983 * i1154_rhs_x_1987) + 
/*GENERATED*/                        -(i1153_lhs_y_1984 * i1154_rhs_y_1988) + 
/*GENERATED*/                        -(i1153_lhs_z_1985 * i1154_rhs_z_1989));
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/            float baseY = 0;
/*GENERATED*/            for (int i = 0; (i < this.steps.Count); (i)++) baseY = (baseY + this.steps[i].bestTargetConservativeAbs.y);
/*GENERATED*/            float baseY_907 = (baseY / this.steps.Count);
/*GENERATED*/            float maxY = float.MinValue;
/*GENERATED*/            for (int i = 0; (i < this.steps.Count); (i)++) if ((!this.steps[i].dockedState && (maxY < this.steps[i].posAbs.y)))  {
/*GENERATED*/                maxY = this.steps[i].posAbs.y;
/*GENERATED*/            }
/*GENERATED*/            float minDeviation = 1;
/*GENERATED*/            float maxDeviation = 0;
/*GENERATED*/            float commonProgress = 0;
/*GENERATED*/            for (int i = 0; (i < this.steps.Count); (i)++)  {
/*GENERATED*/                Step2 step = this.steps[i];
/*GENERATED*/                float dd = (step.deviation / step.comfortRadius);
/*GENERATED*/                bool _923 = !step.dockedState;
/*GENERATED*/                if (_923)  {
/*GENERATED*/                    commonProgress = (commonProgress + step.progress);
/*GENERATED*/                } else  {
/*GENERATED*/                     {
/*GENERATED*/                        bool _924 = (dd < minDeviation);
/*GENERATED*/                        if (_924)  {
/*GENERATED*/                            minDeviation = dd;
/*GENERATED*/                        }
/*GENERATED*/                        bool _925 = (dd > maxDeviation);
/*GENERATED*/                        if (_925)  {
/*GENERATED*/                            maxDeviation = dd;
/*GENERATED*/                        }
/*GENERATED*/                    }
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/            float i978_dstTo = this.downOnStep;
/*GENERATED*/            float i979_res = ((((maxDeviation + (commonProgress * 0.5f)) / 1) * (i978_dstTo + -1)) + 1);
/*GENERATED*/            float newVar_841;
/*GENERATED*/            bool _1658 = (1 < i978_dstTo);
/*GENERATED*/            if (_1658)  {
/*GENERATED*/                float i1818_arg1;
/*GENERATED*/                bool _2035 = (i979_res < i978_dstTo);
/*GENERATED*/                if (_2035)  {
/*GENERATED*/                    i1818_arg1 = i979_res;
/*GENERATED*/                } else  {
/*GENERATED*/                    i1818_arg1 = i978_dstTo;
/*GENERATED*/                }
/*GENERATED*/                newVar_841 = (((1 > i1818_arg1)) ? (1) : (i1818_arg1));
/*GENERATED*/            } else  {
/*GENERATED*/                float i1825_arg1;
/*GENERATED*/                bool _2036 = (i979_res < 1);
/*GENERATED*/                if (_2036)  {
/*GENERATED*/                    i1825_arg1 = i979_res;
/*GENERATED*/                } else  {
/*GENERATED*/                    i1825_arg1 = 1;
/*GENERATED*/                }
/*GENERATED*/                newVar_841 = (((i978_dstTo > i1825_arg1)) ? (i978_dstTo) : (i1825_arg1));
/*GENERATED*/            }
/*GENERATED*/            float i986_to = this.inputWantedPos.y;
/*GENERATED*/            float i987_progress = newVar_841;
/*GENERATED*/            float futureHeightDif;
/*GENERATED*/            bool _913 = ((this.runJumpTime > 0) && (dockedCount == 1));
/*GENERATED*/            if (_913)  {
/*GENERATED*/                float newVar_1492 = (this.realSpeed.y * this.runJumpTime);
/*GENERATED*/                futureHeightDif = (newVar_1492 + -((9.81f * this.runJumpTime * this.runJumpTime) / 2));
/*GENERATED*/            } else  {
/*GENERATED*/                futureHeightDif = 0;
/*GENERATED*/            }
/*GENERATED*/            Vector3 i988_a = this.inputWantedPos;
/*GENERATED*/            Vector3 i990_a = this.realBodyPos;
/*GENERATED*/            Vector3 newVar_844 = new Vector3((i988_a.x + -i990_a.x), 0, (i988_a.z + -i990_a.z));
/*GENERATED*/            Vector3 newVar_847 = new Vector3(additionalSpeed_x, 0, additionalSpeed_z);
/*GENERATED*/            this.realWantedSpeed = newVar_844.mul(this.horizontalMotor.distanceToSpeed).limit(this.horizontalMotor.maxSpeed).add(newVar_847);
/*GENERATED*/            Vector3 i998_a = this.inputWantedPos;
/*GENERATED*/            Vector3 i1000_a = this.imCenter;
/*GENERATED*/            Vector3 newVar_848 = new Vector3((i998_a.x + -i1000_a.x), 0, (i998_a.z + -i1000_a.z));
/*GENERATED*/            Vector3 imCenterWantedSpeed = newVar_848.mul(this.horizontalMotor.distanceToSpeed).limit(this.horizontalMotor.maxSpeed);
/*GENERATED*/            Vector3 i1006_a = this.imCenterSpeed;
/*GENERATED*/            Vector3 newVar_851 = new Vector3((imCenterWantedSpeed.x + -i1006_a.x), imCenterWantedSpeed.y, (imCenterWantedSpeed.z + -i1006_a.z));
/*GENERATED*/            Vector3 imCenterAccel = newVar_851.mul(this.horizontalMotor.speedDifToAccel);
/*GENERATED*/            float mult_909 = dockedCount;
/*GENERATED*/            Vector3 i1012_a = this.realSpeed;
/*GENERATED*/            Vector3 i1014_a = this.realWantedSpeed;
/*GENERATED*/            Vector3 newVar_853 = new Vector3((i1014_a.x + -i1012_a.x), i1014_a.y, (i1014_a.z + -i1012_a.z));
/*GENERATED*/            float newVar_855 = (this.realBodyPos.y + futureHeightDif);
/*GENERATED*/            Vector3 realAccel = newVar_853.mul(this.horizontalMotor.speedDifToAccel).limit((this.horizontalMotor.maxAccel * mult_909)).add(
/*GENERATED*/                0, 
/*GENERATED*/                this.verticalMotor.getAccel(
/*GENERATED*/                ((baseY_907 * (1 + -i987_progress)) + (i986_to * i987_progress)), 
/*GENERATED*/                newVar_855, 
/*GENERATED*/                this.realSpeed.y, 
/*GENERATED*/                9.81f, 
/*GENERATED*/                dockedCount), 
/*GENERATED*/                0);
/*GENERATED*/            Vector3 oldImCenterPos = this.imCenter;
/*GENERATED*/            Vector3 imCenterAccel_910 = imCenterAccel.limit((this.horizontalMotor.maxAccel * mult_909));
/*GENERATED*/            Vector3 i1020_a = this.imCenterSpeed;
/*GENERATED*/            this.imCenterSpeed.x = (i1020_a.x + (imCenterAccel_910.x * dt));
/*GENERATED*/            this.imCenterSpeed.y = (i1020_a.y + (imCenterAccel_910.y * dt));
/*GENERATED*/            this.imCenterSpeed.z = (i1020_a.z + (imCenterAccel_910.z * dt));
/*GENERATED*/            this.imCenterSpeed.y = this.realSpeed.y;
/*GENERATED*/            this.imCenter.y = this.realBodyPos.y;
/*GENERATED*/            Vector3 i1022_a = this.imCenterSpeed;
/*GENERATED*/            Vector3 i1024_a = this.imCenter;
/*GENERATED*/            this.imCenter.x = (i1024_a.x + (i1022_a.x * dt));
/*GENERATED*/            this.imCenter.y = (i1024_a.y + (i1022_a.y * dt));
/*GENERATED*/            this.imCenter.z = (i1024_a.z + (i1022_a.z * dt));
/*GENERATED*/            Vector3 i1026_a = this.imCenter;
/*GENERATED*/            Vector3 i1027_b = this.imBody;
/*GENERATED*/            this.imCenter.x = (((i1027_b.x + -i1026_a.x) * 0.05f) + i1026_a.x);
/*GENERATED*/            this.imCenter.y = (((i1027_b.y + -i1026_a.y) * 0.05f) + i1026_a.y);
/*GENERATED*/            this.imCenter.z = (((i1027_b.z + -i1026_a.z) * 0.05f) + i1026_a.z);
/*GENERATED*/            Vector3 oldImBodyPos = this.imBody;
/*GENERATED*/            Vector3 i1035_a = this.imBodySpeed;
/*GENERATED*/            Vector3 i1037_a = this.realWantedSpeed;
/*GENERATED*/            Vector3 newVar_861 = new Vector3((i1037_a.x + -i1035_a.x), i1037_a.y, (i1037_a.z + -i1035_a.z));
/*GENERATED*/            this.imBodySpeed = (
/*GENERATED*/                this.imBodySpeed + 
/*GENERATED*/                (
/*GENERATED*/                newVar_861.mul(this.horizontalMotor.speedDifToAccel).limit((this.horizontalMotor.maxAccel * mult_909)) * 
/*GENERATED*/                dt));
/*GENERATED*/            Vector3 i1041_a = this.imBodySpeed;
/*GENERATED*/            Vector3 i1043_a = this.imBody;
/*GENERATED*/            this.imBody.x = (i1043_a.x + (i1041_a.x * dt));
/*GENERATED*/            this.imBody.y = (i1043_a.y + (i1041_a.y * dt));
/*GENERATED*/            this.imBody.z = (i1043_a.z + (i1041_a.z * dt));
/*GENERATED*/            Vector3 i1045_a = this.realBodyPos;
/*GENERATED*/            Vector3 i1046_b = this.imBody;
/*GENERATED*/            float newVar_1336 = ((i1045_a.x + -i1046_b.x) * 0.1f);
/*GENERATED*/            float newVar_1337 = ((i1045_a.y + -i1046_b.y) * 0.1f);
/*GENERATED*/            float newVar_1338 = ((i1045_a.z + -i1046_b.z) * 0.1f);
/*GENERATED*/            Vector3 i1051_a = this.imBody;
/*GENERATED*/            this.imBody.x = (i1051_a.x + newVar_1336);
/*GENERATED*/            this.imBody.y = (i1051_a.y + newVar_1337);
/*GENERATED*/            this.imBody.z = (i1051_a.z + newVar_1338);
/*GENERATED*/            Vector3 i1053_a = this.imCenter;
/*GENERATED*/            this.imCenter.x = (i1053_a.x + newVar_1336);
/*GENERATED*/            this.imCenter.y = (i1053_a.y + newVar_1337);
/*GENERATED*/            this.imCenter.z = (i1053_a.z + newVar_1338);
/*GENERATED*/            Vector3 i1055_a = this.imBody;
/*GENERATED*/            this.imBodySpeed.x = ((i1055_a.x + -oldImBodyPos.x) / dt);
/*GENERATED*/            this.imBodySpeed.y = ((i1055_a.y + -oldImBodyPos.y) / dt);
/*GENERATED*/            this.imBodySpeed.z = ((i1055_a.z + -oldImBodyPos.z) / dt);
/*GENERATED*/            Vector3 i1063_a = this.imCenter;
/*GENERATED*/            this.imActualCenterSpeed.x = ((i1063_a.x + -oldImCenterPos.x) / dt);
/*GENERATED*/            this.imActualCenterSpeed.y = ((i1063_a.y + -oldImCenterPos.y) / dt);
/*GENERATED*/            this.imActualCenterSpeed.z = ((i1063_a.z + -oldImCenterPos.z) / dt);
/*GENERATED*/            Vector3 i1071_a = this.imBody;
/*GENERATED*/            Vector3 i1072_b = this.realBodyPos;
/*GENERATED*/            float newVar_1357 = (i1071_a.x + -i1072_b.x);
/*GENERATED*/            float newVar_1358 = (i1071_a.y + -i1072_b.y);
/*GENERATED*/            float newVar_1359 = (i1071_a.z + -i1072_b.z);
/*GENERATED*/            bool _914 = (
/*GENERATED*/                Mathf.Sqrt((float)(((newVar_1357 * newVar_1357) + (newVar_1358 * newVar_1358) + (newVar_1359 * newVar_1359)))) > 
/*GENERATED*/                5);
/*GENERATED*/            if (_914)  {
/*GENERATED*/                this.imCenter = this.realBodyPos;
/*GENERATED*/                this.imBody = this.realBodyPos;
/*GENERATED*/                this.imCenterSpeed = this.realSpeed;
/*GENERATED*/                this.imBodySpeed = this.realSpeed;
/*GENERATED*/            }
/*GENERATED*/            Vector3 i1076_a = this.imCenter;
/*GENERATED*/            Vector3 i1077_b = this.imBody;
/*GENERATED*/            float newVar_1365 = (i1076_a.x + -i1077_b.x);
/*GENERATED*/            float newVar_1366 = (i1076_a.y + -i1077_b.y);
/*GENERATED*/            float newVar_1367 = (i1076_a.z + -i1077_b.z);
/*GENERATED*/            bool _915 = (
/*GENERATED*/                Mathf.Sqrt((float)(((newVar_1365 * newVar_1365) + (newVar_1366 * newVar_1366) + (newVar_1367 * newVar_1367)))) > 
/*GENERATED*/                (this.cogAccel * 0.3f));
/*GENERATED*/            if (_915)  {
/*GENERATED*/                this.imCenter = this.imBody;
/*GENERATED*/                this.imCenterSpeed = this.imBodySpeed;
/*GENERATED*/            }
/*GENERATED*/            Vector3 i1081_a = this.inputWantedPos;
/*GENERATED*/            Vector3 i1082_b = this.imCenter;
/*GENERATED*/            Vector3 newVar_872 = new Vector3((i1081_a.x + -i1082_b.x), (i1081_a.y + -i1082_b.y), (i1081_a.z + -i1082_b.z));
/*GENERATED*/            Vector3 speedDif = (this.imActualCenterSpeed - (newVar_872 * this.horizontalMotor.distanceToSpeed));
/*GENERATED*/            Vector3 speedDif_911 = speedDif.limit(this.horizontalMotor.maxSpeed);
/*GENERATED*/            Vector3 i1083_a = new Vector3();
/*GENERATED*/            i1083_a.x = additionalSpeed_x;
/*GENERATED*/            i1083_a.y = additionalSpeed_y;
/*GENERATED*/            i1083_a.z = additionalSpeed_z;
/*GENERATED*/            Vector3 addNormalized = Vector3.Normalize(i1083_a);
/*GENERATED*/            float i1084_a_x_1941 = additionalSpeed_x;
/*GENERATED*/            float i1084_a_y_1942 = additionalSpeed_y;
/*GENERATED*/            float i1084_a_z_1943 = additionalSpeed_z;
/*GENERATED*/            float adLen = Mathf.Sqrt((float)(((i1084_a_x_1941 * i1084_a_x_1941) + (i1084_a_y_1942 * i1084_a_y_1942) + (i1084_a_z_1943 * i1084_a_z_1943))));
/*GENERATED*/            float proj = (
/*GENERATED*/                (speedDif_911.x * addNormalized.x) + 
/*GENERATED*/                (speedDif_911.y * addNormalized.y) + 
/*GENERATED*/                (speedDif_911.z * addNormalized.z));
/*GENERATED*/            bool _916 = (proj > adLen);
/*GENERATED*/            if (_916)  {
/*GENERATED*/                proj = (proj + -adLen);
/*GENERATED*/            } else  {
/*GENERATED*/                bool _926 = (proj < -adLen);
/*GENERATED*/                if (_926)  {
/*GENERATED*/                    proj = (proj + adLen);
/*GENERATED*/                } else  {
/*GENERATED*/                    proj = 0;
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/            float i1095_b = (
/*GENERATED*/                (addNormalized.x * speedDif_911.x) + 
/*GENERATED*/                (addNormalized.y * speedDif_911.y) + 
/*GENERATED*/                (addNormalized.z * speedDif_911.z));
/*GENERATED*/            float i1103_b = proj;
/*GENERATED*/            Vector3 newVar_873 = new Vector3(
/*GENERATED*/                (speedDif_911.x + -(addNormalized.x * i1095_b) + (addNormalized.x * i1103_b)), 
/*GENERATED*/                (speedDif_911.y + -(addNormalized.y * i1095_b) + (addNormalized.y * i1103_b)), 
/*GENERATED*/                (speedDif_911.z + -(addNormalized.z * i1095_b) + (addNormalized.z * i1103_b)));
/*GENERATED*/            float newVar_878 = (this.midLen * this.lackOfSpeedCompensation);
/*GENERATED*/            float i1112_l = Mathf.Sqrt((float)(((newVar_873.x * newVar_873.x) + (newVar_873.y * newVar_873.y) + (newVar_873.z * newVar_873.z))));
/*GENERATED*/            Vector3 newVar_877;
/*GENERATED*/            bool _1659 = (i1112_l > newVar_878);
/*GENERATED*/            if (_1659)  {
/*GENERATED*/                Vector3 i1830_a = Vector3.Normalize(newVar_873);
/*GENERATED*/                Vector3 newVar_1832 = new Vector3((i1830_a.x * newVar_878), (i1830_a.y * newVar_878), (i1830_a.z * newVar_878));
/*GENERATED*/                newVar_877 = newVar_1832;
/*GENERATED*/            } else  {
/*GENERATED*/                newVar_877 = newVar_873;
/*GENERATED*/            }
/*GENERATED*/            Vector3 i1115_a = this.speedLack;
/*GENERATED*/            this.speedLack.x = (((newVar_877.x + -i1115_a.x) * 0.1f) + i1115_a.x);
/*GENERATED*/            this.speedLack.y = (((newVar_877.y + -i1115_a.y) * 0.1f) + i1115_a.y);
/*GENERATED*/            this.speedLack.z = (((newVar_877.z + -i1115_a.z) * 0.1f) + i1115_a.z);
/*GENERATED*/            this.speedLack.y = 0;
/*GENERATED*/            Vector3 i1124_a = this.imCenter;
/*GENERATED*/            Vector3 i1125_b = this.speedLack;
/*GENERATED*/            this.virtualForLegs.x = (i1124_a.x + i1125_b.x);
/*GENERATED*/            this.virtualForLegs.y = (i1124_a.y + i1125_b.y);
/*GENERATED*/            this.virtualForLegs.z = (i1124_a.z + i1125_b.z);
/*GENERATED*/            bool _917 = (realAccel.y > 0);
/*GENERATED*/            if (_917)  {
/*GENERATED*/                this.resultAcceleration = realAccel;
/*GENERATED*/            } else  {
/*GENERATED*/                this.resultAcceleration.x = 0;
/*GENERATED*/                this.resultAcceleration.y = 0;
/*GENERATED*/                this.resultAcceleration.z = 0;
/*GENERATED*/            }
/*GENERATED*/            bool _918 = ((this.bodyLenHelp != 0) && (this.steps.Count > 1));
/*GENERATED*/            if (_918)  {
/*GENERATED*/                Vector3 i1157_a = right.posAbs;
/*GENERATED*/                Vector3 i1158_b = left.posAbs;
/*GENERATED*/                Vector3 newVar_891 = new Vector3((i1157_a.x + -i1158_b.x), 0, (i1157_a.z + -i1158_b.z));
/*GENERATED*/                Vector3 s0to1Proj = Vector3.Normalize(newVar_891);
/*GENERATED*/                float i1162_THIS_y_2002 = wantedRot.y;
/*GENERATED*/                float i1162_THIS_z_2003 = wantedRot.z;
/*GENERATED*/                float i1167_num2 = (i1162_THIS_y_2002 * 2);
/*GENERATED*/                float i1168_num3 = (i1162_THIS_z_2003 * 2);
/*GENERATED*/                Vector3 newVar_893 = new Vector3(
/*GENERATED*/                    (1 + -((i1162_THIS_y_2002 * i1167_num2) + (i1162_THIS_z_2003 * i1168_num3))), 
/*GENERATED*/                    0, 
/*GENERATED*/                    ((wantedRot.x * i1168_num3) + -(wantedRot.w * i1167_num2)));
/*GENERATED*/                Vector3 curLook = Vector3.Normalize(newVar_893);
/*GENERATED*/                float speedDump;
/*GENERATED*/                bool _927 = this.bodyLenHelpAtSpeedOnly;
/*GENERATED*/                if (_927)  {
/*GENERATED*/                    Vector3 i1195_a = this.inputWantedPos;
/*GENERATED*/                    Vector3 i1196_b = this.realBodyPos;
/*GENERATED*/                    float newVar_1559 = (i1195_a.x + -i1196_b.x);
/*GENERATED*/                    float newVar_1560 = (i1195_a.y + -i1196_b.y);
/*GENERATED*/                    float newVar_1561 = (i1195_a.z + -i1196_b.z);
/*GENERATED*/                    float i1191_value = Mathf.Sqrt((float)(((newVar_1559 * newVar_1559) + (newVar_1560 * newVar_1560) + (newVar_1561 * newVar_1561))));
/*GENERATED*/                    float i1188_value = (i1191_value / this.bodyLenHelpMaxSpeed);
/*GENERATED*/                    float i1199_arg1;
/*GENERATED*/                    bool _1660 = (i1188_value < 1);
/*GENERATED*/                    if (_1660)  {
/*GENERATED*/                        i1199_arg1 = i1188_value;
/*GENERATED*/                    } else  {
/*GENERATED*/                        i1199_arg1 = 1;
/*GENERATED*/                    }
/*GENERATED*/                    speedDump = (((0 > i1199_arg1)) ? (0) : (i1199_arg1));
/*GENERATED*/                } else  {
/*GENERATED*/                    speedDump = 1;
/*GENERATED*/                }
/*GENERATED*/                Quaternion newVar_897 = Quaternion.AngleAxis(
/*GENERATED*/                    (float)((
/*GENERATED*/                    (
/*GENERATED*/                    (
/*GENERATED*/                    ((curLook.x * s0to1Proj.x) + (curLook.y * s0to1Proj.y) + (curLook.z * s0to1Proj.z)) * 
/*GENERATED*/                    this.bodyLenHelp * 
/*GENERATED*/                    speedDump) / 
/*GENERATED*/                    Math.PI) * 
/*GENERATED*/                    180)), 
/*GENERATED*/                    this.up);
/*GENERATED*/                float i1186_lhs_x_2011 = wantedRot.x;
/*GENERATED*/                float i1186_lhs_y_2012 = wantedRot.y;
/*GENERATED*/                float i1186_lhs_z_2013 = wantedRot.z;
/*GENERATED*/                float i1186_lhs_w_2014 = wantedRot.w;
/*GENERATED*/                wantedRot.x = (
/*GENERATED*/                    (i1186_lhs_w_2014 * newVar_897.x) + 
/*GENERATED*/                    (i1186_lhs_x_2011 * newVar_897.w) + 
/*GENERATED*/                    (i1186_lhs_y_2012 * newVar_897.z) + 
/*GENERATED*/                    -(i1186_lhs_z_2013 * newVar_897.y));
/*GENERATED*/                wantedRot.y = (
/*GENERATED*/                    (i1186_lhs_w_2014 * newVar_897.y) + 
/*GENERATED*/                    (i1186_lhs_y_2012 * newVar_897.w) + 
/*GENERATED*/                    (i1186_lhs_z_2013 * newVar_897.x) + 
/*GENERATED*/                    -(i1186_lhs_x_2011 * newVar_897.z));
/*GENERATED*/                wantedRot.z = (
/*GENERATED*/                    (i1186_lhs_w_2014 * newVar_897.z) + 
/*GENERATED*/                    (i1186_lhs_z_2013 * newVar_897.w) + 
/*GENERATED*/                    (i1186_lhs_x_2011 * newVar_897.y) + 
/*GENERATED*/                    -(i1186_lhs_y_2012 * newVar_897.x));
/*GENERATED*/                wantedRot.w = (
/*GENERATED*/                    (i1186_lhs_w_2014 * newVar_897.w) + 
/*GENERATED*/                    -(i1186_lhs_x_2011 * newVar_897.x) + 
/*GENERATED*/                    -(i1186_lhs_y_2012 * newVar_897.y) + 
/*GENERATED*/                    -(i1186_lhs_z_2013 * newVar_897.z));
/*GENERATED*/            }
/*GENERATED*/            this.resultRotAcceleration = torqueFromQuaternions(
/*GENERATED*/                this.rotationMotor, 
/*GENERATED*/                this.realBodyRot, 
/*GENERATED*/                wantedRot, 
/*GENERATED*/                this.realBodyAngularSpeed, 
/*GENERATED*/                ((float)(dockedCount) / this.steps.Count));
/*GENERATED*/            bool _919 = this.doTickHip;
/*GENERATED*/            if (_919)  {
/*GENERATED*/                this.calcHipLenHelp();
/*GENERATED*/                Quaternion i1202_THIS = this.wantedHipRot;
/*GENERATED*/                Quaternion i1203_from = this.realBodyRot;
/*GENERATED*/                float newVar_1569 = -i1203_from.x;
/*GENERATED*/                float newVar_1570 = -i1203_from.y;
/*GENERATED*/                float newVar_1571 = -i1203_from.z;
/*GENERATED*/                float i1675_w = i1203_from.w;
/*GENERATED*/                Quaternion newVar_902 = new Quaternion((
/*GENERATED*/                    (i1202_THIS.w * newVar_1569) + 
/*GENERATED*/                    (i1202_THIS.x * i1675_w) + 
/*GENERATED*/                    (i1202_THIS.y * newVar_1571) + 
/*GENERATED*/                    -(i1202_THIS.z * newVar_1570)), (
/*GENERATED*/                    (i1202_THIS.w * newVar_1570) + 
/*GENERATED*/                    (i1202_THIS.y * i1675_w) + 
/*GENERATED*/                    (i1202_THIS.z * newVar_1569) + 
/*GENERATED*/                    -(i1202_THIS.x * newVar_1571)), (
/*GENERATED*/                    (i1202_THIS.w * newVar_1571) + 
/*GENERATED*/                    (i1202_THIS.z * i1675_w) + 
/*GENERATED*/                    (i1202_THIS.x * newVar_1570) + 
/*GENERATED*/                    -(i1202_THIS.y * newVar_1569)), (
/*GENERATED*/                    (i1202_THIS.w * i1675_w) + 
/*GENERATED*/                    -(i1202_THIS.x * newVar_1569) + 
/*GENERATED*/                    -(i1202_THIS.y * newVar_1570) + 
/*GENERATED*/                    -(i1202_THIS.z * newVar_1571)));
/*GENERATED*/                Quaternion newVar_901 = Quaternion.Lerp(this.slowLocalHipRot, newVar_902, 0.1f);
/*GENERATED*/                this.slowLocalHipRot = newVar_901;
/*GENERATED*/                Quaternion i1212_THIS = this.realBodyRot;
/*GENERATED*/                Vector3 i1213_vector = this.hipPosRel;
/*GENERATED*/                float i1216_num1 = (i1212_THIS.x * 2);
/*GENERATED*/                float i1217_num2 = (i1212_THIS.y * 2);
/*GENERATED*/                float i1218_num3 = (i1212_THIS.z * 2);
/*GENERATED*/                float i1219_num4 = (i1212_THIS.x * i1216_num1);
/*GENERATED*/                float i1220_num5 = (i1212_THIS.y * i1217_num2);
/*GENERATED*/                float i1221_num6 = (i1212_THIS.z * i1218_num3);
/*GENERATED*/                float i1222_num7 = (i1212_THIS.x * i1217_num2);
/*GENERATED*/                float i1223_num8 = (i1212_THIS.x * i1218_num3);
/*GENERATED*/                float i1224_num9 = (i1212_THIS.y * i1218_num3);
/*GENERATED*/                float i1225_num10 = (i1212_THIS.w * i1216_num1);
/*GENERATED*/                float i1226_num11 = (i1212_THIS.w * i1217_num2);
/*GENERATED*/                float i1227_num12 = (i1212_THIS.w * i1218_num3);
/*GENERATED*/                Vector3 i1230_b = this.realBodyPos;
/*GENERATED*/                this.hipPosAbs.x = (
/*GENERATED*/                    ((1 + -(i1220_num5 + i1221_num6)) * i1213_vector.x) + 
/*GENERATED*/                    ((i1222_num7 + -i1227_num12) * i1213_vector.y) + 
/*GENERATED*/                    ((i1223_num8 + i1226_num11) * i1213_vector.z) + 
/*GENERATED*/                    i1230_b.x);
/*GENERATED*/                this.hipPosAbs.y = (
/*GENERATED*/                    ((i1222_num7 + i1227_num12) * i1213_vector.x) + 
/*GENERATED*/                    ((1 + -(i1219_num4 + i1221_num6)) * i1213_vector.y) + 
/*GENERATED*/                    ((i1224_num9 + -i1225_num10) * i1213_vector.z) + 
/*GENERATED*/                    i1230_b.y);
/*GENERATED*/                this.hipPosAbs.z = (
/*GENERATED*/                    ((i1223_num8 + -i1226_num11) * i1213_vector.x) + 
/*GENERATED*/                    ((i1224_num9 + i1225_num10) * i1213_vector.y) + 
/*GENERATED*/                    ((1 + -(i1219_num4 + i1220_num5)) * i1213_vector.z) + 
/*GENERATED*/                    i1230_b.z);
/*GENERATED*/                bool _928 = (this.hipFlexibility == 0);
/*GENERATED*/                if (_928)  {
/*GENERATED*/                    this.hipRotAbs = this.realBodyRot;
/*GENERATED*/                } else  {
/*GENERATED*/                    Quaternion i1231_lhs = this.slowLocalHipRot;
/*GENERATED*/                    Quaternion i1232_rhs = this.realBodyRot;
/*GENERATED*/                    Quaternion newVar_906 = new Quaternion((
/*GENERATED*/                        (i1231_lhs.w * i1232_rhs.x) + 
/*GENERATED*/                        (i1231_lhs.x * i1232_rhs.w) + 
/*GENERATED*/                        (i1231_lhs.y * i1232_rhs.z) + 
/*GENERATED*/                        -(i1231_lhs.z * i1232_rhs.y)), (
/*GENERATED*/                        (i1231_lhs.w * i1232_rhs.y) + 
/*GENERATED*/                        (i1231_lhs.y * i1232_rhs.w) + 
/*GENERATED*/                        (i1231_lhs.z * i1232_rhs.x) + 
/*GENERATED*/                        -(i1231_lhs.x * i1232_rhs.z)), (
/*GENERATED*/                        (i1231_lhs.w * i1232_rhs.z) + 
/*GENERATED*/                        (i1231_lhs.z * i1232_rhs.w) + 
/*GENERATED*/                        (i1231_lhs.x * i1232_rhs.y) + 
/*GENERATED*/                        -(i1231_lhs.y * i1232_rhs.x)), (
/*GENERATED*/                        (i1231_lhs.w * i1232_rhs.w) + 
/*GENERATED*/                        -(i1231_lhs.x * i1232_rhs.x) + 
/*GENERATED*/                        -(i1231_lhs.y * i1232_rhs.y) + 
/*GENERATED*/                        -(i1231_lhs.z * i1232_rhs.z)));
/*GENERATED*/                    Quaternion newVar_905 = Quaternion.Lerp(this.realBodyRot, newVar_906, this.hipFlexibility);
/*GENERATED*/                    this.hipRotAbs = newVar_905;
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/        }
        public static Vector3 torqueFromQuaternions(MotorBean motor, Quaternion currentRot, Quaternion wantedRot, Vector3 currentAngularSpeed, float maxAccelMultiplier) {
            Quaternion rotSub = ExtensionMethods.mul(wantedRot, ExtensionMethods.conjug(currentRot));
            if ((rotSub.w < 0))  {
                rotSub = ExtensionMethods.scale(rotSub, -1);
            }
            Vector3 wantedAngularVelocity = ExtensionMethods.imaginary(rotSub).mul(motor.distanceToSpeed).limit(motor.maxSpeed).sub(currentAngularSpeed).mul(motor.speedDifToAccel).limit((motor.maxAccel * maxAccelMultiplier));
            return wantedAngularVelocity;
        }

        Quaternion oldHipLenHelp = Quaternion.identity;

/*GENERATED*/        [Optimize]
/*GENERATED*/        void calcHipLenHelp() {
/*GENERATED*/            bool _2095 = (this.steps.Count < 2);
/*GENERATED*/            if (_2095)  {
/*GENERATED*/                return ;
/*GENERATED*/            }
/*GENERATED*/            Step2 left = this.steps[this.leadingLegLeft];
/*GENERATED*/            Step2 right = this.steps[this.leadingLegRight];
/*GENERATED*/            Vector3 leg1 = this.surfaceDetector.detect(left.posAbs, Vector3.up);
/*GENERATED*/            Vector3 leg2 = this.surfaceDetector.detect(right.posAbs, Vector3.up);
/*GENERATED*/            Vector3 newVar_2077 = this.surfaceDetector.detect(left.basisAbs, Vector3.up);
/*GENERATED*/            Vector3 i2102_b = left.posAbs;
/*GENERATED*/            float i2105_vector3_x = 0;
/*GENERATED*/            float i2105_vector3_y = 0;
/*GENERATED*/            float i2105_vector3_z = 0;
/*GENERATED*/            i2105_vector3_x = (newVar_2077.x + -i2102_b.x);
/*GENERATED*/            i2105_vector3_y = (newVar_2077.y + -i2102_b.y);
/*GENERATED*/            i2105_vector3_z = (newVar_2077.z + -i2102_b.z);
/*GENERATED*/            float newVar_2076 = Mathf.Sqrt((float)((
/*GENERATED*/                (i2105_vector3_x * i2105_vector3_x) + 
/*GENERATED*/                (i2105_vector3_y * i2105_vector3_y) + 
/*GENERATED*/                (i2105_vector3_z * i2105_vector3_z))));
/*GENERATED*/            float d1 = (newVar_2076 + (0.1f * this.midLen));
/*GENERATED*/            Vector3 newVar_2080 = this.surfaceDetector.detect(right.basisAbs, Vector3.up);
/*GENERATED*/            Vector3 i2108_b = right.posAbs;
/*GENERATED*/            float i2111_vector3_x = 0;
/*GENERATED*/            float i2111_vector3_y = 0;
/*GENERATED*/            float i2111_vector3_z = 0;
/*GENERATED*/            i2111_vector3_x = (newVar_2080.x + -i2108_b.x);
/*GENERATED*/            i2111_vector3_y = (newVar_2080.y + -i2108_b.y);
/*GENERATED*/            i2111_vector3_z = (newVar_2080.z + -i2108_b.z);
/*GENERATED*/            float newVar_2079 = Mathf.Sqrt((float)((
/*GENERATED*/                (i2111_vector3_x * i2111_vector3_x) + 
/*GENERATED*/                (i2111_vector3_y * i2111_vector3_y) + 
/*GENERATED*/                (i2111_vector3_z * i2111_vector3_z))));
/*GENERATED*/            float d2 = (newVar_2079 + (0.1f * this.midLen));
/*GENERATED*/            bool _2096 = !left.dockedState;
/*GENERATED*/            if (_2096)  {
/*GENERATED*/                d1 = (d1 + -this.midLen);
/*GENERATED*/            }
/*GENERATED*/            bool _2097 = !right.dockedState;
/*GENERATED*/            if (_2097)  {
/*GENERATED*/                d2 = (d2 + -this.midLen);
/*GENERATED*/            }
/*GENERATED*/            float forPare_2237 = (d1 + d2);
/*GENERATED*/            float newVar_2084 = (1 + -(d1 / forPare_2237));
/*GENERATED*/            Vector3 i2112_a = this.up;
/*GENERATED*/            float newVar_2083_x = 0;
/*GENERATED*/            float newVar_2083_y = 0;
/*GENERATED*/            float newVar_2083_z = 0;
/*GENERATED*/            newVar_2083_x = (i2112_a.x * newVar_2084);
/*GENERATED*/            newVar_2083_y = (i2112_a.y * newVar_2084);
/*GENERATED*/            newVar_2083_z = (i2112_a.z * newVar_2084);
/*GENERATED*/            float newVar_2082_x = 0;
/*GENERATED*/            float newVar_2082_y = 0;
/*GENERATED*/            float newVar_2082_z = 0;
/*GENERATED*/            newVar_2082_x = newVar_2083_x;
/*GENERATED*/            newVar_2082_y = newVar_2083_y;
/*GENERATED*/            newVar_2082_z = newVar_2083_z;
/*GENERATED*/            float dbgPoint1_x = 0;
/*GENERATED*/            float dbgPoint1_y = 0;
/*GENERATED*/            float dbgPoint1_z = 0;
/*GENERATED*/            dbgPoint1_x = (leg1.x + newVar_2082_x);
/*GENERATED*/            dbgPoint1_y = (leg1.y + newVar_2082_y);
/*GENERATED*/            dbgPoint1_z = (leg1.z + newVar_2082_z);
/*GENERATED*/            float newVar_2089 = (1 + -(d2 / forPare_2237));
/*GENERATED*/            Vector3 i2118_a = this.up;
/*GENERATED*/            float newVar_2088_x = 0;
/*GENERATED*/            float newVar_2088_y = 0;
/*GENERATED*/            float newVar_2088_z = 0;
/*GENERATED*/            newVar_2088_x = (i2118_a.x * newVar_2089);
/*GENERATED*/            newVar_2088_y = (i2118_a.y * newVar_2089);
/*GENERATED*/            newVar_2088_z = (i2118_a.z * newVar_2089);
/*GENERATED*/            float newVar_2087_x = 0;
/*GENERATED*/            float newVar_2087_y = 0;
/*GENERATED*/            float newVar_2087_z = 0;
/*GENERATED*/            newVar_2087_x = newVar_2088_x;
/*GENERATED*/            newVar_2087_y = newVar_2088_y;
/*GENERATED*/            newVar_2087_z = newVar_2088_z;
/*GENERATED*/            float dbgPoint2_x = 0;
/*GENERATED*/            float dbgPoint2_y = 0;
/*GENERATED*/            float dbgPoint2_z = 0;
/*GENERATED*/            dbgPoint2_x = (leg2.x + newVar_2087_x);
/*GENERATED*/            dbgPoint2_y = (leg2.y + newVar_2087_y);
/*GENERATED*/            dbgPoint2_z = (leg2.z + newVar_2087_z);
/*GENERATED*/            Vector3 Z = new Vector3((dbgPoint1_x + -dbgPoint2_x), (dbgPoint1_y + -dbgPoint2_y), (dbgPoint1_z + -dbgPoint2_z));
/*GENERATED*/            Vector3 i2127_Y = this.up;
/*GENERATED*/            Vector3 newVar_2183 = Vector3.Normalize(Z);
/*GENERATED*/            Vector3 i2130_a = new Vector3(
/*GENERATED*/                (float)(((i2127_Y.y * newVar_2183.z) + -(i2127_Y.z * newVar_2183.y))), 
/*GENERATED*/                (float)(((i2127_Y.z * newVar_2183.x) + -(i2127_Y.x * newVar_2183.z))), 
/*GENERATED*/                (float)(((i2127_Y.x * newVar_2183.y) + -(i2127_Y.y * newVar_2183.x))));
/*GENERATED*/            Vector3 i2128_X = Vector3.Normalize(i2130_a);
/*GENERATED*/            Vector3 i2135_a = new Vector3(
/*GENERATED*/                (float)(((newVar_2183.y * i2128_X.z) + -(newVar_2183.z * i2128_X.y))), 
/*GENERATED*/                (float)(((newVar_2183.z * i2128_X.x) + -(newVar_2183.x * i2128_X.z))), 
/*GENERATED*/                (float)(((newVar_2183.x * i2128_X.y) + -(newVar_2183.y * i2128_X.x))));
/*GENERATED*/            Vector3 newVar_2202 = Vector3.Normalize(i2135_a);
/*GENERATED*/            Quaternion newVar_2093 = MUtil.qToAxes(
/*GENERATED*/                i2128_X.x, 
/*GENERATED*/                i2128_X.y, 
/*GENERATED*/                i2128_X.z, 
/*GENERATED*/                newVar_2202.x, 
/*GENERATED*/                newVar_2202.y, 
/*GENERATED*/                newVar_2202.z, 
/*GENERATED*/                newVar_2183.x, 
/*GENERATED*/                newVar_2183.y, 
/*GENERATED*/                newVar_2183.z);
/*GENERATED*/            Quaternion newVar_2092 = Quaternion.Lerp(this.oldHipLenHelp, newVar_2093, 0.2f);
/*GENERATED*/            this.oldHipLenHelp = newVar_2092;
/*GENERATED*/            Quaternion newVar_2094 = Quaternion.Lerp(this.wantedHipRot, this.oldHipLenHelp, 0.8f);
/*GENERATED*/            this.wantedHipRot = newVar_2094;
/*GENERATED*/        }
        public static Vector3 rotDisp(Quaternion rot, Vector3 vec) {
            return (ExtensionMethods.rotate(rot, vec) - vec);
        }

        [HideInInspector] public float emphasis;
        public Vector3 realWantedSpeed;

        public virtual void tickSteps(float dt) {
            this.steps[this.leadingLegRight].additionalDisplacement = new Vector3(-this.emphasis, 0, 0);
            for (int stepIndex = 0; (stepIndex < this.steps.Count); (stepIndex)++)  {
                Step2 step = this.steps[stepIndex];
                float hDif = MyMath.clamp(((step.bestTargetProgressiveAbs.y - (step.posAbs.y + step.lastDockedAtLocal.y)) / step.maxLen), -1, 1);
                step.undockHDif = (1 + Math.Max(-0.8f, (((hDif > 0)) ? ((hDif * 5)) : (hDif))));
                 {
                    float value = (-1 + Math.Min(1, step.landTime));
                    step.fromAbove += value;
                    if (this.collectSteppingHistory)  {
                        step.paramHistory.setValue(HistoryInfoBean.lt, value);
                    }
                }
                float restTime = 0.5f;
                if ((step.dockedState && (step.landTime < restTime)))  {
                    float value = ((-1 + (step.landTime / restTime)) * 5);
                    step.fromAbove += value;
                    if (this.collectSteppingHistory)  {
                        step.paramHistory.setValue(HistoryInfoBean.lt2, value);
                    }
                }
                if (!step.dockedState)  {
                    for (int index = 0; (index < step.affectedByProgress.Count); (index)++)  {
                        StepNeuro<Step2> affected = step.affectedByProgress[index];
                        float value = (affected.add + (affected.mul * step.progress));
                        affected.leg.fromAbove += value;
                        if (this.collectSteppingHistory)  {
                            affected.leg.paramHistory.setValue(affected.desc, value);
                        }
                    }
                } else  {
                     {
                        for (int index = 0; (index < step.affectedByDeviation.Count); (index)++)  {
                            StepNeuro<Step2> affected = step.affectedByDeviation[index];
                            float value = (affected.add + (affected.mul * step.deviation));
                            affected.leg.fromAbove += value;
                            if (this.collectSteppingHistory)  {
                                affected.leg.paramHistory.setValue(affected.desc, value);
                            }
                        }
                    }
                }
            }
            Step2 right = this.steps[this.leadingLegRight];
            Step2 left = this.steps[this.leadingLegLeft];
            left.forbidHalf = this.bipedalForbidPlacement;
            right.forbidHalf = this.bipedalForbidPlacement;
            if (this.bipedalForbidPlacement)  {
                left.forbidHalfPos = right.posAbs;
                right.forbidHalfPos = left.posAbs;
                left.forbidHalfDir = ExtensionMethods.normalized(ExtensionMethods.withSetY(ExtensionMethods.sub(left.basisAbs, right.basisAbs), 0));
                right.forbidHalfDir = -left.forbidHalfDir;
            }
            if (((this.forceBipedalEarlyStep && right.dockedState) && left.dockedState))  {
                Vector3 zeroSpeed = ExtensionMethods.withSetY(this.imCenterSpeed, 0);
                if ((ExtensionMethods.length(zeroSpeed) > (0.2f * this.midLen)))  {
                    float sRight = ExtensionMethods.scalarProduct(ExtensionMethods.sub(right.posAbs, this.realBodyPos), zeroSpeed);
                    float sLeft = ExtensionMethods.scalarProduct(ExtensionMethods.sub(left.posAbs, this.realBodyPos), zeroSpeed);
                    if (((sRight < 0) && (sLeft < 0)))  {
                        left.fromAbove -= (sLeft / this.midLen);
                        if (this.collectSteppingHistory)  {
                            left.paramHistory.setValue(HistoryInfoBean.bipedEarlyStep, (-sLeft / this.midLen));
                        }
                        right.fromAbove -= (sRight / this.midLen);
                        if (this.collectSteppingHistory)  {
                            right.paramHistory.setValue(HistoryInfoBean.bipedEarlyStep, (-sRight / this.midLen));
                        }
                    }
                }
            }
            if (this.collectSteppingHistory)  {
                foreach (Step2 step in this.steps) {
                    step.paramHistory.setValue(HistoryInfoBean.fromAbove, step.fromAbove);
                }
            }
            float biggestFromAbove = 0;
            Step2 biggestStep = null;
            for (int i = 0; (i < this.steps.Count); (i)++)  {
                MoveenSkelBase skel = this.legSkel[i];
                Step2 step = this.steps[i];
                if ((step.dockedState && (step.fromAbove > biggestFromAbove)))  {
                    biggestFromAbove = step.fromAbove;
                    biggestStep = step;
                }
            }
            if ((biggestStep != null))  {
                biggestStep.beginStep(1);
            }
            if (((this.steps[0].stepStarted || this.steps[0].wasTooLong) && !this.steps[1].dockedState))  {
            }
            if (((this.steps[1].stepStarted || this.steps[1].wasTooLong) && !this.steps[0].dockedState))  {
            }
            this.steps[0].stepStarted = false;
            this.steps[1].stepStarted = false;
            for (int i = 0; (i < this.steps.Count); (i)++)  {
                MoveenSkelBase skel = this.legSkel[i];
                Step2 step = this.steps[i];
                step.tick(dt);
                if ((skel != null))  {
                    skel.setTarget(step.posAbs, step.footOrientation);
                    skel.tick(Time.deltaTime);
                    step.comfortFromSkel = skel.comfort;
                    step.posAbs = skel.limitedResultTarget;
                } else  {
                     {
                    }
                }
            }
        }
        void calcAbs(float dt) {
            float bodySpeedForLeg = Math.Min(
                this.horizontalMotor.maxSpeed, 
                Math.Max(ExtensionMethods.length(this.realSpeed), ExtensionMethods.length((this.realBodyPos - this.inputWantedPos))));
            for (int i = 0; (i < this.steps.Count); (i)++)  {
                Step2 step = this.steps[i];
                step.g = this.g;
                MoveenSkelBase skel = this.legSkel[i];
                step.basisAbs = step.thisTransform.position;
                step.projectedRot = this.projectedRot;
                step.legSpeed = MyMath.max(
                    0, 
                    step.stepSpeedMin, 
                    (bodySpeedForLeg * step.stepSpeedBodySpeedMul), 
                    (ExtensionMethods.length(this.realBodyAngularSpeed) * step.stepSpeedBodyRotSpeedMul));
                step.bodyPos = this.imCenter;
                step.bodyRot = this.realBodyRot;
                step.bodySpeed = this.imActualCenterSpeed;
                step.bodySpeedLength = ExtensionMethods.length(this.imActualCenterSpeed);
                step.bodySpeedMax = this.horizontalMotor.maxSpeed;
                step.calcAbs(dt, this.virtualForLegs, this.inputWantedRot);
                step.fromAbove = -1;
            }
        }
        public virtual void reset(Vector3 pos, Quaternion rot) {
            this.inputWantedPos = pos;
            this.inputWantedRot = rot;
            this.realBodyPos = pos;
            this.realBodyRot = rot;
            this.hipPosAbs = (ExtensionMethods.rotate(this.realBodyRot, this.hipPosRel) + this.realBodyPos);
            this.hipRotAbs = this.realBodyRot;
            this.calculatedCOG = (ExtensionMethods.rotate(this.realBodyRot, new Vector3(0, this.cogUpDown, 0)) + this.realBodyPos);
            this.imCenter = this.realBodyPos;
            this.imBody = this.realBodyPos;
            for (int index = 0; (index < this.steps.Count); (index)++)  {
                this.steps[index].reset(pos, rot);
            }
        }
        public virtual Vector3 project(Vector3 input) {
            return this.surfaceDetector.detect(input, Vector3.up);
        }

    }
}
