using System;
using System.Collections.Generic;
using moveen.core;
using moveen.utils;
using UnityEngine;

namespace moveen.descs {
    [ExecuteInEditMode] //to init structure properly
    public abstract class MoveenSkelWithBones : MoveenSkelBase {
        [HideInInspector]public List<Vector3> wanteds;//TODO make null when not used

        [Range(0, 1)] public float transition;

        public List<Transform> bonesGeometry; //TODO call MUtil2.al<Transform>(null) in children only for first time
        [NonSerialized] public bool isInError;
        [NonSerialized] public List<Bone> bones = MUtil2.al<Bone>();

        [HideInInspector] public List<Vector3> bonesDeltaPos = new List<Vector3>();
        [HideInInspector] public List<Quaternion> bonesDeltaRot = new List<Quaternion>();
//        [HideInInspector] public List<Vector3> childrenDeltaPos = new List<Vector3>();
//        [HideInInspector] public List<Quaternion> childrenDeltaRot = new List<Quaternion>();

        //we can't use this transform's pos/rot as it corresponds to the position of the first (or the only) bone 
        [HideInInspector] public Vector3 basePos;

        [HideInInspector] public Quaternion baseRot;
        [HideInInspector] public Vector3 cachedLocalPos;
        [HideInInspector] public Quaternion cachedLocalRot;

        protected MoveenSkelWithBones() {
            executionOrder = 20;
        }
        
        public override void tick(float dt) {
            isInError = false;

            basePos = transform.position;
            baseRot = transform.rotation;
            cachedLocalPos = transform.localPosition;
            cachedLocalRot = transform.localRotation;

//            if (Application.isPlaying) {
//                if (transform.parent != null) {
//                    Quaternion parentRotation = transform.parent.rotation;
//                    basePos = transform.parent.TransformPoint(cachedLocalPos);
////                    basePos = transform.parent.position + parentRotation.rotate(cachedLocalPos);
//                    baseRot = parentRotation * cachedLocalRot;
//                } else {
//                    basePos = cachedLocalPos;
//                    baseRot = cachedLocalRot;
//                }
//            } else {
//                basePos = transform.position;
//                baseRot = transform.rotation;
//                cachedLocalPos = transform.localPosition;
//                cachedLocalRot = transform.localRotation;
//            }

            base.tick(dt);
            tickStructure(dt);
            for (int i = 0; i < bones.Count; i++) bones[i].origin.tick();
            if (Application.isPlaying) {
                copyTransforms();
                //copyChildrenTransforms();
            } else {
                updateDeltas();
                //updateChildrenDeltas();
            }
            copyChildrenTransformsDeltless();
        }

        public virtual void tickStructure(float dt) {
        }

        //actual params copy, after deserialization included
        public override void updateData() {
            base.updateData();
            updateBones();
//            updateChildren();
        }

        private void updateBones() {
            MUtil.madeCount(bonesDeltaPos, bonesGeometry.Count);
            MUtil.madeCount(bonesDeltaRot, bonesGeometry.Count);
            for (int index = 0; index < bonesGeometry.Count; index++) {
                Bone bone = bones[index];
                bone.attachedGeometry = bonesGeometry[index];
                bone.deltaPos = bonesDeltaPos[index];
                bone.deltaRot = bonesDeltaRot[index];
            }
        }

//        private void updateChildren() {
//            MUtil.madeCount(childrenDeltaPos, bonesGeometry.Count);
//            MUtil.madeCount(childrenDeltaRot, bonesGeometry.Count);
//
////            if (!Application.isPlaying) {
////                cachedLocalPos = transform.localPosition;
////                cachedLocalRot = transform.localRotation;
////            }
//        }

        private void updateDeltas() {
            for (int index = 0; index < bonesGeometry.Count; index++) {
                Transform geometry = bonesGeometry[index];
                if (geometry != null) {
                    bones[index].origin.tick();
                    Quaternion geomR = geometry.rotation;
                    Quaternion skelR = bones[index].origin.getRot();

                    bonesDeltaPos[index] = skelR.conjug().rotate(geometry.position - bones[index].origin.getPos());
                    bonesDeltaRot[index] = geomR.rotSub(skelR);
                }
            }
        }

//        private void updateChildrenDeltas() {
////            childrenDeltaPos[0] = transform.localPosition;
////            childrenDeltaRot[0] = transform.localRotation;
////            childrenDeltaPos[0] = bones[0].origin.getRot().rotate(transform.position - bones[0].origin.getPos());
////            childrenDeltaRot[0] = transform.rotation.rotSub(bones[0].origin.getRot());
//            {
//                Quaternion geomR = transform.rotation;
//                Quaternion skelR = bones[0].origin.getRot();
//
//                bones[0].origin.tick();
//                childrenDeltaPos[0] = skelR.conjug().rotate(transform.position - bones[0].origin.getPos());
//                childrenDeltaRot[0] = geomR.rotSub(skelR);
//
//            }
//
//            //0 not needed, as this transform gives needed pos/rot
//            //  so 0 child and every child outside bones bondaries - just stay on their local pos/rot
//            for (int index = 1; index < bones.Count; index++) {
//                if (transform.childCount <= index) break;
//                Transform child = transform.GetChild(index);
//                bones[index].origin.tick();
//                Quaternion geomR = child.rotation;
//                Quaternion skelR = bones[index].origin.getRot();
//
//                childrenDeltaPos[index] = skelR.conjug().rotate(child.position - bones[index].origin.getPos());
//                childrenDeltaRot[index] = geomR.rotSub(skelR);
//            }
//        }

        [NonSerialized] private bool canExpectExternalChange;

        private void copyTransforms() {
            for (int i = 0; i < bones.Count; i++) {
                Bone bone = bones[i];
                if (transition == 1) {
                } else if (transition == 0) {
                    bone.copy();
                } else {
                    bone.copy(bone.attachedGeometry, transition);
                }
            }
        }

//        private void copyChildrenTransforms() {
////            transform.SetPositionAndRotation(bones[0].origin.getPos(), bones[0].origin.getRot());
//            {
//                Transform child = transform;
//                Bone bone = bones[0];
//                if (bone.attachedGeometry == null)
//                    child.SetPositionAndRotation(
//                        bone.origin.getPos() + bone.origin.getRot().rotate(childrenDeltaPos[0]),
//                        bone.origin.getRot() * childrenDeltaRot[0]);
//            }
//            for (int i = 1; i < bones.Count; i++) {
//                if (transform.childCount <= i) break;
//                Transform child = transform.GetChild(i);
//                Bone bone = bones[i];
//                if (bone.attachedGeometry == null)
//                    child.SetPositionAndRotation(
//                        bone.origin.getPos() + bone.origin.getRot().rotate(childrenDeltaPos[i]),
//                        bone.origin.getRot() * childrenDeltaRot[i]);
//            }
//        }

        private void copyChildrenTransformsDeltless() {
//            if (!Application.isPlaying) {
//                Transform child = transform;
//                Bone bone = bones[0];
//                child.SetPositionAndRotation( bone.origin.getPos(), bone.origin.getRot());
//            }
            for (int i = 0; i < bones.Count; i++) {
                if (transform.childCount <= i) break;
                Transform child = transform.GetChild(i);
                Bone bone = bones[i];
                child.SetPositionAndRotation(bone.origin.getPos(), bone.origin.getRot());
            }
        }

        public virtual void createRagdoll() {
            for (int i = 0; i < bones.Count; i++) {
                Transform affected = getAffected(this, i);
                if (affected != null && bones[i].r != 0) createCapsule(affected, bones[i].deltaRot, bones[i].r);
            }
            for (int i = 0; i < bones.Count - 1; i++) {
                Transform p0 = getAffected(this, i);
                Transform p1 = getAffected(this, i + 1);
                if (p0 != null && p1 != null && p0.GetComponent<Rigidbody>() != null && p1.GetComponent<Rigidbody>() != null) {
                    connectToParentHingeJoint(p1, p0, new Vector3(0, 0, 1));
                }
            }
        }

        public static Transform getAffected(MoveenSkelWithBones bones, int i) {
            if (bones.bones[i].attachedGeometry != null) return bones.bones[i].attachedGeometry;
            if (bones.transform.childCount > i) return bones.transform.GetChild(i);
            return null;
        }

        public static CharacterJoint connectToParentCharacterJoint(Transform a, Transform p) {
            if (a == null || p == null) return null;
            CharacterJoint[] joints = a.gameObject.GetComponents<CharacterJoint>();
            for (int i = 1; i < joints.Length; i++) {
                DestroyImmediate(joints[i]);
            }
            CharacterJoint joint = a.gameObject.GetComponent<CharacterJoint>();
            joint = joint != null ? joint : a.gameObject.AddComponent<CharacterJoint>();
            joint.connectedBody = p.GetComponent<Rigidbody>();
            return joint;
        }

        public static void connectToParentHingeJoint(Transform a, Transform p, Vector3 jointAxis) {
            if (a == null || p == null) return;
            Joint[] joints = a.gameObject.GetComponents<Joint>();
            for (int i = 0; i < joints.Length; i++) {
                DestroyImmediate(joints[i]);
            }
            HingeJoint joint = a.gameObject.AddComponent<HingeJoint>();
            joint.useLimits = true;
            joint.limits = new JointLimits {min = -45, max = 45};
            joint.axis = jointAxis;
            joint.connectedBody = p.GetComponent<Rigidbody>();
        }

        public static T ensureOneComponent<T>(Transform t) where T : Component {
            T[] cc = t.GetComponents<T>();
            for (int i = 1; i < cc.Length; i++) DestroyImmediate(cc[i]);
            return cc.Length > 0 ? cc[0] : t.gameObject.AddComponent<T>();
        }

        public static void createCapsule(Transform target, Quaternion rot, float height) {
            if (target == null) return;
            CapsuleCollider capsuleCollider = ensureOneComponent<CapsuleCollider>(target);
            ensureOneComponent<Rigidbody>(target);

            Vector3 vec = rot.conjug().rotate(new Vector3(1, 0, 0));
            //the same effect:
//            Vector3 vec = bone.origin.getRot().rotate(new Vector3(1, 0, 0));
//            vec = bone.attachedGeometry.rotation.conjug().rotate(vec);
//            Vector3 vec = (bone.attachedGeometry.rotation.conjug() * bone.origin.getRot()).rotate(new Vector3(1, 0, 0));
//            Vector3 vec = bone.origin.getRot().rotSub(bone.attachedGeometry.rotation).rotate(new Vector3(1, 0, 0));

            Vector3 center = new Vector3();
            if (Math.Abs(vec.x) > Math.Abs(vec.y) && Math.Abs(vec.x) > Math.Abs(vec.z)) {
                capsuleCollider.direction = 0;
                center = new Vector3(vec.x > 0 ? 1 : -1, 0, 0);
            }
            if (Math.Abs(vec.y) > Math.Abs(vec.x) && Math.Abs(vec.y) > Math.Abs(vec.z)) {
                capsuleCollider.direction = 1;
                center = new Vector3(0, vec.y > 0 ? 1 : -1, 0);
            }
            if (Math.Abs(vec.z) > Math.Abs(vec.x) && Math.Abs(vec.z) > Math.Abs(vec.y)) {
                capsuleCollider.direction = 2;
                center = new Vector3(0, vec.z > 0 ? 1 : -1);
            }

            if (height == 0) height = 1;
            capsuleCollider.height = height;
            capsuleCollider.radius = height * 0.2f;
            capsuleCollider.center = center * height / 2;
        }

        public static void createCapsule2(Transform target, float height, float radius) {
            if (target == null) return;
            CapsuleCollider capsuleCollider = ensureOneComponent<CapsuleCollider>(target);
            ensureOneComponent<Rigidbody>(target);
            capsuleCollider.height = height;
            capsuleCollider.radius = radius;
        }

        //This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
        public override void OnValidate() {
            MUtil.logEvent(this, "OnValidate");
            base.OnValidate();
            int targetBonesCount = bones.Count;
            if (bonesGeometry.Count != targetBonesCount) {
                Debug.LogWarning("Don't change array size!");
                if (bonesGeometry.Count > targetBonesCount) {
                    bonesGeometry.RemoveRange(targetBonesCount, bonesGeometry.Count - targetBonesCount);
                }
                while (bonesGeometry.Count < targetBonesCount) {
                    bonesGeometry.Add(null);
                }
            }
            needsUpdate = true;
        }

        public override void reset() {
            MUtil.logEvent(this, "reset");
            base.reset();
            tick(1);
        }

        public virtual bool canBeSolved() {
            return false;
        }
    }
}