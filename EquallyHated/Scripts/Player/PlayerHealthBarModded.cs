using UnityEngine;

namespace S.EquallyHated.Scripts {
    public class PlayerHealthBarModded : MonoBehaviour {
        public PlayerHealthBar healthBar { get; private set; }
        public bool showingCustomOvertime { get; private set; }

        //english only sory
        internal string[] overtimeMessages = new string[] {
            "OVERTIME",
            "WATCH OUT",
            "CLOSE ONE",
            "THAT HURTS YOU",
            "LAST CHANCE",
            "OKAYYY",
            "NOT CREDIT TAKING",
            "NICE DODGE DUDEEE",
            "I SHOULD JUST PUT YOU DOWN!",
            "KYS",
            "STOP BEING RISKY!"
        };
        private int overtimeIndex = 0;

        public void SetupForHealthBar(PlayerHealthBar healthBar) {
            this.healthBar = healthBar;
        }

        public void SetShowingOvertime(bool showing) {
            showingCustomOvertime = showing;
        }
        public string GetOvertimeMessage() {
            string currentMessage = overtimeMessages[overtimeIndex];

            overtimeIndex++;
            if (overtimeIndex > overtimeMessages.Length - 1) {
                overtimeIndex = UnityEngine.Random.Range(0, overtimeMessages.Length - 1);
            }

            return currentMessage;
        }
        public void ResetOvertimeMessages() {
            overtimeIndex = 0;
        }
    }
}
