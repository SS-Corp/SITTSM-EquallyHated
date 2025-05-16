using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(ResizePhysicsCharacter))]
    internal class ResizePhysicsCharacterPatch {
        static int maxExtraDamage = 1;

        [HarmonyPatch(nameof(ResizePhysicsCharacter.GetExtraDamage))]
        [HarmonyPrefix]
        static bool GetExtraDamagePrefix(ResizePhysicsCharacter __instance, ref int __result) {
            FieldInfo resizeCountField = typeof(ResizePhysicsCharacter).GetField("_resizeCount", BindingFlags.NonPublic | BindingFlags.Instance);
            int resizeCountInstance = (int)resizeCountField.GetValue(__instance);
            
            if (resizeCountInstance > maxExtraDamage) {
                __result = maxExtraDamage;
            }
            else {
                __result = resizeCountInstance;
            }

            return false;
        }

        public static void ResizePermanently(ResizePhysicsCharacter __instance, float size) {
            FieldInfo initialSizeField = typeof(ResizePhysicsCharacter).GetField("_initialSize", BindingFlags.NonPublic | BindingFlags.Instance);
            float initialSizeInstance = (float)initialSizeField.GetValue(__instance);

            float newSize = initialSizeInstance + size;

            initialSizeField.SetValue(__instance, newSize);

            __instance.Resize(size, 5);
        }
    }
}
