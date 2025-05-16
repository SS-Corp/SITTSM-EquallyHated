using UnityEngine;
using HarmonyLib;
using S.EquallyHated.Scripts;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(ElevatorEnterTrigger))]
    internal class ElevatorEnterTriggerPatch {
        static Vector3 speechOffset = new Vector3(0, -2.5f, 0);

        [HarmonyPatch(nameof(ElevatorEnterTrigger.Collect))]
        [HarmonyPrefix]
        static bool CollectPrefix(ElevatorEnterTrigger __instance, CharacterMovement character) {
            if (PlayerManager.NumberOfPlayersWithAliveStickMen() > 1) { // 
                ErrorElevator(__instance);
                return false; // SKIPPED!
            }
            else {
                return true; //ok yea go ahead
            }
        }
        static void ErrorElevator(ElevatorEnterTrigger __instance) {
            WeightLimitHelper helper = __instance.GetComponent<WeightLimitHelper>();
            if (helper == null) {
                helper = __instance.gameObject.AddComponent<WeightLimitHelper>();
            }

            helper.ShowMessage();
        }

    }
}
