using UnityEngine;
using HarmonyLib;
using S.EquallyHated.Scripts;
using System.Collections;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(Unit))]
    class UnitPatch {

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(Unit __instance) {
            __instance.StartCoroutine(StartWait(__instance));
        }

        static IEnumerator StartWait(Unit __instance) {
            yield return new WaitForEndOfFrame();

            CheckCurrentFloor(__instance);
        }

        static void CheckCurrentFloor(Unit __instance) {
            FloorHolder floor = __instance.GetComponentInParent<FloorHolder>();
            if (floor == null) return;

            if (floor.gameObject.name.Contains("Floor Intro")) {
                IntroHate(__instance);
            } // make more! maybe?
        }

        static void IntroHate(Unit __instance) {
            if (__instance.Movement.SittingOnChair == null) return;
            WorkerFloorIntro workerTalk = __instance.gameObject.AddComponent<WorkerFloorIntro>();
            workerTalk.SetupUnit(__instance);
        }
    }
}
