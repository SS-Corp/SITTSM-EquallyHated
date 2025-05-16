using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class MusicManagerTitleScreen : MusicManagerModded {
        //THIS LOOPS SOO GOOD ALL OF THEM
        public static AudioClip JingleLow;
        public static AudioClip JingleMid;
        public static AudioClip JingleHigh;

        public MusicManagerTitleScreen() {
            JingleLow = AssetsHandler.reusedAssets.LoadAsset<AudioClip>("Corporate Jingle - Low");
            JingleMid = AssetsHandler.reusedAssets.LoadAsset<AudioClip>("Corporate Jingle - Mid");
            JingleHigh = AssetsHandler.reusedAssets.LoadAsset<AudioClip>("Corporate Jingle - High");
        }

        protected override void Start() {
            base.Start();

            PlayRandomJingle();
        }
        protected virtual void Update() {
            CheckMusicUpdate();
        }
        protected virtual void CheckMusicUpdate() {
            if (audioSourceMain == null) return;
            
            if (!audioSourceMain.isPlaying) {
                PlayRandomJingle();
            }
        }

        static void PlayRandomJingle() {
            AudioClip[] jingles = { JingleLow, JingleMid, JingleHigh };
            int randomIndex = Random.Range(0, jingles.Length);
            AudioClip selectedJingle = jingles[randomIndex];

            MusicManager.Instance.PerformMusicTransition(selectedJingle, selectedJingle, null, 0);
        }
    }
}
