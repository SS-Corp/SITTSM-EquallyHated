using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class FloorModded : MonoBehaviour {
        public FloorHolder floor;

        protected virtual void Start() {
            floor = GetComponent<FloorHolder>();
        }

        protected virtual void Update() {

        }
    }
}
