using UnityEngine;
using HarmonyLib;
using S.EquallyHated.Scripts;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(PresentSpawner))]
    internal class PresentSpawnerPatch {
        
        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        static bool UpdatePrefix(PresentSpawner __instance) {
            return false;
        }

        internal static void SpawnPresent() {
            if (PresentSpawner.HasInstance) {
                if (PassiveManager.Instance.IsAScrounger()) {
                    PresentSpawner.Instance.SpawnPresent(null, false); // get 2 for 1 deal
                }
                PresentSpawner.Instance.SpawnPresent(null, false); //default
            }
        }
    }
}
