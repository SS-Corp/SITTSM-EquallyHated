using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(BonusPresent))]
    internal class BonusPresentPatch {
        static float healSize = .05f;
        
        [HarmonyPatch(nameof(BonusPresent.Use))]
        [HarmonyPostfix]
        static void UsePostfix(BonusPresent __instance, CharacterMovement character) {
            UseHealth(__instance, character);
        }

        static void UseHealth(BonusPresent __instance, CharacterMovement character) {
            FieldInfo receiveHealthField = typeof(BonusPresent).GetField("_receiveHealth", BindingFlags.NonPublic | BindingFlags.Instance);
            bool receiveHealthInstance = (bool)receiveHealthField.GetValue(__instance);
            FieldInfo receiveOneHealthField = typeof(BonusPresent).GetField("_receiveOneHealth", BindingFlags.NonPublic | BindingFlags.Instance);
            bool receiveOneHealthInstance = (bool)receiveOneHealthField.GetValue(__instance);

            if (receiveHealthInstance || receiveOneHealthInstance) {
                ResizePhysicsCharacterPatch.ResizePermanently(character.GetComponent<ResizePhysicsCharacter>(), healSize);
                EquallyHatedModBase.Logger.LogInfo("grow fatter");
            }
        }
    }
}
