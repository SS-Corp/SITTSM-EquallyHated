using UnityEngine;
using HarmonyLib;
using Framework;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(PassiveManager))]
    internal class PassiveManagerPatch {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void UpdatePostFix(PassiveManager __instance) {
            FieldInfo fartSplitField = typeof(PassiveManager).GetField("_fartsCanSplit", BindingFlags.NonPublic | BindingFlags.Instance);
            fartSplitField.SetValue(__instance, true);

            FieldInfo homingField = typeof(PassiveManager).GetField("_projectilesCanHome", BindingFlags.NonPublic | BindingFlags.Instance);
            homingField.SetValue(__instance, true);
        }
    }
}
