using UnityEngine;
using HarmonyLib;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(CharacterPhysics))]
    internal class CharacterPhysicsPatch {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(CharacterPhysics __instance) {
            if (UnityEngine.Random.Range(9, 10) == UnityEngine.Random.Range(1, 21)) { //whats 9 + 10
                __instance.CrippleLimbs(); //lel
            }
        }
    }
}
