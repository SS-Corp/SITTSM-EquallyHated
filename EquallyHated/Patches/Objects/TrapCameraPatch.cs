using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(TrapCamera))]
    internal class TrapCameraPatch {
        [HarmonyPatch("SeesPlayer")]
        [HarmonyPrefix]
        static bool SeesPlayerPrefix(TrapCamera __instance, ref bool __result, float distance) { // NUH UH EVERYONE DIES!!!
            FieldInfo firePointField = typeof(TrapCamera).GetField("_firePoint", BindingFlags.NonPublic | BindingFlags.Instance);
            Transform InstanceFirePoint = (Transform)firePointField.GetValue(__instance);
            FieldInfo cameraJointField = typeof(TrapCamera).GetField("_cameraJoint", BindingFlags.NonPublic | BindingFlags.Instance);
            Transform InstanceCameraJoint = (Transform)cameraJointField.GetValue(__instance);

            RaycastHit raycastHit;
            __result = Physics.Raycast(InstanceFirePoint.position, InstanceCameraJoint.forward, out raycastHit, distance) && raycastHit.collider.gameObject.GetComponentInParent<Unit>() != null;
            return false; // skip
        }
    }
}
