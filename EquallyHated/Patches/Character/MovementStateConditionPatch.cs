using UnityEngine;
using HarmonyLib;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(MovementStateCondition))]
    class MovementStateConditionPatch {
        [HarmonyPatch(nameof(MovementStateCondition.Evaluate))]
        [HarmonyPrefix]
        static bool EvaluatePrefix(ref bool __result) {
            // prob not a good way to STOP THE FIRE
            // but i am tired of thisss

            __result = true;
            return false;
        }
    }
}
