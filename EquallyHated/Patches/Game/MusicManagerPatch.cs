using UnityEngine;
using HarmonyLib;
using S.EquallyHated.Scripts;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(MusicManager))]
    internal class MusicManagerPatch {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(MusicManager __instance) {
            CheckMusicHelpers(__instance);
        }

        static void CheckMusicHelpers(MusicManager __instance) {
            if (__instance.gameObject.scene.name == "TitleScene") {
                MusicManagerTitleScreen musicTitleScreen = __instance.gameObject.AddComponent<MusicManagerTitleScreen>();
            }
        }
    }
}
