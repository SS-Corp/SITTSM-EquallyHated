using UnityEngine;
using HarmonyLib;
using System;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(CameraShake))]
    internal class CameraShakePatch {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        static void StartPostFix(CameraShake __instance) {
            SetupRigidCam(__instance);
            MainUICanvasPatch.SetUpWorldSpaceUI(MainUICanvas.Instance); //idk beter
        }

        [HarmonyPatch(nameof(CameraShake.ShakeRumble), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) })]
        [HarmonyPrefix]
        static bool ShakeRumblePrefix(CameraShake __instance, float duration, float strengthM, float speedM, float distanceM = 1f) {
            Rigidbody camRigid = __instance.GetComponent<Rigidbody>();
            if (camRigid == null) return true;

            Vector3 randomForce = new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));

            strengthM *= LevelManager.Level.screenShakeM;
            strengthM *= 1.25f;
            randomForce *= strengthM;

            camRigid.AddForceAtPosition(randomForce, __instance.transform.position, ForceMode.Impulse);
            return false;
        }

        [HarmonyPatch(nameof(CameraShake.ShakeY), new Type[] { typeof(float), typeof(float), typeof(float), typeof(bool) })]
        [HarmonyPrefix]
        static bool ShakeYPrefix(CameraShake __instance, float strengthM) {
            Rigidbody camRigid = __instance.GetComponent<Rigidbody>();
            if (camRigid == null) return true;

            Vector3 randomForce = Vector3.one * UnityEngine.Random.Range(-1, 1);
            
            strengthM *= LevelManager.Level.screenShakeM;
            strengthM *= 1.25f;
            randomForce *= strengthM;

            camRigid.AddForceAtPosition(randomForce, __instance.transform.position, ForceMode.Impulse);
            return false;
        }
        [HarmonyPatch(nameof(CameraShake.ShakeY), new Type[] { typeof(Vector3), typeof(float), typeof(float)})]
        [HarmonyPrefix]
        static bool ShakeYPrefix(CameraShake __instance, Vector3 position, float strengthM) {
            Rigidbody camRigid = __instance.GetComponent<Rigidbody>();
            if (camRigid == null) return true;

            Vector3 randomForce = Vector3.one * UnityEngine.Random.Range(-1, 1);

            strengthM *= LevelManager.Level.screenShakeM;

            randomForce *= strengthM;

            camRigid.AddForceAtPosition(randomForce, position, ForceMode.Impulse);
            return false;
        }

        public static void SetupRigidCam(CameraShake __instance) {
            GameObject cameraObject = __instance.gameObject;
            Rigidbody cameraRigid = cameraObject.AddComponent<Rigidbody>();
            ConfigurableJoint cameraJoint = cameraObject.AddComponent<ConfigurableJoint>();

            GameObject camHolder = cameraObject.transform.parent.gameObject;
            Rigidbody camHolderRigid = camHolder.AddComponent<Rigidbody>();
            camHolderRigid.isKinematic = true;

            //joint
            JointDrive configuredDrive = new JointDrive() {
                positionSpring = 205,
                positionDamper = 5,
                maximumForce = Mathf.Infinity
            };

            cameraJoint.autoConfigureConnectedAnchor = true; //FUCK!!!!!!!!!!!!!
            cameraJoint.connectedBody = camHolderRigid;

            cameraJoint.xDrive = configuredDrive;
            cameraJoint.yDrive = configuredDrive;
            cameraJoint.zDrive = configuredDrive;
            cameraJoint.angularXDrive = configuredDrive;
            cameraJoint.angularYZDrive = configuredDrive;
            cameraJoint.slerpDrive = configuredDrive;

            __instance.enabled = false;

            EquallyHatedModBase.Logger.LogInfo("made camera rigid!");
        }
    }
}
