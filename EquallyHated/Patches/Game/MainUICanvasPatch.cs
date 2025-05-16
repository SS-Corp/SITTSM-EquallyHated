using UnityEngine;
using HarmonyLib;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(MainUICanvas))]
    internal class MainUICanvasPatch {

        public static void SetUpWorldSpaceUI(MainUICanvas __instance) {
            Canvas canvas = __instance.GetComponent<Canvas>();
            Transform canvasTransform = __instance.transform;

            canvas.renderMode = RenderMode.WorldSpace;
            canvasTransform.parent = CameraShake.Instance.transform.parent;
            canvasTransform.localPosition = Vector3.zero;
            canvasTransform.localScale = Vector3.one * .011f;
        }
    }
}
