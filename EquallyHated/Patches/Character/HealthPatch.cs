using UnityEngine;
using HarmonyLib;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(Health))]
    internal class HealthPatch {
        [HarmonyPatch("Die")]
        [HarmonyPostfix]
        static void DiePostfix(Health __instance) {
            Unit unit = __instance.Unit;
            CharacterMovement movement = unit.Movement;
            if (__instance.Unit.Faction == FactionType.Hero || __instance.IsBoss) {
                CameraShake camShake =  CameraShake.Instance;
                if (camShake == null || !camShake.GetComponent<Rigidbody>()) return;

                Rigidbody camRigid = camShake.GetComponent<Rigidbody>();

                Vector3 suckForce = movement.Torso.worldCenterOfMass - camRigid.worldCenterOfMass;
                suckForce *= LevelManager.Level.screenShakeM;
                //suckForce *= 2.5f;

                camRigid.AddForce(suckForce, ForceMode.Impulse);
            }
        }
    }
}
