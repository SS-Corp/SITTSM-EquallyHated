using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using Framework;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(EnemyMovementTrigger))]
    internal class EnemyMovementTriggerPatch {

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostFix(EnemyMovementTrigger __instance) {
            if (ReuseAssets.explosion == null) { // SET IT
                if (__instance.ExplosionOnFireDMGPrefab) {
                    ReuseAssets.explosion = __instance.ExplosionOnFireDMGPrefab;
                }
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void UpdatePostFix(EnemyMovementTrigger __instance) {
            DieUpdateFix(__instance);
            CollisionUpdateFix(__instance);
        }

        static void DieUpdateFix(EnemyMovementTrigger __instance) {
            FieldInfo lifeField = typeof(EnemyMovementTrigger).GetField("_life", BindingFlags.NonPublic | BindingFlags.Instance);
            float lifeInstance = (float)lifeField.GetValue(__instance);

            if (lifeInstance <= 0) {
                if (!__instance.IsFire) {
                    EquallyHatedModBase.Logger.LogInfo("BOOM" + __instance.gameObject.name);
                    __instance.StartCoroutine(__instance.ExplodeFart(.35f));
                    //ExplodeTrigger(__instance);
                }
            }
        }
        static void CollisionUpdateFix(EnemyMovementTrigger __instance) {
            FieldInfo lifeField = typeof(EnemyMovementTrigger).GetField("_life", BindingFlags.NonPublic | BindingFlags.Instance);
            float lifeInstance = (float)lifeField.GetValue(__instance);
            FieldInfo effectTimeMField = typeof(EnemyMovementTrigger).GetField("_effectTimeM", BindingFlags.NonPublic | BindingFlags.Instance);
            float effectTimeMInstance = (float)effectTimeMField.GetValue(__instance);

            FieldInfo ownerField = typeof(EnemyMovementTrigger).GetField("owner", BindingFlags.NonPublic | BindingFlags.Instance);
            Unit ownerInstance = (Unit)ownerField.GetValue(__instance);
            FieldInfo alreadyHitCharactersField = typeof(EnemyMovementTrigger).GetField("_alreadyHitCharacters", BindingFlags.NonPublic | BindingFlags.Instance);
            List<CharacterMovement> alreadyHitCharactersInstance = (List<CharacterMovement>)alreadyHitCharactersField.GetValue(__instance);

            FieldInfo offsetField = typeof(EnemyMovementTrigger).GetField("offset", BindingFlags.NonPublic | BindingFlags.Instance);
            Vector3 offsetInstance = (Vector3)offsetField.GetValue(__instance);
            FieldInfo rangeField = typeof(EnemyMovementTrigger).GetField("range", BindingFlags.NonPublic | BindingFlags.Instance);
            float rangeInstance = (float)rangeField.GetValue(__instance);

            FieldInfo makeHeadFragileField = typeof(EnemyMovementTrigger).GetField("_makeHeadFragile", BindingFlags.NonPublic | BindingFlags.Instance);
            bool _makeHeadFragileInstance = (bool)makeHeadFragileField.GetValue(__instance);
            FieldInfo addForceField = typeof(EnemyMovementTrigger).GetField("_addForce", BindingFlags.NonPublic | BindingFlags.Instance);
            Vector3 addForceInstance = (Vector3)addForceField.GetValue(__instance);
            FieldInfo addXTorqueField = typeof(EnemyMovementTrigger).GetField("_addXTorque", BindingFlags.NonPublic | BindingFlags.Instance);
            float addXTorqueInstance = (float)addXTorqueField.GetValue(__instance);

            FieldInfo reactionStateField = typeof(EnemyMovementTrigger).GetField("_reactionState", BindingFlags.NonPublic | BindingFlags.Instance);
            CharacterMovementState reactionStateInstance = (CharacterMovementState)reactionStateField.GetValue(__instance);

            MethodInfo TriggerReactionState = typeof(EnemyMovementTrigger).GetMethod("TriggerReactionState", BindingFlags.Instance | BindingFlags.NonPublic);

            FieldInfo damageDataField = typeof(EnemyMovementTrigger).GetField("_damageData", BindingFlags.NonPublic | BindingFlags.Instance);
            DamageData damageDataInstance = (DamageData)damageDataField.GetValue(__instance);
            FieldInfo triggerSoundField = typeof(EnemyMovementTrigger).GetField("_triggerSound", BindingFlags.NonPublic | BindingFlags.Instance);
            EffectSoundBank triggerSoundInstance = (EffectSoundBank)triggerSoundField.GetValue(__instance);

            if (lifeInstance < effectTimeMInstance) return;

            foreach (Collider collider in Physics.OverlapSphere(__instance.transform.position + offsetInstance, rangeInstance)) {
                CharacterMovement movement = collider.GetComponentInParent<CharacterMovement>();
                if (movement != null && (ownerInstance == null || movement != ownerInstance.Movement) && movement.Health.IsAlive && !movement.Health.CharacterOnFire.IsBurning && !alreadyHitCharactersInstance.Contains(movement)) {
                    if (damageDataInstance != null) {
                        if (movement.Unit.Hero != null && ownerInstance != null) {
                            movement.Health.TakeDamage(new Damage(ownerInstance, ownerInstance.Movement.GetExtraDamage() + damageDataInstance.GetDamage(movement.Unit.Hero, movement.Health), damageDataInstance.damageType, movement.Torso, __instance.transform.position, (ownerInstance != null) ? damageDataInstance.GetExtraForce(ownerInstance.Movement.FacingDirection, ownerInstance.Movement.FacingDirection, movement.Unit.Hero) : Vector3.zero, null, damageDataInstance.explosionForce, damageDataInstance, false));
                        }
                        else {
                            movement.Health.TakeDamage(new Damage(ownerInstance, damageDataInstance.GetDamage(null, movement.Health), damageDataInstance.damageType, movement.Torso, __instance.transform.position, (ownerInstance != null) ? damageDataInstance.GetExtraForce(ownerInstance.Movement.FacingDirection, ownerInstance.Movement.FacingDirection, null) : Vector3.zero, null, damageDataInstance.explosionForce, damageDataInstance, false));
                        }
                        if (ownerInstance != null) {
                            ownerInstance.Movement.HasDealtDamageTo(movement.Health);
                        }
                        if (damageDataInstance.causeStutter) {
                            SingletonBehaviour<TimeManager>.Instance.Stutter();
                        }
                    }

                    if (reactionStateInstance != null) {
                        TriggerReactionState.Invoke(__instance, new object[] { movement });
                    }

                    alreadyHitCharactersInstance.Add(movement);
                    triggerSoundInstance.Play(__instance.transform.position);
                    if (_makeHeadFragileInstance) {
                        movement.HeadBecomeFragile(true);
                    }
                    if (addForceInstance != Vector3.zero) {
                        if (!movement.IsWalkingOnXZPlane) {
                            movement.AddVelocityToAllBodies(addForceInstance.WithX(addForceInstance.x * (float)movement.FacingDirection));
                        }
                        else {
                            float num = 1f;
                            if (__instance.transform.position.z - movement.Position.z < 1f) {
                                num = -1f;
                            }
                            movement.ReleaseTorsoHeightLock();
                            movement.GetComponentInChildren<LockTorsoHeight>().ReleaseZLock();
                            movement.AddVelocityToAllBodies(addForceInstance.WithZ(addForceInstance.z * num));
                            Debug.Log("Add force " + (addForceInstance.z * num).ToString());
                        }
                    }
                    if (addXTorqueInstance != 0f && !movement.IsWalkingOnXZPlane) {
                        AddTorqueForTime.AddTorque(movement.Torso.gameObject, SnapAxis.Z, addXTorqueInstance, 0.3f, 0f);
                    }
                }
            }
        }
    }
}
