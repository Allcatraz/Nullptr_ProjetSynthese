using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace ProjetSynthese
{
    public class TimerController : GameScript
    {

        private Text text;
        private DeathCircleTimeLeftEventChannel deathCircleTimeLeftEventChannel;

        private void InjectTimerController([GameObjectScope] Text text,
                                           [EventChannelScope] DeathCircleTimeLeftEventChannel deathCircleTimeLeftEventChannel)
        {
            this.text = text;
            this.deathCircleTimeLeftEventChannel = deathCircleTimeLeftEventChannel;
        }

        private void Awake()
        {
            InjectDependencies("InjectTimerController");
            deathCircleTimeLeftEventChannel.OnEventPublished += OnTimeLeft;
        }

        private void OnDestroy()
        {
            deathCircleTimeLeftEventChannel.OnEventPublished -= OnTimeLeft;
        }

        private void OnTimeLeft(DeathCircleTimeLeftEvent deathCircleTimeLeftEvent)
        {
            string timeMinutes = deathCircleTimeLeftEvent.Minutes / 10 < 1 ? "0" : "";
            string timeSeconds = deathCircleTimeLeftEvent.Seconds / 10 < 1 ? "0" : "";
            string time = "Time left: " + timeMinutes + deathCircleTimeLeftEvent.Minutes + " : " + timeSeconds + deathCircleTimeLeftEvent.Seconds;
            Color timeColor = deathCircleTimeLeftEvent.IsWaitFinish ? Color.blue : Color.white;

            text.text = time;
            text.color = timeColor;
        }
    }
}