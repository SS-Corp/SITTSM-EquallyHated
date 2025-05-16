using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;

namespace S.EquallyHated.Patches {
    [HarmonyPatch(typeof(SendAnimationMessages))]
    internal class SendAnimationMessagesPatch {
        [HarmonyPatch(nameof(SendAnimationMessages.CauseSleepiness))]
        [HarmonyPostfix]
        static void CauseSleepinessPostfix(SendAnimationMessages __instance, AnimationMessageData data) {
            Vector3 headPoint = __instance.Movement.Head.worldCenterOfMass;
            Collider[] nearbyColliders = Physics.OverlapBox(headPoint, new Vector3(data.range / 2f, 1f, Mathf.Max(data.range / 2f, 2f)), Quaternion.identity);
            List<Unit> targetedUnits = new List<Unit>();
            //checker
            foreach (Collider collider in nearbyColliders) {
                Unit unit = collider.GetComponentInParent<Unit>();
                if (unit != null && unit != __instance.Movement.Unit) {
                    if (unit.Health.IsAlive && !unit.Movement.IsInAir(1f, 0f)) {
                        targetedUnits.Add(unit);
                    }
                }
            }
			//IDK WHAT THIS IS BUT IM ADDING IT STEALING CODE!!!!!!!
			if (targetedUnits.Count > 0 && data.extraSpecialEffect && data.specialMovementState != null) {
				int num = -1;
				float num2 = 0f;
				for (int j = 0; j < targetedUnits.Count; j++) {
					if (targetedUnits[j].Health.IsAlive && !targetedUnits[j].Movement.IsInAir(1f, 0f)) {
						float num3 = Mathf.Abs(targetedUnits[j].Movement.Torso.worldCenterOfMass.x - __instance.Movement.Torso.worldCenterOfMass.x);
						if (num3 > num2) {
							num2 = num3;
							num = j;
						}
					}
				}
				if (num >= 0) {
					if (targetedUnits[num].Movement.TryImposeMovementState(data.specialMovementState, true)) {
						targetedUnits[num].Movement.SetStunTime(data.time + 2f);
					}
					targetedUnits.RemoveAt(num);
				}
				if (targetedUnits.Count > 3) {
					int count = targetedUnits.Count;
					int num4 = 0;
					while ((float)num4 < (float)(count - 1) / 3.1f) {
						int index = UnityEngine.Random.Range(0, targetedUnits.Count);
						if (targetedUnits[index].Movement.TryImposeMovementState(data.specialMovementState, true)) {
							targetedUnits[index].Movement.SetStunTime(data.time + 2f);
						}
						targetedUnits.RemoveAt(index);
						num4++;
					}
				}
			}

			//do it
			foreach (Unit unit in targetedUnits) {
                if (unit.Health.IsAlive && !unit.Movement.IsInAir(1f, 0f) && unit.Movement.Torso.velocity.y < 1f && !unit.Movement.IsStunned && unit.Movement.TryImposeMovementState(data.playMovementState, true)) {
                    unit.Movement.SetStunTime(data.time);
                    unit.Health.SetVulnerable(data.time);
                }
            }
        }
    }
}
