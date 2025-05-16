using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class FloorOfficeParty : FloorModded {
        protected override void Update() {
            base.Update();

            CheckFloor();
        }

        protected virtual void CheckFloor() {
            Unit hero = UnitManager.Instance.GetHero();
            FloorHolder nearestFloor = LevelGenerator.Instance.GetFloor(Mathf.Max(hero.Floor, Elevator.HighestFloor));
            if (floor == nearestFloor) {
                OnFloor();
            }
        }

        protected virtual void OnFloor() {
            // i got lazy 
            // this is supposed to shake camera to the beat
            EquallyHatedModBase.Logger.LogInfo("PARTY");
        }
    }
}
