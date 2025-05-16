using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(ExplodeDetachedWeapon))]
    internal class ExplodeDetachedWeaponPatch {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostfix(ExplodeDetachedWeapon __instance) {
            FieldInfo explosionField = typeof(ExplodeDetachedWeapon).GetField("_explosionPrefab", BindingFlags.NonPublic | BindingFlags.Instance);
            Explosion InstanceExplosion = (Explosion)explosionField.GetValue(__instance);

            if (ReuseAssets.explosion == null && InstanceExplosion != null) {
                ReuseAssets.explosion = InstanceExplosion;
            }
        }
    }
}
