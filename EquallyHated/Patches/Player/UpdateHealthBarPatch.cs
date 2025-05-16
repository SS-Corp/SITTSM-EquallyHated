using UnityEngine;
using HarmonyLib;
using System.Reflection;
using TMPro;
using S.EquallyHated.Scripts;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(PlayerHealthBar))]
    internal class UpdateHealthBarPatch {
        
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void AwakePostfix(PlayerHealthBar __instance) {
            PlayerHealthBarModded healthBarHelper = __instance.gameObject.AddComponent<PlayerHealthBarModded>();

            healthBarHelper.SetupForHealthBar(__instance);
        }

        [HarmonyPatch(nameof(PlayerHealthBar.UpdateHealthBar))]
        [HarmonyPostfix]
        static void UpdateHealthBarPostfix(PlayerHealthBar __instance, Unit unit) {
            PlayerHealthBarModded healthBarHelper = __instance.GetComponent<PlayerHealthBarModded>();
            float normalizedHealth = unit.Health.CurrentHealth / unit.Health.maxHealth;

            FieldInfo overtimeTextField = typeof(PlayerHealthBar).GetField("_overtimeText", BindingFlags.NonPublic | BindingFlags.Instance);
            TextMeshProUGUI overtimeTextInstance = (TextMeshProUGUI)overtimeTextField.GetValue(__instance);

            if (unit.Health.IsInOverTime) {
                if (healthBarHelper != null) {
                    if (!healthBarHelper.showingCustomOvertime) {
                        overtimeTextInstance.text = healthBarHelper.GetOvertimeMessage();
                        healthBarHelper.SetShowingOvertime(true);
                    }
                }
            }
            if (unit.Health.CurrentHealth <= 0) {
                if (healthBarHelper != null) {
                    healthBarHelper.ResetOvertimeMessages(); //reset cuz yeah;
                }
            }
            if (healthBarHelper != null) {
                if (healthBarHelper.showingCustomOvertime) {
                    healthBarHelper.SetShowingOvertime(false);
                }
            }
        }

    }
}
