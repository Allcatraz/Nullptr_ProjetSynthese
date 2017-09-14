using Harmony.EventSystem;
using Harmony.Injection;

namespace ProjetSynthese
{
    public class ScoreEventPublisher : GameScript
    {
        private Score score;
        private ScoreEventChannel eventChannel;

        public void InjectScoreEventPublisher([EntityScope] Score score,
                                              [EventChannelScope] ScoreEventChannel eventChannel)
        {
            this.score = score;
            this.eventChannel = eventChannel;
        }

        public void Awake()
        {
            InjectDependencies("InjectScoreEventPublisher");
        }

        public void OnEnable()
        {
            score.OnScoreChanged += OnScoreChanged;
            eventChannel.OnUpdateRequested += OnRequestUpdate;
        }

        public void OnDisable()
        {
            score.OnScoreChanged -= OnScoreChanged;
            eventChannel.OnUpdateRequested -= OnRequestUpdate;
        }

        private void OnRequestUpdate(EventChannelUpdateHandler<ScoreUpdate> updateHandler)
        {
            updateHandler(new ScoreUpdate(score));
        }

        private void OnScoreChanged(uint oldScorePoints, uint newScorePoints)
        {
            eventChannel.Publish(new ScoreEvent(score, oldScorePoints, newScorePoints));
        }
    }
}