using System.Collections;
using TMPro;
using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class WeightLimitMessage : MonoBehaviour {
        public float messageTime = 5;
        public float messageLerpSpeed = .25f;
        public AnimationCurve messageLerp = AnimationCurve.Linear(0,0,1,1);
        public string[] messages;
        public AudioClip[] messageAudios;
        private bool _messageShown;
        private float _messageTimer;
        private int _messageIndex;
        [Space]
        public TMP_Text text;
        private AudioSource audioSource;
        
        private void Awake() {
            audioSource = GetComponent<AudioSource>();
        }
        private void Update() {
            CheckHideMessage();
        }

        private void CheckHideMessage() {
            if (!_messageShown) return;
            if (_messageTimer > 0) {
                _messageTimer -= Time.deltaTime;
            }
            else {
                UnshowingMessage();
            }
        }

        public void ShowMessage() {
            string message = messages[_messageIndex];
            AudioClip messageAudio = messageAudios[_messageIndex];

            text.text = message;
            audioSource.PlayOneShot(messageAudio);

            _messageIndex++;
            if (_messageIndex > messages.Length - 1) {
                _messageIndex = 0;
            }

            _messageShown = true;
            _messageTimer = messageTime;

            gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            gameObject.SetActive(true);
            StartCoroutine(ScaleMessage(new Vector3(1, 1, 1), messageLerpSpeed));
        }
        public void UnshowingMessage() {
            _messageShown = false;
            StartCoroutine(ScaleMessage(new Vector3(0.05f, 0.05f, 0.05f), messageLerpSpeed));
        }

        private IEnumerator ScaleMessage(Vector3 targetScale, float duration) {
            float timeElapsed = 0;
            Vector3 startScale = transform.localScale;

            while (timeElapsed < duration) {
                float t = timeElapsed / duration;
                messageLerp.Evaluate(t);

                transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = targetScale;

            gameObject.SetActive(_messageShown);
        }

    }
}
