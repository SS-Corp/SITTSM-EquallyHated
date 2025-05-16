using UnityEngine;
using HarmonyLib;
using Framework;
using System.Reflection;
using System.Collections;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(CharacterMovement))]
    internal class CharacterMovementPatch {
        static CharacterMovementState originalRunState;
        static CharacterMovementState originalRunCrippledState;

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostfix(CharacterMovement __instance) {
            FieldInfo runStateField = typeof(CharacterMovement).GetField("runState", BindingFlags.NonPublic | BindingFlags.Instance);
            CharacterMovementState runStateInstance = (CharacterMovementState)runStateField?.GetValue(__instance);
            FieldInfo runCrippledStateField = typeof(CharacterMovement).GetField("runCrippledState", BindingFlags.NonPublic | BindingFlags.Instance);
            CharacterMovementState runCrippledStateInstance = (CharacterMovementState)runCrippledStateField?.GetValue(__instance);

            if (originalRunState == null) {
                originalRunState = runStateInstance;
            }
            if (originalRunCrippledState == null) {
                originalRunCrippledState = runCrippledStateInstance;
            }

        }

        [HarmonyPatch("CalculateState")]
        [HarmonyPrefix]
        static bool CalculateStatePrefix(CharacterMovement __instance) {
            FieldInfo moveXZPlaneField = typeof(CharacterMovement).GetField("_moveOnXZPlane", BindingFlags.NonPublic | BindingFlags.Instance);
            bool moveXZPlaneInstance = (bool)moveXZPlaneField?.GetValue(__instance);

            if (!__instance.Health.IsAlive) return true;
            
            if (!moveXZPlaneInstance) {
                if (__instance.Input.HoldingLeft() || __instance.Input.HoldingRight()) {
                    if (__instance.Input.HoldingDown()) {
                        __instance.PlayMovementState(originalRunCrippledState, false, null);
                        return false;
                    }
                }
            }

            return true;
        }
        [HarmonyPatch(nameof(CharacterMovement.PlayIdleState))]
        [HarmonyPostfix]
        static void PlayIdleStatePostfix(CharacterMovement __instance) {
            FieldInfo runCrippledStateField = typeof(CharacterMovement).GetField("runCrippledState", BindingFlags.NonPublic | BindingFlags.Instance);
            CharacterMovementState runCrippledStateInstance = (CharacterMovementState)runCrippledStateField?.GetValue(__instance);

            FieldInfo moveXZPlaneField = typeof(CharacterMovement).GetField("_moveOnXZPlane", BindingFlags.NonPublic | BindingFlags.Instance);
            bool moveXZPlaneInstance = (bool)moveXZPlaneField?.GetValue(__instance);

            if (!moveXZPlaneInstance && __instance.Input.HoldingDown()) {
                __instance.PlayMovementState(runCrippledStateInstance, false, null);
            } 
        }

        //MISS
        [HarmonyPatch("ConsiderEndingComboRoutine")]
        [HarmonyPostfix]
        static IEnumerator ConsiderEndingComboRoutinePostfix(IEnumerator result, CharacterMovement __instance) {
            while (result.MoveNext())
                yield return result.Current;

            FieldInfo lessClumsyField = typeof(CharacterMovement).GetField("_lessClumsy", BindingFlags.NonPublic | BindingFlags.Instance);
            bool InstanceLessClumsy = (bool)lessClumsyField?.GetValue(__instance);

            FieldInfo lessClumsyTimeField = typeof(CharacterMovement).GetField("_lastClumsyTime", BindingFlags.NonPublic | BindingFlags.Instance);
            float InstanceLessClumsyTime = (float)lessClumsyTimeField?.GetValue(__instance);

            FieldInfo lessClumsyDelayField = typeof(CharacterMovement).GetField("_lessClumsyDelay", BindingFlags.NonPublic | BindingFlags.Instance);
            float InstanceLessClumsyDelay = (float)lessClumsyDelayField?.GetValue(__instance);

            if (__instance.Unit.Faction == FactionType.Hero && SingletonBehaviour<PassiveManager>.HasInstance && SingletonBehaviour<PassiveManager>.Instance.EnemiesAreClumsy() && (!InstanceLessClumsy || Time.time - InstanceLessClumsyTime > InstanceLessClumsyDelay)) {
                lessClumsyDelayField.SetValue(__instance, Time.time);
                __instance.ClumsyMissedEnemyAttack(true);

                EquallyHatedModBase.Logger.LogInfo("clumsy");
            }
        }

        //JUMP!
        [HarmonyPatch(nameof(CharacterMovement.TryJumpOffEnemies))]
        [HarmonyPrefix]
        static bool TryJumpOffEnemiesPatch(ref bool __result, ref bool grappleOverEnemy) {
            grappleOverEnemy = false;
            __result = false;
            return false; //skip
        }
        [HarmonyPatch(nameof(CharacterMovement.TryJumpOffOtherPlayer))]
        [HarmonyPrefix]
        static bool TryJumpOffOtherPlayerPatch(ref bool __result, ref bool grappleOverHero) {
            grappleOverHero = false;
            __result = false;
            return false; //skip
        }
    }
}
