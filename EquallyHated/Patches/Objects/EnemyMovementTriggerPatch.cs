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

            FieldInfo damageDataField = typeof(EnemyMovementTrigger).GetField("_damageData", BindingFlags.NonPublic | BindingFlags.Instance);
            DamageData damageDataInstance = (DamageData)damageDataField.GetValue(__instance);

            if (lifeInstance < effectTimeMInstance) return;

            foreach (Collider collider in Physics.OverlapSphere(__instance.transform.position + offsetInstance, rangeInstance)) {
                CharacterMovement movement = collider.GetComponentInParent<CharacterMovement>();
                if (movement != null && (ownerInstance == null || movement != ownerInstance.Movement) && movement.Health.IsAlive && !movement.Health.CharacterOnFire.IsBurning && (!alreadyHitCharactersInstance.Contains(movement))) {
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

                    alreadyHitCharactersInstance.Add(movement);
                    //yeah im not going anything else
                }
            }
        }
    }
}
