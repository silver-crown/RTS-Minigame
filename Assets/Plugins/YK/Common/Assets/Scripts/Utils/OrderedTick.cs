using System;
using System.Collections.Generic;
using moveen.descs;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace moveen.utils {
    public static class OrderedTick {
        public static bool tickInLateUpdate = true;
        private static List<IOrderableTick> all = new List<IOrderableTick>();
        private static List<IOrderableTick> toAdd = new List<IOrderableTick>();
        private static bool readyForUpdate = !tickInLateUpdate;
        private static bool isSorted;

//        public static CounterStacksCollection paramHistory = new CounterStacksCollection(100);//TODO some global switch

        public static List<HistoryInfoBean> historyBeans = new List<HistoryInfoBean>() {
            new HistoryInfoBean("fixedUpdate", Color.red),
            new HistoryInfoBean("lateUpdate", Color.green),
        };


        public static void setUnsorted() {
            isSorted = false;
        }

        public static void addComponent(IOrderableTick co) {
            all.Add(co);
//            toAdd.Add(co);
            isSorted = false;
        }

        //TODO delete centrally, because of possible skip
        //  and check "is enabled"
        public static void deleteComponent(IOrderableTick co) {
            all.Remove(co);
        }

        //TODO remember indices (when usecase will be available)
        //TODO fix broken order here
        public static void deleteComponentFast(OrderedMonoBehaviour co) {
            int i = all.IndexOf(co);
            if (i == -1) {
                Debug.LogError("index of MonoBehaviour == -1");
            } else {
                all[i] = all[all.Count - 1];
                all.RemoveAt(all.Count - 1);
                isSorted = false;
            }
        }

        public static bool readyForfixedUpdate;

        public static void onUnityFixedUpdate() {
            if (!readyForfixedUpdate) return;
//            paramHistory.next();
//            paramHistory.setValue("fixedUpdate", -1);
            for (int i = 0; i < all.Count; i++) if (all[i].participateInPhysicsUpdate()) all[i].fixedTick(Time.deltaTime);
            readyForfixedUpdate = false;
            
            //because we could leave it false with tester
            //and we currently not reset it in Update/LateUpdate but in FixedUpdate instead
            readyForUpdate = true; 
        }

        public static void onUnityLateFixedUpdate() {
            readyForfixedUpdate = true;

            if (tester != null) {
                if (tester.haveToUpdateAfterFixedUpdate()) {
                    tick(Time.deltaTime);
                    //readyForUpdate = true;//true - will allow Update right after (Unity may or may not call it after FixedUpdate)
                }
                readyForUpdate = false;
            }
        }

        public static void onUnityUpdate() {
            if (tickInLateUpdate) finishCurrentUpdateCycle();
            else tick(Time.deltaTime);
        }

        public static void onUnityLateUpdate() {
            if (tickInLateUpdate) tick(Time.deltaTime);
            else finishCurrentUpdateCycle();
        }

        private static OrderedTicksTest tester = null;
        
        //TODO stress tests:
        //  1 from 5 updates isn't called
        //  2 updates per 1 fixed update
        //  strict Update after FixedUpdate (in late fixed udpate)
        //  random modes and selected modes
        private static int skip;


        public static void tick(float dt) {
            if (!readyForUpdate) return;
//            paramHistory.setValue("lateUpdate", 1);

            if (!isSorted) {
                for (int i = 0; i < toAdd.Count; i++) all.Add(toAdd[i]);
                toAdd.Clear();
                all.Sort(compareOrderable);
                isSorted = true;
            }
            try {
                for (int i = 0; i < all.Count; i++) if (all[i].doParticipateInsUpdate()) all[i].tick(dt);
            }
            catch (Exception e) {
                Debug.Log(e);
            }
            readyForUpdate = false;
        }

        public static void forceTick(float dt) {
            for (int i = 0; i < all.Count; i++) all[i].tick(dt);
        }

        private static void finishCurrentUpdateCycle() {
            //we can't skip this in editor mode because FixedUpdate isn't called in it
            //TODO is run? refactor other place?
            readyForUpdate = true;
        }

        private static int compareOrderable(IOrderableTick a, IOrderableTick b) {
            return a.getOrder() - b.getOrder();
        }
    }
}