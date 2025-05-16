using UnityEngine;
using HarmonyLib;
using System.Reflection;
using Framework;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(TrapElectrical))]
    internal class TrapElectricalPatch {
		static float _knocbackPower = 55;
		static Vector3 _knockbackAdd = new Vector3(0, .25f, 0);

        [HarmonyPatch("TryShockBasedOnCollision")]
		[HarmonyPrefix]
		static bool TryShockBasedOnCollisionPrefix(TrapElectrical __instance, Collision collision, ref bool __result) {
			FieldInfo damageInfoField = typeof(TrapElectrical).GetField("_damageData", BindingFlags.NonPublic | BindingFlags.Instance);
			DamageData InstanceDamageData = (DamageData)damageInfoField.GetValue(__instance);
			FieldInfo damageAreaField = typeof(TrapElectrical).GetField("_damageArea", BindingFlags.NonPublic | BindingFlags.Instance);
			Collider InstanceDamageArea = (Collider)damageAreaField.GetValue(__instance);
			FieldInfo lastDamagedHealthField = typeof(TrapElectrical).GetField("_lastDamagedHealth", BindingFlags.NonPublic | BindingFlags.Instance);
			Health InstanceLastDamagedHealth = (Health)lastDamagedHealthField.GetValue(__instance);
			FieldInfo lastDamageTimeField = typeof(TrapElectrical).GetField("_lastDamageTime", BindingFlags.NonPublic | BindingFlags.Instance);
			float InstanceLastDamageTime = (float)lastDamageTimeField.GetValue(__instance);
			FieldInfo arcTransformsField = typeof(TrapElectrical).GetField("_arcTransforms", BindingFlags.NonPublic | BindingFlags.Instance);
			Transform[] InstanceArcTransforms = (Transform[])arcTransformsField.GetValue(__instance);

			//ok
			Health foundHealth = collision.gameObject.GetComponentInParent<Health>();

			if (foundHealth != null && foundHealth.IsAlive && (foundHealth != InstanceLastDamagedHealth || Time.time - InstanceLastDamageTime > 2) && InstanceDamageArea.IsColliding(collision.collider)) {
				foundHealth.TakeDamage(InstanceDamageData.GetDamageObject((foundHealth.LastUnitToCauseDamage != null) ? foundHealth.LastUnitToCauseDamage.Hero : SingletonBehaviour<UnitManager>.Instance.GetHero().Hero, collision.contacts[0].point, collision.contacts[0].normal * 2f, foundHealth));
				GameObject zap1 = new GameObject();
				zap1.transform.parent = __instance.transform;
				zap1.transform.localPosition = InstanceArcTransforms[0].localPosition.WithY(0.25f + UnityEngine.Random.value * 1.2f);
				GameObject zap2 = new GameObject();
				zap2.transform.parent = __instance.transform;
				zap2.transform.localPosition = InstanceArcTransforms[1].localPosition.WithY(0.25f + UnityEngine.Random.value * 1.2f);
				SingletonBehaviour<EffectsController>.Instance.CreateArcLineElectrical(zap1.transform, collision.collider.transform);
				SingletonBehaviour<EffectsController>.Instance.CreateArcLineElectrical(collision.collider.transform, zap2.transform);
				SingletonBehaviour<EffectsController>.Instance.CreateArcLineElectrical(zap1.transform, zap2.transform);
				SingletonBehaviour<EffectsController>.Instance.MakeWhiteElectricSparks(collision.contacts[0].point + collision.contacts[0].normal * 0.04f, 0.05f, __instance.transform.forward * -12f + Vector3.up * 6f, 6f, UnityEngine.Random.Range(3, 6), 1f);
				UnityEngine.Object.Destroy(zap1, 1.5f);
				UnityEngine.Object.Destroy(zap2, 1.5f);

				//knockback
				if (collision.rigidbody) {
					Rigidbody rigid = collision.rigidbody;
					Vector3 KnockbackForce = (-__instance.transform.forward + _knockbackAdd) * _knocbackPower;

					rigid.AddForce(KnockbackForce, ForceMode.Impulse);
                }

				lastDamageTimeField.SetValue(__instance, Time.time);
				lastDamagedHealthField.SetValue(__instance, foundHealth);

				__result = true;
				return false; // skip og
			}
			return true; // continue
		}
    }
}
