using System.Reflection;
using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class MusicManagerModded : MonoBehaviour {
        public AudioSource audioSourceMain;

        protected virtual void Start() {
            FieldInfo mainAudioSourceField = typeof(MusicManager).GetField("_audioSourceCurrent", BindingFlags.NonPublic | BindingFlags.Instance);
            AudioSource mainAudioSourceInstance = (AudioSource)mainAudioSourceField.GetValue(MusicManager.Instance);

            audioSourceMain = mainAudioSourceInstance;
            audioSourceMain.loop = false;
        }
    }
}
