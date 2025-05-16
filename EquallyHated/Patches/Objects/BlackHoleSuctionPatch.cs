using UnityEngine;
using HarmonyLib;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(BlackHoleSuction))]
    internal class BlackHoleSuctionPatch {
        
        [HarmonyPatch("SuckEnemies")]
        [HarmonyPostfix]
        static void SuckPostfix(BlackHoleSuction __instance, CharacterMovement character, Vector3 worldPoint, float range, float force) {
            Collider[] suckedColliders = Physics.OverlapSphere(worldPoint, range);

            foreach (Collider sucked in suckedColliders) {
                Health health = sucked.GetComponentInParent<Health>();
                if (health == null || health.Unit.Movement == character) continue;
                CharacterMovement movement = health.Unit.Movement;

                Vector3 suckDestination = movement.Torso.worldCenterOfMass - worldPoint;
                Vector3 suckForce = -1f / suckDestination.magnitude * suckDestination * __instance.suctionForce * Time.deltaTime;

                //K
                movement.AddVelocityToAllBodies(suckForce * 0.1f);
            }


        }
    }
}
