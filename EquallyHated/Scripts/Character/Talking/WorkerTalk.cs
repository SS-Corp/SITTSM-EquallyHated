using System.Collections;
using UnityEngine;

namespace S.EquallyHated.Scripts {
    public abstract class WorkerTalk : MonoBehaviour {
        protected Unit unit;
        protected TalkingHead talkHead;

        protected AudioSource talkAudioSource;

        public virtual void SetupUnit(Unit unit) {
            this.unit = unit;
            this.talkHead = unit.TalkingHead;
            this.talkAudioSource = unit.TalkingHead.gameObject.AddComponent<AudioSource>();
        }
    }
}
