using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(CharacterOnFire))]
    internal class CharacterOnFirePatch {

        [HarmonyPatch(nameof(CharacterOnFire.StartBurning))]
        [HarmonyPrefix]
        static void StartBurningPrefix(CharacterOnFire __instance, ref float time) {
            FieldInfo spreadFireField = typeof(CharacterOnFire).GetField("_shouldSpreadFire", BindingFlags.NonPublic | BindingFlags.Instance);

            time = 10f; // reallistic.
            spreadFireField.SetValue(__instance, true);
        }
    }
}
