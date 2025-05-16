using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(HandGrabber))]
    internal class HandGrabberPatch {
        [HarmonyPatch("TryGrab", new Type[] { })]
        [HarmonyPrefix]
        static bool TryGrabPreFix(HandGrabber __instance) {
            FieldInfo unitField = typeof(HandGrabber).GetField("_unit", BindingFlags.NonPublic | BindingFlags.Instance);
            Unit InstanceUnit = (Unit)unitField?.GetValue(__instance);

            FieldInfo grabDummyField = typeof(HandGrabber).GetField("_grabDummy", BindingFlags.NonPublic | BindingFlags.Instance);
            Transform InstanceGrabDummy = (Transform)grabDummyField?.GetValue(__instance);

            FieldInfo enemyGrabbedOntoField = typeof(HandGrabber).GetField("_enemyGrabbedOnto", BindingFlags.NonPublic | BindingFlags.Instance);

            FieldInfo grabSoundField = typeof(HandGrabber).GetField("_grabSound", BindingFlags.NonPublic | BindingFlags.Instance);
            EffectSoundBank InstanceGrabSound = (EffectSoundBank)grabSoundField?.GetValue(__instance);

            FieldInfo stunTimeField = typeof(HandGrabber).GetField("_stunTime", BindingFlags.NonPublic | BindingFlags.Instance);
            float InstanceStunTime = (float)stunTimeField.GetValue(__instance);

            MethodInfo createJointMethod = typeof(HandGrabber).GetMethod("CreateJoint", BindingFlags.NonPublic | BindingFlags.Instance);

            if (InstanceGrabDummy != null) {
                Collider[] foundColliders = Physics.OverlapSphere(InstanceGrabDummy.position, 0.15f);
                for (int i = 0; i < foundColliders.Length; i++) {
                    Rigidbody foundRigid = foundColliders[i].GetComponentInParent<Rigidbody>();
                    if (foundRigid == null) continue;

                    Unit foundUnit = foundRigid.GetComponentInParent<Unit>();
                    if (foundUnit == null || foundUnit == InstanceUnit) continue;

                    CharacterMovement foundMovement = foundUnit.Movement;
                    InstanceUnit?.Movement?.ReactToSmell(foundMovement);
                    foundMovement?.AddAttachedEnemy(InstanceUnit.Movement);

                    enemyGrabbedOntoField?.SetValue(__instance, foundMovement);
                    createJointMethod?.Invoke(__instance, new object[] { foundRigid });
                    InstanceGrabSound?.Play(__instance.transform.position);

                    if (InstanceStunTime <= 0f) {
                        foundMovement?.SetStunTime(InstanceStunTime);
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
