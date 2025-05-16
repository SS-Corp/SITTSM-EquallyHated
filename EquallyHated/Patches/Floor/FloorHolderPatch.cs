using UnityEngine;
using HarmonyLib;
using S.EquallyHated.Scripts;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(FloorHolder))]
    internal class FloorHolderPatch {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostfix(FloorHolder __instance) {
            CheckFloorToMod(__instance);
        }

        static void CheckFloorToMod(FloorHolder __instance) {
            if (__instance.gameObject.name.Contains("Dance Party")) {
                FloorOfficeParty party = __instance.gameObject.AddComponent<FloorOfficeParty>();
            }
            //more?
        }
    }
}
