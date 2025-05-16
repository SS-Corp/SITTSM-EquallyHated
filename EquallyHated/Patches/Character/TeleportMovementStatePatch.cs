using UnityEngine;
using HarmonyLib;
using System;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(TeleportMovementState))]
    internal class TeleportMovementStatePatch {
        [HarmonyPatch("KnockAwayEnemies")]
        [HarmonyPostfix]
        static void KnockAwayPostfix(TeleportMovementState __instance, CharacterMovement character, Vector3 worldDodgePoint, float range, float force, DamageData damageData, float damageRange) {
            Collider[] knockedColliders = Physics.OverlapSphere(worldDodgePoint, range);

            foreach (Collider knocked in knockedColliders) {
                Health health = knocked.GetComponentInParent<Health>();
                if (health == null || health.Unit.Movement == character) continue;
                CharacterMovement movement = health.Unit.Movement;

                Vector3 knockedDestination = movement.Torso.worldCenterOfMass - worldDodgePoint;
                float knockedMagnitude = knockedDestination.magnitude;
                float d = (float)(1 + ((knockedMagnitude < range * 0.6f) ? 1 : 0)); //decomplied thing IDK name yes

                health.TakeDamage(new Damage(character.Unit, 0f, DamageType.Force, null, worldDodgePoint, (knockedDestination.normalized * d * force), null, force, null, false));
                movement.HeadBecomeFragile(true);
            }
        }
    }
}
