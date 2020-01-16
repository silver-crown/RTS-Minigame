namespace moveen.core {
    using System;
    using System.Collections.Generic;
    using moveen.descs;
    using moveen.utils;
    using UnityEngine;
    using UnityEngine.Serialization;

    [Serializable] public class Step2 {

        [NonSerialized] public Transform thisTransform;
        MoveenStepper5 fld;
        static int nextId;
        [NonSerialized][InstrumentalInfo] public int internalId = (Step2.nextId)++;
        [Tooltip("Put the foot closer on steep slopes")] public bool handleSteepSlopes = true;
        [Tooltip("Limit max step height (may lead to slippage)")] public bool limitMaxStepHeight;
        [Tooltip("Absolute max step height")] public float maxStepHeight = 0.5f;
        [Tooltip("Off: comfortPosRel will be same as the leg tip.\nOn: comfortPosRel can be defined separately from the leg tip.")] public bool detachedComfortPosRel;
        [Tooltip("Best (comfort) leg position")][FormerlySerializedAs("bestTargetRel")] public Vector3 comfortPosRel = new Vector3(0, -1, 0);
        [HideInInspector] public ISurfaceDetector surfaceDetector;
        [HideInInspector] public List<StepNeuro<Step2>> affectedByProgress = MUtil2.al<StepNeuro<Step2>>();
        [HideInInspector] public List<StepNeuro<Step2>> affectedByDeviation = MUtil2.al<StepNeuro<Step2>>();
        [HideInInspector] public List<StepNeuro<Step2>> affectedByDir = MUtil2.al<StepNeuro<Step2>>();
        [Header("Step dynamics (consider increasing of speeds if stepping is odd)")][Tooltip("Minimal leg stepping speed")] public float stepSpeedMin = 4;
        [Tooltip("Leg stepping speed X body speed dependency")] public float stepSpeedBodySpeedMul = 1.2f;
        [Tooltip("Leg stepping speed X body rotation speed dependency")] public float stepSpeedBodyRotSpeedMul = 4;
        public float maxAcceleration = 5;
        [Tooltip("Pause between leg decided to step, and it actually ups (0 for very light walk, 0.2 and more to add more weight)")] public float undockPause;
        [HideInInspector] public float airTime;
        [HideInInspector][InstrumentalInfo] public bool wasTooLong;
        [HideInInspector][InstrumentalInfo] public bool dockedState;
        [HideInInspector] public Vector3 bodyPos;
        [HideInInspector] public Quaternion bodyRot;
        [HideInInspector] public Quaternion projectedRot;
        [HideInInspector] public float bodySpeedLength;
        [HideInInspector] public Vector3 bodySpeed;
        [HideInInspector] public float bodySpeedMax;
        [HideInInspector] public Vector3 additionalDisplacement;
        [HideInInspector] public Vector3 terrainNormal = new Vector3(0, 1, 0);
        [HideInInspector] public float pursueBodySpeed = 1;
        [NonSerialized] public Vector3 bodyForward = new Vector3(1, 0, 0);
        [NonSerialized] public Vector3 bodyUp = new Vector3(0, 1, 0);
        [Header("Step geometry")][Tooltip("Show possible step trajectory")] public bool showTrajectory;
        [Tooltip("Step radius multiplier. Some leg layouts may require modification of default value")] public float comfortRadiusRatio = 1;
        [HideInInspector][InstrumentalInfo] public float comfortRadius;
        [Tooltip("Speed, with which foot is orienting in body's direction while in air")] public int footOrientationSpeed = 5;
        Quaternion possibleFootOrientation;
        [HideInInspector] public Quaternion footOrientation = Quaternion.identity;
        [FloatWarning(min=0)][Tooltip("Parameter for step trajectory, turn on 'showTrajectory' and look at changes")] public float targetDockR = 0.3f;
        [HideInInspector][InstrumentalInfo] public Vector3 targetTo;
        [FloatWarning(min=0)][Tooltip("Parameter for step trajectory, turn on 'showTrajectory' and look at changes")] public float undockInitialStrength = 3;
        [FloatWarning(min=0)][Tooltip("Parameter for step trajectory, turn on 'showTrajectory' and look at changes")] public float undockTime = 0.3f;
        [HideInInspector] public float undockHDif;
        public bool emergencyDown;
        [NonSerialized] public bool forbidHalf;
        [NonSerialized] public Vector3 forbidHalfPos;
        [NonSerialized] public Vector3 forbidHalfDir;
        [NonSerialized] public bool isExample;
        [HideInInspector][InstrumentalInfo] public bool stepStarted;
        [HideInInspector][InstrumentalInfo] public bool switchToBottom;
        [HideInInspector][InstrumentalInfo] public bool wasUnderground;
        [HideInInspector][InstrumentalInfo] public Vector3 curTarget;
        [HideInInspector][InstrumentalInfo] public float speedSlow;
        [HideInInspector][InstrumentalInfo] public Vector3 calcTarget;
        [HideInInspector][InstrumentalInfo] public float legAlt;
        [HideInInspector][InstrumentalInfo] public float lastStepLen;
        [HideInInspector][InstrumentalInfo] public Vector3 lastDockedAtLocal;
        [HideInInspector][InstrumentalInfo] public Vector3 lastUndockedAtLocal;
        [HideInInspector][InstrumentalInfo] public Vector3 basisPosRel;
        [HideInInspector][InstrumentalInfo] public Quaternion basisRotRel;
        [HideInInspector][InstrumentalInfo] public Vector3 basisAbs;
        [HideInInspector][InstrumentalInfo] public Vector3 acceleration;
        [HideInInspector][InstrumentalInfo] public float comfortFromSkel;
        [HideInInspector][InstrumentalInfo] public Vector3 bestTargetConservativeAbs = new Vector3(0, 0, 0);
        [HideInInspector][InstrumentalInfo] public Vector3 bestTargetConservativeUnprojAbs = new Vector3(0, 0, 0);
        [HideInInspector][InstrumentalInfo] public Vector3 bestTargetConservativeUnprojAbs2 = new Vector3(0, 0, 0);
        [HideInInspector][InstrumentalInfo] public Vector3 bestTargetProgressiveAbs = new Vector3(0, 0, 0);
        [HideInInspector][InstrumentalInfo] public Vector3 posAbs;
        [HideInInspector][InstrumentalInfo] public Vector3 wantedSpeed;
        [HideInInspector][InstrumentalInfo] public Vector3 speedAbs;
        [HideInInspector][InstrumentalInfo] public float undockPauseCur;
        [HideInInspector][InstrumentalInfo] public float undockUpLength;
        [HideInInspector][InstrumentalInfo] public Vector3 undockVec;
        [HideInInspector][InstrumentalInfo] public float undockProgress;
        [HideInInspector][InstrumentalInfo] public float undockCurTime;
        [HideInInspector][InstrumentalInfo] public float maxLen;
        [HideInInspector][InstrumentalInfo] public float fromAbove;
        [HideInInspector][InstrumentalInfo] public float landTime;
        [HideInInspector][InstrumentalInfo] public float deviation;
        [HideInInspector][InstrumentalInfo] public float progress;
        [HideInInspector][InstrumentalInfo] public float timedProgress;
        [HideInInspector][InstrumentalInfo] public float beFaster;
        [HideInInspector][InstrumentalInfo] public float legSpeed = 4;
        [HideInInspector][InstrumentalInfo] public Vector3 g = new Vector3(0, -9.81f, 0);
        [NonSerialized] public bool collectSteppingHistory;
        public CounterStacksCollection paramHistory = new CounterStacksCollection(20);

        public virtual void calcAbs(float dt, Vector3 futurePos, Quaternion futureRot) {
            this.bodyUp = this.bodyRot.rotate(Vector3.up);
            this.bodyForward = this.bodyRot.rotate(Vector3.right);
            Vector3 chosenTargetRel = ExtensionMethods.add(this.comfortPosRel, this.additionalDisplacement);
            this.bestTargetConservativeUnprojAbs2 = Step2.toAbs(chosenTargetRel, this.bodyPos, this.projectedRot);
            this.bestTargetConservativeUnprojAbs = ExtensionMethods.withSetY(this.bestTargetConservativeUnprojAbs2, this.bodyPos.y);
            this.bestTargetConservativeAbs = this.surfaceDetector.detect(this.bestTargetConservativeUnprojAbs, Vector3.up).limit(this.bestTargetConservativeUnprojAbs, (this.comfortRadius * 4));
            futurePos = (
                futurePos - 
                (
                ExtensionMethods.getXForHorizontalAxis(this.lastUndockedAtLocal, ExtensionMethods.limit(this.bodySpeed, 1)) / 
                this.bodySpeedMax));
            if (this.emergencyDown)  {
                this.bestTargetProgressiveAbs = this.bestTargetConservativeAbs;
            } else  {
                 {
                    this.bestTargetProgressiveAbs = ExtensionMethods.withSetY(Step2.toAbs(chosenTargetRel, futurePos, this.projectedRot), futurePos.y);
                    this.bestTargetProgressiveAbs = this.surfaceDetector.detect(this.bestTargetProgressiveAbs, Vector3.up);
                }
            }
            if (this.handleSteepSlopes)  {
                Vector3 downAbsPos = this.surfaceDetector.detect((this.basisAbs + (futurePos - this.bodyPos)), this.bodyUp);
                float fall = (this.bestTargetProgressiveAbs.y - downAbsPos.y);
                float mapped = MyMath.regionMapClamp(Math.Abs(fall), 0, (this.maxLen * 0.5f), 0, 1);
                this.bestTargetProgressiveAbs = ExtensionMethods.mix(this.bestTargetProgressiveAbs, downAbsPos, mapped);
            }
            this.bestTargetProgressiveAbs = ExtensionMethods.limit(this.bestTargetProgressiveAbs, this.bestTargetConservativeAbs, this.comfortRadius);
            if (this.forbidHalf)  {
                float x = ExtensionMethods.scalarProduct(ExtensionMethods.sub(this.bestTargetProgressiveAbs, this.forbidHalfPos), this.forbidHalfDir);
                if ((x < 0))  {
                    this.bestTargetProgressiveAbs = (this.bestTargetProgressiveAbs - ((x * 1.1f) * this.forbidHalfDir));
                }
            }
            if (this.limitMaxStepHeight)  {
                float cur = ExtensionMethods.sub(this.bestTargetProgressiveAbs, this.basisAbs).y;
                if ((cur > this.maxStepHeight))  {
                    this.bestTargetProgressiveAbs = ExtensionMethods.withSetY(this.bestTargetProgressiveAbs, -100500);
                }
            }
            this.comfortRadius = Step2.getComfortRadius(this.posAbs, this.basisAbs, this.maxLen);
            this.comfortRadius = Math.Min(((this.maxLen * 0.5f) * this.comfortRadiusRatio), this.comfortRadius);
            this.comfortRadius = Math.Max(1, this.comfortRadius);
            this.comfortRadius = ((this.maxLen * 0.5f) * this.comfortRadiusRatio);
            this.deviationUnclamped = (
                MyMath.min(
                ExtensionMethods.dist(this.bestTargetConservativeAbs, this.posAbs), 
                ExtensionMethods.dist(this.bestTargetProgressiveAbs, this.posAbs)) / 
                this.comfortRadius);
            this.deviation = MyMath.clamp(this.deviationUnclamped, 0, 10);
        }

        [HideInInspector] public Vector3 oldPosAbs;
        [HideInInspector][InstrumentalInfo] public float airParam;
        [HideInInspector][InstrumentalInfo] public bool canGoAir;
        [HideInInspector][InstrumentalInfo] public Vector3 airTarget;
        float deviationUnclamped;
        [NonSerialized] P<Vector3> tmp = new P<Vector3>(new Vector3());

/*GENERATED*/        [Optimize]
/*GENERATED*/        public virtual void tick(float dt) {
/*GENERATED*/            Vector3 i31_a = this.posAbs;
/*GENERATED*/            Vector3 i32_b = this.oldPosAbs;
/*GENERATED*/            this.speedAbs.x = ((i31_a.x + -i32_b.x) / dt);
/*GENERATED*/            this.speedAbs.y = ((i31_a.y + -i32_b.y) / dt);
/*GENERATED*/            this.speedAbs.z = ((i31_a.z + -i32_b.z) / dt);
/*GENERATED*/            this.oldPosAbs = this.posAbs;
/*GENERATED*/            bool _443 = (this.comfortFromSkel > 0.98f);
/*GENERATED*/            if (_443)  {
/*GENERATED*/                bool _449 = (this.dockedState && (this.landTime > 0.2f));
/*GENERATED*/                if (_449)  {
/*GENERATED*/                    bool _450 = this.collectSteppingHistory;
/*GENERATED*/                    if (_450)  {
/*GENERATED*/                        this.paramHistory.setValue(HistoryInfoBean.beginStep, 1);
/*GENERATED*/                    }
/*GENERATED*/                    this.stepStarted = true;
/*GENERATED*/                    bool _451 = this.dockedState;
/*GENERATED*/                    if (_451)  {
/*GENERATED*/                        this.undockPauseCur = (this.undockPause * 0);
/*GENERATED*/                        float i80_progress = this.deviation;
/*GENERATED*/                        this.undockPauseCur = ((this.undockPauseCur * (1 + -i80_progress)) + (this.undockPause * i80_progress));
/*GENERATED*/                    }
/*GENERATED*/                    bool _452 = (1 > 0);
/*GENERATED*/                    if (_452)  {
/*GENERATED*/                        this.switchToBottom = false;
/*GENERATED*/                    }
/*GENERATED*/                    this.dockedState = false;
/*GENERATED*/                    this.undockProgress = 0;
/*GENERATED*/                    this.undockCurTime = (this.undockProgress * this.undockTime);
/*GENERATED*/                    this.undockUpLength = (this.undockInitialStrength * this.undockHDif * (1 + -this.undockProgress));
/*GENERATED*/                    this.targetTo = this.bestTargetProgressiveAbs;
/*GENERATED*/                    this.emergencyDown = false;
/*GENERATED*/                    Vector3 i69_a = this.posAbs;
/*GENERATED*/                    Vector3 i70_b = this.bestTargetProgressiveAbs;
/*GENERATED*/                    this.lastUndockedAtLocal.x = (i69_a.x + -i70_b.x);
/*GENERATED*/                    this.lastUndockedAtLocal.y = (i69_a.y + -i70_b.y);
/*GENERATED*/                    this.lastUndockedAtLocal.z = (i69_a.z + -i70_b.z);
/*GENERATED*/                    Vector3 i73_a = this.lastDockedAtLocal;
/*GENERATED*/                    Vector3 i74_b = this.lastUndockedAtLocal;
/*GENERATED*/                    float newVar_259 = (i73_a.x + -i74_b.x);
/*GENERATED*/                    float newVar_260 = (i73_a.y + -i74_b.y);
/*GENERATED*/                    float newVar_261 = (i73_a.z + -i74_b.z);
/*GENERATED*/                    this.lastStepLen = Mathf.Sqrt((float)(((newVar_259 * newVar_259) + (newVar_260 * newVar_260) + (newVar_261 * newVar_261))));
/*GENERATED*/                }
/*GENERATED*/                this.wasTooLong = true;
/*GENERATED*/            } else  {
/*GENERATED*/                 {
/*GENERATED*/                    this.wasTooLong = false;
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/            this.wantedSpeed.x = 0;
/*GENERATED*/            this.wantedSpeed.y = 0;
/*GENERATED*/            this.wantedSpeed.z = 0;
/*GENERATED*/            this.airParam = 0;
/*GENERATED*/            bool _21 = !this.dockedState;
/*GENERATED*/            if (_21)  {
/*GENERATED*/                this.undockPauseCur = (this.undockPauseCur + dt);
/*GENERATED*/                this.landTime = 0;
/*GENERATED*/                bool _25 = (this.undockPauseCur >= this.undockPause);
/*GENERATED*/                if (_25)  {
/*GENERATED*/                    this.airTime = (this.airTime + dt);
/*GENERATED*/                    this.targetTo = this.bestTargetProgressiveAbs;
/*GENERATED*/                    this.undockCurTime = (this.undockCurTime + dt);
/*GENERATED*/                    this.undockProgress = (this.undockCurTime / this.undockTime);
/*GENERATED*/                    bool _454 = (this.undockProgress >= 1);
/*GENERATED*/                    if (_454)  {
/*GENERATED*/                        this.undockProgress = 1;
/*GENERATED*/                        this.undockUpLength = 0;
/*GENERATED*/                        this.undockVec.x = 0;
/*GENERATED*/                        this.undockVec.y = 0;
/*GENERATED*/                        this.undockVec.z = 0;
/*GENERATED*/                    } else  {
/*GENERATED*/                         {
/*GENERATED*/                            this.undockUpLength = ((this.undockInitialStrength / this.legSpeed) * this.undockHDif * (1 + -this.undockProgress));
/*GENERATED*/                            this.undockVec.x = 0;
/*GENERATED*/                            this.undockVec.y = this.undockUpLength;
/*GENERATED*/                            this.undockVec.z = 0;
/*GENERATED*/                        }
/*GENERATED*/                    }
/*GENERATED*/                    Vector3 i564_a = this.undockVec;
/*GENERATED*/                    Vector3 i568_a = this.basisAbs;
/*GENERATED*/                    Vector3 i569_b = this.posAbs;
/*GENERATED*/                    Vector3 i566_a = new Vector3((i568_a.x + -i569_b.x), (i568_a.y + -i569_b.y), (i568_a.z + -i569_b.z));
/*GENERATED*/                    float i567_newLen = ((this.undockInitialStrength / 3) * (1 + -this.undockProgress));
/*GENERATED*/                    Vector3 i570_a = Vector3.Normalize(i566_a);
/*GENERATED*/                    Vector3 i565_b = new Vector3((i570_a.x * i567_newLen), (i570_a.y * i567_newLen), (i570_a.z * i567_newLen));
/*GENERATED*/                    Vector3 newVar_597 = new Vector3((i564_a.x + i565_b.x), (i564_a.y + i565_b.y), (i564_a.z + i565_b.z));
/*GENERATED*/                    this.undockVec = newVar_597;
/*GENERATED*/                    Vector3 i94_from = this.posAbs;
/*GENERATED*/                    Vector3 i103_b = this.targetTo;
/*GENERATED*/                    float newVar_287 = (i94_from.x + -i103_b.x);
/*GENERATED*/                    float newVar_289 = (i94_from.z + -i103_b.z);
/*GENERATED*/                    float i95_fromToDist = Mathf.Sqrt((float)(((newVar_287 * newVar_287) + (newVar_289 * newVar_289))));
/*GENERATED*/                    Vector3 i107_a = this.targetTo;
/*GENERATED*/                    this.curTarget.x = i107_a.x;
/*GENERATED*/                    this.curTarget.y = (i107_a.y + this.targetDockR);
/*GENERATED*/                    this.curTarget.z = i107_a.z;
/*GENERATED*/                    bool _455 = (((i95_fromToDist > this.targetDockR) && !this.switchToBottom) || (this.undockProgress < 0.3f));
/*GENERATED*/                    if (_455)  {
/*GENERATED*/                    } else  {
/*GENERATED*/                         {
/*GENERATED*/                            this.switchToBottom = true;
/*GENERATED*/                            Vector3 i162_a = this.targetTo;
/*GENERATED*/                            this.curTarget.x = i162_a.x;
/*GENERATED*/                            this.curTarget.y = (i162_a.y + -this.targetDockR);
/*GENERATED*/                            this.curTarget.z = i162_a.z;
/*GENERATED*/                        }
/*GENERATED*/                    }
/*GENERATED*/                    Vector3 i111_THIS = this.curTarget;
/*GENERATED*/                    Vector3 i112_center = this.basisAbs;
/*GENERATED*/                    float i113_max = (this.maxLen * 1.5f);
/*GENERATED*/                    Vector3 i116_THIS = new Vector3((i111_THIS.x + -i112_center.x), (i111_THIS.y + -i112_center.y), (i111_THIS.z + -i112_center.z));
/*GENERATED*/                    float i118_l = Mathf.Sqrt((float)(((i116_THIS.x * i116_THIS.x) + (i116_THIS.y * i116_THIS.y) + (i116_THIS.z * i116_THIS.z))));
/*GENERATED*/                    Vector3 i114_a;
/*GENERATED*/                    bool _456 = (i118_l > i113_max);
/*GENERATED*/                    if (_456)  {
/*GENERATED*/                        Vector3 i574_a = Vector3.Normalize(i116_THIS);
/*GENERATED*/                        Vector3 newVar_603 = new Vector3((i574_a.x * i113_max), (i574_a.y * i113_max), (i574_a.z * i113_max));
/*GENERATED*/                        i114_a = newVar_603;
/*GENERATED*/                    } else  {
/*GENERATED*/                        i114_a = i116_THIS;
/*GENERATED*/                    }
/*GENERATED*/                    this.curTarget.x = (i114_a.x + i112_center.x);
/*GENERATED*/                    this.curTarget.y = (i114_a.y + i112_center.y);
/*GENERATED*/                    this.curTarget.z = (i114_a.z + i112_center.z);
/*GENERATED*/                    this.airParam = ((this.canGoAir) ? (MyMath.clamp(((this.bestTargetConservativeUnprojAbs2.y + -this.curTarget.y) / this.maxLen), 0, 1)) : (0));
/*GENERATED*/                    Vector3 i123_a = this.curTarget;
/*GENERATED*/                    Vector3 i124_b = this.bestTargetConservativeUnprojAbs2;
/*GENERATED*/                    float i125_progress = this.airParam;
/*GENERATED*/                    this.airTarget.x = (((i124_b.x + -i123_a.x) * i125_progress) + i123_a.x);
/*GENERATED*/                    this.airTarget.y = (((i124_b.y + -i123_a.y) * i125_progress) + i123_a.y);
/*GENERATED*/                    this.airTarget.z = (((i124_b.z + -i123_a.z) * i125_progress) + i123_a.z);
/*GENERATED*/                    Vector3 i132_a = this.airTarget;
/*GENERATED*/                    Vector3 i133_b = this.posAbs;
/*GENERATED*/                    Vector3 i96_pos2target = new Vector3((i132_a.x + -i133_b.x), (i132_a.y + -i133_b.y), (i132_a.z + -i133_b.z));
/*GENERATED*/                    float i97_posTargetLen = Mathf.Sqrt((float)((
/*GENERATED*/                        (i96_pos2target.x * i96_pos2target.x) + 
/*GENERATED*/                        (i96_pos2target.y * i96_pos2target.y) + 
/*GENERATED*/                        (i96_pos2target.z * i96_pos2target.z))));
/*GENERATED*/                    Vector3 i98_dir;
/*GENERATED*/                    bool _457 = (i97_posTargetLen > 0.01f);
/*GENERATED*/                    if (_457)  {
/*GENERATED*/                        Vector3 newVar_607 = new Vector3(
/*GENERATED*/                            (i96_pos2target.x / i97_posTargetLen), 
/*GENERATED*/                            (i96_pos2target.y / i97_posTargetLen), 
/*GENERATED*/                            (i96_pos2target.z / i97_posTargetLen));
/*GENERATED*/                        i98_dir = newVar_607;
/*GENERATED*/                    } else  {
/*GENERATED*/                        i98_dir = i96_pos2target;
/*GENERATED*/                    }
/*GENERATED*/                    this.speedSlow = 1;
/*GENERATED*/                    bool _458 = (this.airParam > 0.1f);
/*GENERATED*/                    if (_458)  {
/*GENERATED*/                        this.speedSlow = 0.1f;
/*GENERATED*/                    }
/*GENERATED*/                    Vector3 i143_b = this.undockVec;
/*GENERATED*/                    Vector3 i140_a = new Vector3((i98_dir.x + i143_b.x), (i98_dir.y + i143_b.y), (i98_dir.z + i143_b.z));
/*GENERATED*/                    Vector3 i146_a = Vector3.Normalize(i140_a);
/*GENERATED*/                    float forPare_767 = (this.legSpeed * this.speedSlow);
/*GENERATED*/                    this.calcTarget.x = (i146_a.x * forPare_767);
/*GENERATED*/                    this.calcTarget.y = (i146_a.y * forPare_767);
/*GENERATED*/                    this.calcTarget.z = (i146_a.z * forPare_767);
/*GENERATED*/                    this.tmp.v = this.calcTarget;
/*GENERATED*/                    Vector3 i148_a = this.wantedSpeed;
/*GENERATED*/                    Vector3 i149_b = this.tmp.v;
/*GENERATED*/                    this.wantedSpeed.x = (i148_a.x + i149_b.x);
/*GENERATED*/                    this.wantedSpeed.y = (i148_a.y + i149_b.y);
/*GENERATED*/                    this.wantedSpeed.z = (i148_a.z + i149_b.z);
/*GENERATED*/                    bool _26 = (this.wantedSpeed.y < 0);
/*GENERATED*/                    if (_26)  {
/*GENERATED*/                        this.undockCurTime = this.undockTime;
/*GENERATED*/                    }
/*GENERATED*/                    Vector3 i150_THIS = this.wantedSpeed;
/*GENERATED*/                    float i151_max = this.legSpeed;
/*GENERATED*/                    float i152_l = Mathf.Sqrt((float)(((i150_THIS.x * i150_THIS.x) + (i150_THIS.y * i150_THIS.y) + (i150_THIS.z * i150_THIS.z))));
/*GENERATED*/                    Vector3 newVar_15;
/*GENERATED*/                    bool _459 = (i152_l > i151_max);
/*GENERATED*/                    if (_459)  {
/*GENERATED*/                        Vector3 i580_a = Vector3.Normalize(i150_THIS);
/*GENERATED*/                        Vector3 newVar_611 = new Vector3((i580_a.x * i151_max), (i580_a.y * i151_max), (i580_a.z * i151_max));
/*GENERATED*/                        newVar_15 = newVar_611;
/*GENERATED*/                    } else  {
/*GENERATED*/                        newVar_15 = i150_THIS;
/*GENERATED*/                    }
/*GENERATED*/                    this.wantedSpeed = newVar_15;
/*GENERATED*/                    Vector3 i155_a = this.bodySpeed;
/*GENERATED*/                    float i156_d = this.pursueBodySpeed;
/*GENERATED*/                    Vector3 i157_a = this.wantedSpeed;
/*GENERATED*/                    this.wantedSpeed.x = (i157_a.x + (i155_a.x * i156_d));
/*GENERATED*/                    this.wantedSpeed.y = (i157_a.y + (i155_a.y * i156_d));
/*GENERATED*/                    this.wantedSpeed.z = (i157_a.z + (i155_a.z * i156_d));
/*GENERATED*/                } else  {
/*GENERATED*/                     {
/*GENERATED*/                    }
/*GENERATED*/                }
/*GENERATED*/                Vector3 i81_a = this.wantedSpeed;
/*GENERATED*/                Vector3 i82_b = this.speedAbs;
/*GENERATED*/                Vector3 newVar_12 = new Vector3((i81_a.x + -i82_b.x), (i81_a.y + -i82_b.y), (i81_a.z + -i82_b.z));
/*GENERATED*/                float i88_max = this.maxAcceleration;
/*GENERATED*/                float i89_l = Mathf.Sqrt((float)(((newVar_12.x * newVar_12.x) + (newVar_12.y * newVar_12.y) + (newVar_12.z * newVar_12.z))));
/*GENERATED*/                Vector3 newVar_11;
/*GENERATED*/                bool _453 = (i89_l > i88_max);
/*GENERATED*/                if (_453)  {
/*GENERATED*/                    Vector3 i584_a = Vector3.Normalize(newVar_12);
/*GENERATED*/                    Vector3 newVar_615 = new Vector3((i584_a.x * i88_max), (i584_a.y * i88_max), (i584_a.z * i88_max));
/*GENERATED*/                    newVar_11 = newVar_615;
/*GENERATED*/                } else  {
/*GENERATED*/                    newVar_11 = newVar_12;
/*GENERATED*/                }
/*GENERATED*/                this.acceleration = newVar_11;
/*GENERATED*/            } else  {
/*GENERATED*/                 {
/*GENERATED*/                    this.airTime = 0;
/*GENERATED*/                    this.undockPauseCur = 0;
/*GENERATED*/                    this.landTime = (this.landTime + dt);
/*GENERATED*/                    this.wantedSpeed = this.g;
/*GENERATED*/                    Vector3 i166_a = this.wantedSpeed;
/*GENERATED*/                    Vector3 i167_b = this.speedAbs;
/*GENERATED*/                    this.acceleration.x = (i166_a.x + -i167_b.x);
/*GENERATED*/                    this.acceleration.y = (i166_a.y + -i167_b.y);
/*GENERATED*/                    this.acceleration.z = (i166_a.z + -i167_b.z);
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/            Vector3 i39_a = this.speedAbs;
/*GENERATED*/            Vector3 i40_b = this.acceleration;
/*GENERATED*/            this.speedAbs.x = (i39_a.x + i40_b.x);
/*GENERATED*/            this.speedAbs.y = (i39_a.y + i40_b.y);
/*GENERATED*/            this.speedAbs.z = (i39_a.z + i40_b.z);
/*GENERATED*/            Vector3 i41_a = this.speedAbs;
/*GENERATED*/            Vector3 i43_a = this.posAbs;
/*GENERATED*/            this.posAbs.x = (i43_a.x + (i41_a.x * dt));
/*GENERATED*/            this.posAbs.y = (i43_a.y + (i41_a.y * dt));
/*GENERATED*/            this.posAbs.z = (i43_a.z + (i41_a.z * dt));
/*GENERATED*/            bool _22 = this.isExample;
/*GENERATED*/            if (_22)  {
/*GENERATED*/                return ;
/*GENERATED*/            }
/*GENERATED*/            Vector3 i45_a = this.posAbs;
/*GENERATED*/            Vector3 newVar_8 = new Vector3(i45_a.x, (i45_a.y + this.maxLen), i45_a.z);
/*GENERATED*/            Vector3 curPosProjected = this.surfaceDetector.detect(newVar_8, Vector3.up);
/*GENERATED*/            Vector3 i49_a = this.posAbs;
/*GENERATED*/            Vector3 i51_a = this.terrainNormal;
/*GENERATED*/            this.legAlt = (
/*GENERATED*/                (i51_a.x * (i49_a.x + -curPosProjected.x)) + 
/*GENERATED*/                (i51_a.y * (i49_a.y + -curPosProjected.y)) + 
/*GENERATED*/                (i51_a.z * (i49_a.z + -curPosProjected.z)));
/*GENERATED*/            this.wasUnderground = false;
/*GENERATED*/            bool _23 = (this.legAlt < 0.01f);
/*GENERATED*/            if (_23)  {
/*GENERATED*/                this.wasUnderground = true;
/*GENERATED*/                this.posAbs = curPosProjected;
/*GENERATED*/            }
/*GENERATED*/            bool _24 = ((this.legAlt < 0.01f) && this.switchToBottom);
/*GENERATED*/            if (_24)  {
/*GENERATED*/                bool _27 = !this.dockedState;
/*GENERATED*/                if (_27)  {
/*GENERATED*/                    Vector3 i168_a = this.posAbs;
/*GENERATED*/                    Vector3 i169_b = this.bestTargetConservativeAbs;
/*GENERATED*/                    this.lastDockedAtLocal.x = (i168_a.x + -i169_b.x);
/*GENERATED*/                    this.lastDockedAtLocal.y = (i168_a.y + -i169_b.y);
/*GENERATED*/                    this.lastDockedAtLocal.z = (i168_a.z + -i169_b.z);
/*GENERATED*/                    Vector3 i170_a = this.lastDockedAtLocal;
/*GENERATED*/                    Vector3 i171_b = this.lastUndockedAtLocal;
/*GENERATED*/                    float newVar_371 = (i170_a.x + -i171_b.x);
/*GENERATED*/                    float newVar_372 = (i170_a.y + -i171_b.y);
/*GENERATED*/                    float newVar_373 = (i170_a.z + -i171_b.z);
/*GENERATED*/                    float newVar_20 = Mathf.Sqrt((float)(((newVar_371 * newVar_371) + (newVar_372 * newVar_372) + (newVar_373 * newVar_373))));
/*GENERATED*/                    this.lastStepLen = newVar_20;
/*GENERATED*/                }
/*GENERATED*/                this.dockedState = true;
/*GENERATED*/            }
/*GENERATED*/            float i53_at = ((this.lastStepLen / this.legSpeed) * 1.5f);
/*GENERATED*/            Vector3 i55_a = this.bodySpeed;
/*GENERATED*/            float i54_earthTime = (
/*GENERATED*/                this.lastStepLen / 
/*GENERATED*/                Mathf.Sqrt((float)(((i55_a.x * i55_a.x) + (i55_a.y * i55_a.y) + (i55_a.z * i55_a.z)))));
/*GENERATED*/            bool _444 = this.dockedState;
/*GENERATED*/            if (_444)  {
/*GENERATED*/                Vector3 i175_a = this.posAbs;
/*GENERATED*/                Vector3 i177_a = this.bestTargetConservativeAbs;
/*GENERATED*/                Vector3 i178_b = this.lastDockedAtLocal;
/*GENERATED*/                float newVar_382 = (i175_a.x + -(i177_a.x + i178_b.x));
/*GENERATED*/                float newVar_383 = (i175_a.y + -(i177_a.y + i178_b.y));
/*GENERATED*/                float newVar_384 = (i175_a.z + -(i177_a.z + i178_b.z));
/*GENERATED*/                float projCur = Mathf.Sqrt((float)(((newVar_382 * newVar_382) + (newVar_383 * newVar_383) + (newVar_384 * newVar_384))));
/*GENERATED*/                float i182_value = (projCur / this.lastStepLen);
/*GENERATED*/                float i186_arg1;
/*GENERATED*/                bool _460 = (i182_value < 1);
/*GENERATED*/                if (_460)  {
/*GENERATED*/                    i186_arg1 = i182_value;
/*GENERATED*/                } else  {
/*GENERATED*/                    i186_arg1 = 1;
/*GENERATED*/                }
/*GENERATED*/                this.progress = (((0 > i186_arg1)) ? (0) : (i186_arg1));
/*GENERATED*/                this.timedProgress = (this.progress * (i54_earthTime / (i53_at + i54_earthTime)));
/*GENERATED*/            } else  {
/*GENERATED*/                 {
/*GENERATED*/                    Vector3 i189_a = this.posAbs;
/*GENERATED*/                    Vector3 i191_a = this.bestTargetConservativeAbs;
/*GENERATED*/                    Vector3 i192_b = this.lastUndockedAtLocal;
/*GENERATED*/                    float newVar_396 = (i189_a.x + -(i191_a.x + i192_b.x));
/*GENERATED*/                    float newVar_397 = (i189_a.y + -(i191_a.y + i192_b.y));
/*GENERATED*/                    float newVar_398 = (i189_a.z + -(i191_a.z + i192_b.z));
/*GENERATED*/                    float projCur = Mathf.Sqrt((float)(((newVar_396 * newVar_396) + (newVar_397 * newVar_397) + (newVar_398 * newVar_398))));
/*GENERATED*/                    float i196_value = (projCur / this.lastStepLen);
/*GENERATED*/                    float i200_arg1;
/*GENERATED*/                    bool _461 = (i196_value < 1);
/*GENERATED*/                    if (_461)  {
/*GENERATED*/                        i200_arg1 = i196_value;
/*GENERATED*/                    } else  {
/*GENERATED*/                        i200_arg1 = 1;
/*GENERATED*/                    }
/*GENERATED*/                    this.progress = (((0 > i200_arg1)) ? (0) : (i200_arg1));
/*GENERATED*/                    float forPare_766 = (i53_at + i54_earthTime);
/*GENERATED*/                    this.timedProgress = ((this.progress * (i53_at / forPare_766)) + (i54_earthTime / forPare_766));
/*GENERATED*/                }
/*GENERATED*/            }
/*GENERATED*/            bool _445 = float.IsNaN(this.progress);
/*GENERATED*/            if (_445)  {
/*GENERATED*/                this.progress = 0;
/*GENERATED*/            }
/*GENERATED*/            float i57_value = this.progress;
/*GENERATED*/            float i61_arg1;
/*GENERATED*/            bool _446 = (i57_value < 1);
/*GENERATED*/            if (_446)  {
/*GENERATED*/                i61_arg1 = i57_value;
/*GENERATED*/            } else  {
/*GENERATED*/                i61_arg1 = 1;
/*GENERATED*/            }
/*GENERATED*/            this.progress = (((0 > i61_arg1)) ? (0) : (i61_arg1));
/*GENERATED*/            bool _447 = (!this.dockedState && (this.undockPauseCur > this.undockPause));
/*GENERATED*/            if (_447)  {
/*GENERATED*/                this.surfaceDetector.detect(this.posAbs, Vector3.up);
/*GENERATED*/                Vector3 newVar_410 = new Vector3(0, 1, 0);
/*GENERATED*/                Quaternion i203_a = Quaternion.FromToRotation(newVar_410, this.surfaceDetector.normal);
/*GENERATED*/                Quaternion i204_b = this.projectedRot;
/*GENERATED*/                this.possibleFootOrientation.x = ((i203_a.w * i204_b.x) + (i203_a.x * i204_b.w) + (i203_a.y * i204_b.z) + -(i203_a.z * i204_b.y));
/*GENERATED*/                this.possibleFootOrientation.y = ((i203_a.w * i204_b.y) + (i203_a.y * i204_b.w) + (i203_a.z * i204_b.x) + -(i203_a.x * i204_b.z));
/*GENERATED*/                this.possibleFootOrientation.z = ((i203_a.w * i204_b.z) + (i203_a.z * i204_b.w) + (i203_a.x * i204_b.y) + -(i203_a.y * i204_b.x));
/*GENERATED*/                this.possibleFootOrientation.w = ((i203_a.w * i204_b.w) + -(i203_a.x * i204_b.x) + -(i203_a.y * i204_b.y) + -(i203_a.z * i204_b.z));
/*GENERATED*/                float i210_value = (dt * this.footOrientationSpeed);
/*GENERATED*/                float i214_arg1;
/*GENERATED*/                bool _462 = (i210_value < 1);
/*GENERATED*/                if (_462)  {
/*GENERATED*/                    i214_arg1 = i210_value;
/*GENERATED*/                } else  {
/*GENERATED*/                    i214_arg1 = 1;
/*GENERATED*/                }
/*GENERATED*/                float i209_blend;
/*GENERATED*/                bool _463 = (0 > i214_arg1);
/*GENERATED*/                if (_463)  {
/*GENERATED*/                    i209_blend = 0;
/*GENERATED*/                } else  {
/*GENERATED*/                    i209_blend = i214_arg1;
/*GENERATED*/                }
/*GENERATED*/                Quaternion newVar_440 = Quaternion.Lerp(this.footOrientation, this.possibleFootOrientation, i209_blend);
/*GENERATED*/                this.footOrientation = newVar_440;
/*GENERATED*/            }
/*GENERATED*/            bool _448 = this.collectSteppingHistory;
/*GENERATED*/            if (_448)  {
/*GENERATED*/                this.paramHistory.setValue(HistoryInfoBean.deviation, this.deviation);
/*GENERATED*/                float p;
/*GENERATED*/                bool _464 = (this.progress > 0.5f);
/*GENERATED*/                if (_464)  {
/*GENERATED*/                    p = (1 + -this.progress);
/*GENERATED*/                } else  {
/*GENERATED*/                    p = this.progress;
/*GENERATED*/                }
/*GENERATED*/                p = (p * 2);
/*GENERATED*/                this.paramHistory.setValue(HistoryInfoBean.progress, ((this.dockedState) ? (p) : (-p)));
/*GENERATED*/                this.paramHistory.setValue(HistoryInfoBean.land, ((this.dockedState) ? (1) : (0)));
/*GENERATED*/                this.paramHistory.setValue(HistoryInfoBean.switchToBottom, ((this.switchToBottom) ? (1) : (0)));
/*GENERATED*/                this.paramHistory.setValue(HistoryInfoBean.legAlt, (this.legAlt * 3));
/*GENERATED*/                this.paramHistory.setValue(HistoryInfoBean.wasTooLong, ((this.wasTooLong) ? (1) : (0)));
/*GENERATED*/                this.paramHistory.setValue(HistoryInfoBean.undockProgress, this.undockProgress);
/*GENERATED*/            }
/*GENERATED*/        }
        void history() {
            if (this.collectSteppingHistory)  {
                this.paramHistory.setValue(HistoryInfoBean.deviation, this.deviation);
                float p = (((this.progress > 0.5f)) ? ((1 - this.progress)) : (this.progress));
                p = (p * 2);
                this.paramHistory.setValue(HistoryInfoBean.progress, ((this.dockedState) ? (p) : (-p)));
                this.paramHistory.setValue(HistoryInfoBean.land, ((this.dockedState) ? (1) : (0)));
                this.paramHistory.setValue(HistoryInfoBean.switchToBottom, ((this.switchToBottom) ? (1) : (0)));
                this.paramHistory.setValue(HistoryInfoBean.legAlt, (this.legAlt * 3));
                this.paramHistory.setValue(HistoryInfoBean.wasTooLong, ((this.wasTooLong) ? (1) : (0)));
                this.paramHistory.setValue(HistoryInfoBean.undockProgress, this.undockProgress);
            }
        }
        void tickProgress() {
            float at = ((this.lastStepLen / this.legSpeed) * 1.5f);
            float earthTime = (this.lastStepLen / ExtensionMethods.length(this.bodySpeed));
            if (this.dockedState)  {
                float projCur = ExtensionMethods.dist(this.posAbs, (this.bestTargetConservativeAbs + this.lastDockedAtLocal));
                this.progress = MyMath.clamp((projCur / this.lastStepLen), 0, 1);
                this.timedProgress = (this.progress * (earthTime / (at + earthTime)));
            } else  {
                 {
                    float projCur = ExtensionMethods.dist(this.posAbs, (this.bestTargetConservativeAbs + this.lastUndockedAtLocal));
                    this.progress = MyMath.clamp((projCur / this.lastStepLen), 0, 1);
                    this.timedProgress = ((this.progress * (at / (at + earthTime))) + (earthTime / (at + earthTime)));
                }
            }
            if (float.IsNaN(this.progress))  {
                this.progress = 0;
            }
            this.progress = MyMath.clamp(this.progress, 0, 1);
        }
/*GENERATED*/        [Optimize]
/*GENERATED*/        public virtual void tickFoot(float dt) {
/*GENERATED*/            bool _775 = (!this.dockedState && (this.undockPauseCur > this.undockPause));
/*GENERATED*/            if (_775)  {
/*GENERATED*/                this.surfaceDetector.detect(this.posAbs, Vector3.up);
/*GENERATED*/                Vector3 newVar_771 = new Vector3(0, 1, 0);
/*GENERATED*/                Quaternion newVar_770 = Quaternion.FromToRotation(newVar_771, this.surfaceDetector.normal);
/*GENERATED*/                Quaternion i779_b = this.projectedRot;
/*GENERATED*/                this.possibleFootOrientation.x = (
/*GENERATED*/                    (newVar_770.w * i779_b.x) + 
/*GENERATED*/                    (newVar_770.x * i779_b.w) + 
/*GENERATED*/                    (newVar_770.y * i779_b.z) + 
/*GENERATED*/                    -(newVar_770.z * i779_b.y));
/*GENERATED*/                this.possibleFootOrientation.y = (
/*GENERATED*/                    (newVar_770.w * i779_b.y) + 
/*GENERATED*/                    (newVar_770.y * i779_b.w) + 
/*GENERATED*/                    (newVar_770.z * i779_b.x) + 
/*GENERATED*/                    -(newVar_770.x * i779_b.z));
/*GENERATED*/                this.possibleFootOrientation.z = (
/*GENERATED*/                    (newVar_770.w * i779_b.z) + 
/*GENERATED*/                    (newVar_770.z * i779_b.w) + 
/*GENERATED*/                    (newVar_770.x * i779_b.y) + 
/*GENERATED*/                    -(newVar_770.y * i779_b.x));
/*GENERATED*/                this.possibleFootOrientation.w = (
/*GENERATED*/                    (newVar_770.w * i779_b.w) + 
/*GENERATED*/                    -(newVar_770.x * i779_b.x) + 
/*GENERATED*/                    -(newVar_770.y * i779_b.y) + 
/*GENERATED*/                    -(newVar_770.z * i779_b.z));
/*GENERATED*/                float newVar_774 = (dt * this.footOrientationSpeed);
/*GENERATED*/                float i786_arg1;
/*GENERATED*/                bool _821 = (newVar_774 < 1);
/*GENERATED*/                if (_821)  {
/*GENERATED*/                    i786_arg1 = newVar_774;
/*GENERATED*/                } else  {
/*GENERATED*/                    i786_arg1 = 1;
/*GENERATED*/                }
/*GENERATED*/                float newVar_773;
/*GENERATED*/                bool _822 = (0 > i786_arg1);
/*GENERATED*/                if (_822)  {
/*GENERATED*/                    newVar_773 = 0;
/*GENERATED*/                } else  {
/*GENERATED*/                    newVar_773 = i786_arg1;
/*GENERATED*/                }
/*GENERATED*/                Quaternion newVar_772 = Quaternion.Lerp(this.footOrientation, this.possibleFootOrientation, newVar_773);
/*GENERATED*/                this.footOrientation = newVar_772;
/*GENERATED*/            }
/*GENERATED*/        }
        public virtual void checkTooLong() {
            if ((this.comfortFromSkel > 0.98f))  {
                if ((this.dockedState && (this.landTime > 0.2f)))  {
                    this.beginStep(1);
                }
                this.wasTooLong = true;
            } else  {
                 {
                    this.wasTooLong = false;
                }
            }
        }
        public virtual void beginStep(float undockStrength) {
            if (this.collectSteppingHistory)  {
                this.paramHistory.setValue(HistoryInfoBean.beginStep, 1);
            }
            this.stepStarted = true;
            if (this.dockedState)  {
                this.undockPauseCur = (this.undockPause * (1 - undockStrength));
                this.undockPauseCur = MyMath.mix(this.undockPauseCur, this.undockPause, this.deviation);
            }
            if ((undockStrength > 0))  {
                this.switchToBottom = false;
            }
            this.dockedState = false;
            this.undockProgress = (1 - undockStrength);
            this.undockCurTime = (this.undockProgress * this.undockTime);
            this.undockUpLength = MyMath.mix((this.undockInitialStrength * this.undockHDif), 0, this.undockProgress);
            this.targetTo = this.bestTargetProgressiveAbs;
            this.emergencyDown = false;
            this.lastUndockedAtLocal = ExtensionMethods.sub(this.posAbs, this.bestTargetProgressiveAbs);
            this.lastStepLen = ExtensionMethods.dist(this.lastDockedAtLocal, this.lastUndockedAtLocal);
        }
        public virtual void reset(Vector3 pos, Quaternion rot) {
            MUtil.logEvent(this, ("reset " + pos));
            this.comfortRadius = ((this.maxLen * 0.5f) * this.comfortRadiusRatio);
            this.bodyPos = pos;
            this.bodyRot = rot;
            this.bestTargetConservativeUnprojAbs = (Step2.toAbs(this.comfortPosRel, this.bodyPos, rot) + this.additionalDisplacement);
            this.bestTargetConservativeAbs = this.bestTargetConservativeUnprojAbs;
            this.bestTargetProgressiveAbs = this.bestTargetConservativeUnprojAbs;
            this.posAbs = this.bestTargetConservativeUnprojAbs;
            this.oldPosAbs = this.bestTargetConservativeUnprojAbs;
            this.speedAbs = new Vector3();
            this.acceleration = new Vector3();
            this.undockPauseCur = 0;
            this.airTime = 0;
            this.undockProgress = 100500;
            this.undockCurTime = 100500;
            this.footOrientation = rot;
        }
        public static Vector3 toAbs(Vector3 abs, Vector3 pos, Quaternion rot) {
            return ExtensionMethods.add(ExtensionMethods.rotate(rot, abs), pos);
        }
        public static float getComfortRadius(Vector3 baseAbs, Vector3 targetAbs, float maxLen) {
            Vector3 vec = (targetAbs - baseAbs);
            float down = vec.scalarProduct(Vector3.up);
            float right = (float)(Math.Sqrt((vec.sqrMagnitude - MyMath.sqr(down))));
            CircleLineIntersection.inst.calc(maxLen, -100, down, 100, down);
            if ((CircleLineIntersection.inst.resDisc >= 0))  {
                return Math.Min(Math.Abs((CircleLineIntersection.inst.resX1 - right)), Math.Abs((CircleLineIntersection.inst.resX2 - right)));
            }
            return 0;
        }
        public virtual void targetFill(P<Vector3> pvec) {
            Vector3 from = this.posAbs;
            float fromToDist = ExtensionMethods.length(ExtensionMethods.withSetY(ExtensionMethods.sub(from, this.targetTo), 0));
            this.curTarget = ExtensionMethods.add(this.targetTo, 0, this.targetDockR, 0);
            if ((((fromToDist > this.targetDockR) && !this.switchToBottom) || (this.undockProgress < 0.3f)))  {
            } else  {
                 {
                    this.switchToBottom = true;
                    this.curTarget = ExtensionMethods.sub(this.targetTo, 0, this.targetDockR, 0);
                }
            }
            this.curTarget = ExtensionMethods.limit(this.curTarget, this.basisAbs, (this.maxLen * 1.5f));
            this.airParam = ((this.canGoAir) ? (MyMath.clamp(((this.bestTargetConservativeUnprojAbs2.y - this.curTarget.y) / this.maxLen), 0, 1)) : (0));
            this.airTarget = ExtensionMethods.mix(this.curTarget, this.bestTargetConservativeUnprojAbs2, this.airParam);
            Vector3 pos2target = ExtensionMethods.sub(this.airTarget, this.posAbs);
            float posTargetLen = ExtensionMethods.length(pos2target);
            Vector3 dir = (((posTargetLen > 0.01f)) ? ((pos2target / posTargetLen)) : (pos2target));
            this.speedSlow = 1;
            if ((this.airParam > 0.1f))  {
                this.speedSlow = 0.1f;
            }
            this.calcTarget = (ExtensionMethods.normalized(ExtensionMethods.add(dir, this.undockVec), this.legSpeed) * this.speedSlow);
            pvec.v = this.calcTarget;
        }
        public virtual void undockTick(float dt) {
            this.undockCurTime += dt;
            this.undockProgress = (this.undockCurTime / this.undockTime);
            if ((this.undockProgress >= 1))  {
                this.undockProgress = 1;
                this.undockUpLength = 0;
                this.undockVec = new Vector3();
            } else  {
                 {
                    this.undockUpLength = MyMath.mix(((this.undockInitialStrength / this.legSpeed) * this.undockHDif), 0, this.undockProgress);
                    this.undockVec = new Vector3(0, this.undockUpLength, 0);
                }
            }
            this.undockVec += ExtensionMethods.normalized((this.basisAbs - this.posAbs), ((this.undockInitialStrength / 3) * (1 - this.undockProgress)));
        }

    }
}
