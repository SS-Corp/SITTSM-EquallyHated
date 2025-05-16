using UnityEngine;
using HarmonyLib;
using System.Collections;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(LevelManager))]
    internal class LevelManagerPatch {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostFix(LevelManager __instance) {
            __instance.StartCoroutine(IEStartPatch(__instance));
        }

        static IEnumerator IEStartPatch(LevelManager __instance) {
            FieldInfo friendlyFireField = typeof(LevelManager).GetField("_friendlyFireOn", BindingFlags.NonPublic | BindingFlags.Static);
            yield return new WaitForSeconds(4f); // wait until worker GETS TIRED OF THIS!!!

            friendlyFireField.SetValue(null, true);
            EquallyHatedModBase.Logger.LogInfo($"Friendly fire = { (bool)friendlyFireField.GetValue(__instance)}");
        }
    }
}
