using System.Collections;
using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class WorkerFloorIntro : WorkerTalk {
        private AudioClip hateAudioClip;

        public override void SetupUnit(Unit unit) {
            base.SetupUnit(unit);

            hateAudioClip = AssetsHandler.towerAssets.LoadAsset<AudioClip>("worker- i hate my supervisor");
            StartCoroutine(IntroHate());
        }

        public IEnumerator IntroHate() {
            yield return new WaitForSeconds(2.5f);

            talkHead.Talk("i hate my supervisor!");
            talkAudioSource.PlayOneShot(hateAudioClip);
        }
    }
}
