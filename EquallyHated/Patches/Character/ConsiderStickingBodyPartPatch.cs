using UnityEngine;
using HarmonyLib;
using Framework;
using System.Reflection;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(ConsiderStickingBodyPart))]
    internal class ConsiderStickingBodyPartPatch {
        [HarmonyPatch("OnCollisionEnter")]
        [HarmonyPrefix]
        static bool OnCollisionEnterPrefix(ConsiderStickingBodyPart __instance, Collision collision) {
			FieldInfo unitField = typeof(ConsiderStickingBodyPart).GetField("_unit", BindingFlags.NonPublic | BindingFlags.Instance);
			Unit InstanceUnit = (Unit)unitField.GetValue(__instance);

			FieldInfo rigidField = typeof(ConsiderStickingBodyPart).GetField("_rigidbody", BindingFlags.NonPublic | BindingFlags.Instance);
			Rigidbody InstanceRigid = (Rigidbody)rigidField.GetValue(__instance);

			FieldInfo stickSoundField = typeof(ConsiderStickingBodyPart).GetField("_stickSound", BindingFlags.NonPublic | BindingFlags.Instance);
			EffectSoundBank InstanceStickSound = (EffectSoundBank)stickSoundField.GetValue(__instance);


			FieldInfo coveredGlueField = typeof(ConsiderStickingBodyPart).GetField("_isCoveredInGlue", BindingFlags.NonPublic | BindingFlags.Instance);
			bool InstanceCoveredGlue = (bool)coveredGlueField.GetValue(__instance);

			FieldInfo chanceStickField = typeof(ConsiderStickingBodyPart).GetField("_chanceToStick", BindingFlags.NonPublic | BindingFlags.Instance);
			float InstanceChanceStick = (float)chanceStickField.GetValue(__instance);


			if (collision.gameObject.layer == 8 && collision.rigidbody == null && !collision.gameObject.CompareTag("Ignore") && (InstanceCoveredGlue || collision.contacts[0].normal.y < 0.4f)) {
				if (!InstanceCoveredGlue || Random.value >= InstanceChanceStick) {
					if (Random.value >= 0.33f || !InstanceUnit.Movement.IsStunned) {
						return true;
					}
					PassiveManager instance = SingletonBehaviour<PassiveManager>.Instance;
					Unit lastUnitToCauseDamage = InstanceUnit.Health.LastUnitToCauseDamage;
					if (!instance.EnemiesStickToWalls((lastUnitToCauseDamage != null) ? lastUnitToCauseDamage.Hero : null)) {
						return true;
					}
				}
				if (StickBodyPartToWall.Setup(InstanceUnit.Movement, InstanceRigid, collision) != null) {
					InstanceStickSound.Play(__instance.transform.position);
				}
				return false; //skip
			}
			return true;
		}
    }
}
