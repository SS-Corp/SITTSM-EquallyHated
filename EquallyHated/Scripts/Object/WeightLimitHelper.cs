using System.Collections;
using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class WeightLimitHelper : MonoBehaviour {
        static GameObject messagerAsset;
        public WeightLimitMessage currentMessager;
        public Vector3 messagerOffset = new Vector3(0, 2.3f, -2f);

        private bool canShowMessage = true;

        public WeightLimitHelper() {
            messagerAsset = AssetsHandler.weightlimit.LoadAsset<GameObject>("WeightLimitBubble");
        }

        public WeightLimitMessage GetMessager() {
            if (currentMessager == null) {
                currentMessager = CreateMessager();
            }

            return currentMessager;
        }
        public WeightLimitMessage CreateMessager() {
            if (messagerAsset == null) {
                messagerAsset = AssetsHandler.weightlimit.LoadAsset<GameObject>("WeightLimitBubble");
            }

            GameObject messagerObject = Instantiate(messagerAsset, transform.position, Quaternion.identity, transform);
            WeightLimitMessage messager = messagerObject.GetComponent<WeightLimitMessage>();

            messagerObject.transform.localPosition = messagerOffset;

            return messager;
        }

        public void ShowMessage() {
            if (!canShowMessage) return;
            canShowMessage = false;

            GetMessager().ShowMessage();

            StartCoroutine(ReturnCanShowMessage());
        }

        private IEnumerator ReturnCanShowMessage() {
            yield return new WaitForSeconds(0.05f);

            canShowMessage = true;
        }
    }
}
