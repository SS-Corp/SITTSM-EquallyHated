using UnityEngine;
using HarmonyLib;
using System;
using System.Reflection;
using S.EquallyHated.Scripts;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(ExperienceUI))]
    internal class ExperienceUIPatch {

        [HarmonyPatch(nameof(ExperienceUI.AddExperience))]
        [HarmonyPostfix]
        static void AddExperiencePostfix(ExperienceUI __instance) {
            if (PlayerManager.Main.stats.RunStats.XPPoints >= PlayerManager.Main.stats.RunStats.MaxXPPoints) {
                FieldInfo lastPromotionTimeField = typeof(ExperienceUI).GetField("_lastPromotionTime", BindingFlags.NonPublic | BindingFlags.Instance);
                PromotionDoor promotionDoor = __instance.CalculateNextPromotionDoor(UnitManager.Instance.GetHeroCenter());

                PlayerManager.Main.stats.RunStats.SetXpPoints(0);  // restarts xp bar
                PresentSpawnerPatch.SpawnPresent(); //spawn
                lastPromotionTimeField.SetValue(__instance, Time.time); // brhu

                if (!__instance.AnyPromotionDoorsActive) { //already door
                    __instance.RevealPromotionDoor(promotionDoor); // open door
                }
            }
        }
    }
}
