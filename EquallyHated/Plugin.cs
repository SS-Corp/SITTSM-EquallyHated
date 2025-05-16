using UnityEngine;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using S.EquallyHated.Patches;

namespace S.EquallyHated {
    [BepInPlugin(modGUID, modName, modVersion)]
    public class EquallyHatedModBase : BaseUnityPlugin {
        public const string modGUID = "S.EquallyHated";
        public const string modName = "EquallyHated";
        public const string modVersion = "0.0.5";

        internal static EquallyHatedModBase Instance;
        private readonly Harmony harmony = new Harmony(modGUID);
        internal static new ManualLogSource Logger;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }

            Logger = base.Logger;

            AssetsHandler.LoadAll();
            harmony.PatchAll();

            Logger.LogInfo($"READY");
        }
    }
}
